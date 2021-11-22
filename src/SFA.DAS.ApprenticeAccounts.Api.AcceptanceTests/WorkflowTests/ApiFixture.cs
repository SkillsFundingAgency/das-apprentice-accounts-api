using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using NServiceBus.Testing;
using NUnit.Framework;
using SFA.DAS.ApprenticeCommitments.Application.Commands.ChangeRegistrationCommand;
using SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApprenticeAccountCommand;
using SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApprenticeshipFromRegistrationCommand;
using SFA.DAS.ApprenticeCommitments.Application.Commands.CreateRegistrationCommand;
using SFA.DAS.ApprenticeCommitments.Application.Commands.StoppedApprenticeshipCommand;
using SFA.DAS.ApprenticeCommitments.Application.Queries.ApprenticeshipsQuery;
using SFA.DAS.ApprenticeCommitments.Application.Queries.RegistrationQuery;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using SFA.DAS.ApprenticeCommitments.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;

#nullable enable

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.WorkflowTests
{
    public class ApiFixture
    {
        private protected Fixture fixture = null!;
        private protected TestContext context = null!;
        private protected HttpClient client = null!;
        protected ApprenticeCommitmentsDbContext Database { get; private set; } = null!;
        protected TestableMessageSession Messages => context.Messages;

        private protected TimeSpan TimeBetweenActions = TimeSpan.FromDays(2);

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();
            fixture.Customizations.Add(new EmailPropertyCustomisation());
            fixture.Customize<ChangeRegistrationCommand>(c => c
                .Without(p => p.CommitmentsContinuedApprenticeshipId));

            Database = Bindings.Database.CreateDbContext();
            Reset();
        }

        public void Reset()
        {
            var factory = Bindings.Api.CreateApiFactory();
            context = new TestContext();
            _ = new Bindings.Api(context);
            client = factory.CreateClient();
        }

        public async Task<CreateRegistrationCommand> CreateRegistration(
            MailAddress? email = null, string? lastName = null, DateTime? dateOfBirth = null)
        {
            email ??= fixture.Create<MailAddress>();
            lastName ??= fixture.Create<string>();
            dateOfBirth ??= fixture.Create<DateTime>();

            var create = fixture.Build<CreateRegistrationCommand>()
                .With(x => x.Email, email.ToString())
                .With(x => x.LastName, lastName)
                .With(x => x.DateOfBirth, dateOfBirth)
                .Create();

            var response = await PostCreateRegistrationCommand(create);
            response.Should().Be2XXSuccessful();

            return create;
        }

        protected async Task<HttpResponseMessage> PostCreateRegistrationCommand(CreateRegistrationCommand create)
        {
            return await client.PostValueAsync("approvals", create);
        }

        protected async Task<RegistrationResponse> GetRegistration(Guid apprenticeId)
        {
            var (response, registration) = await client.GetValueAsync<RegistrationResponse>($"registrations/{apprenticeId}");
            response.Should().Be200Ok();
            return registration;
        }

        protected async Task<CreateApprenticeAccountCommand> CreateAccount(CreateRegistrationCommand? approval = default,
            Guid? apprenticeId = default, MailAddress? email = default, DateTime? dateOfBirth = default)
        {
            approval ??= fixture.Create<CreateRegistrationCommand>();
            email ??= new MailAddress(approval.Email);
            dateOfBirth ??= approval.DateOfBirth;

            var create = fixture.Build<CreateApprenticeAccountCommand>()
                .With(p => p.ApprenticeId, (Guid id) => apprenticeId ?? id)
                .With(p => p.Email, email.ToString())
                .With(p => p.DateOfBirth, dateOfBirth)
                .With(p => p.FirstName, approval.FirstName)
                .With(p => p.LastName, approval.LastName)
                .Create();

            var response = await PostCreateAccountCommand(create);
            response.Should().Be2XXSuccessful();

            return create;
        }

        protected async Task UpdateAccount(Guid apprenticeId, string firstName, string lastName, DateTime dateOfBirth)
        {
            var response = await SendUpdateAccountRequest(apprenticeId, firstName, lastName, dateOfBirth);
            response.Should().Be2XXSuccessful();
        }

        protected async Task<HttpResponseMessage> SendUpdateAccountRequest(Guid apprenticeId, string firstName, string lastName, DateTime dateOfBirth)
        {
            var patch = new JsonPatchDocument<ApprenticeDto>()
                .Replace(x => x.FirstName, firstName)
                .Replace(x => x.LastName, lastName)
                .Replace(x => x.DateOfBirth, dateOfBirth);

            return await client.PatchValueAsync($"apprentices/{apprenticeId}", patch);
        }

        protected async Task<HttpResponseMessage> SendAcceptTermsOfUseRequest(Guid apprenticeId, bool accept = true)
        {
            var patch = new JsonPatchDocument<ApprenticeDto>()
                .Replace(x => x.TermsOfUseAccepted, accept);
            
            return await client.PatchValueAsync($"apprentices/{apprenticeId}", patch);
        }

        protected async Task<ApprenticeshipDto> CreateVerifiedApprenticeship(MailAddress? email = null)
        {
            var approval = await CreateRegistration(email);
            return await VerifyRegistration(approval);
        }

        protected async Task<ApprenticeshipDto> VerifyRegistration(CreateRegistrationCommand approval)
        {
            var account = await CreateAccount(approval);
            await VerifyRegistration(approval.RegistrationId, account.ApprenticeId);
            var apprenticeship = await GetApprenticeships(account.ApprenticeId);
            context.Time.Now = approval.CommitmentsApprovedOn;
            return apprenticeship[0];
        }

        protected async Task<ApprenticeDto> GetApprentice(Guid apprenticeId)
        {
            var (response, apprentice) = await client.GetValueAsync<ApprenticeDto>($"apprentices/{apprenticeId}");
            response.Should().Be200Ok();
            return apprentice;
        }

        protected async Task<HttpResponseMessage> PostCreateAccountCommand(CreateApprenticeAccountCommand create)
        {
            return await client.PostValueAsync("apprentices", create);
        }

        protected async Task VerifyRegistration(Guid registrationId, Guid apprenticeId)
        {
            var response = await PostVerifyRegistrationCommand(registrationId, apprenticeId);
            response.Should().Be2XXSuccessful();
        }

        protected async Task<HttpResponseMessage> PostVerifyRegistrationCommand(Guid registrationId, Guid apprenticeId)
        {
            var create = fixture.Build<CreateApprenticeshipFromRegistrationCommand>()
                             .With(p => p.RegistrationId, registrationId)
                             .With(p => p.ApprenticeId, apprenticeId)
                             .Create();

            var response = await client.PostValueAsync("apprenticeships", create);
            return response;
        }

        protected async Task<List<ApprenticeshipDto>> GetApprenticeships(Guid apprenticeId)
        {
            var (response, apprenticeships) = await client.GetValueAsync<ApprenticeshipsResponse>($"apprentices/{apprenticeId}/apprenticeships");
            response.Should().Be200Ok();
            return apprenticeships.Apprenticeships;
        }

        protected async Task ChangeOfCircumstances(ChangeRegistrationCommand command)
        {
            var response = await PutChangeOfCircumstances(command);
            response.Should().Be2XXSuccessful();
        }

        protected Task<HttpResponseMessage> PutChangeOfCircumstances(ChangeRegistrationCommand command)
            => client.PutValueAsync("approvals", command);

        protected async Task<ApprenticeshipDto> GetApprenticeship(ApprenticeshipDto apprenticeship)
        {
            var (r2, apprenticeships) = await client.GetValueAsync<ApprenticeshipsResponse>(
                $"apprentices/{apprenticeship.ApprenticeId}/apprenticeships");
            r2.EnsureSuccessStatusCode();

            apprenticeships.Should().NotBeNull();
            return apprenticeships.Apprenticeships.Last();
        }

        private protected async Task ConfirmApprenticeship(ApprenticeshipDto apprenticeship, ConfirmationBuilder confirm)
        {
            context.Time.Now = context.Time.Now.Add(TimeBetweenActions);

            apprenticeship = await GetApprenticeship(apprenticeship);

            foreach (var payload in confirm.BuildAll())
            {
                var r4 = await client.PostValueAsync(
                    $"apprentices/{apprenticeship.ApprenticeId}/apprenticeships/{apprenticeship.Id}/revisions/{apprenticeship.RevisionId}/{payload.Item1}",
                    payload.Item2);
                r4.EnsureSuccessStatusCode();
            }
        }


        protected async Task StopApprenticeship(long commitmentsApprenticeshipId, DateTime stoppedOn)
        {
            var response = await PostStopped(new StoppedApprenticeshipCommand
            {
                CommitmentsApprenticeshipId = commitmentsApprenticeshipId,
                CommitmentsStoppedOn = stoppedOn,
            });

            response.Should().Be2XXSuccessful();
        }

        protected Task<HttpResponseMessage> PostStopped(StoppedApprenticeshipCommand command)
            => client.PostValueAsync("registrations/stopped", command);
    }
}