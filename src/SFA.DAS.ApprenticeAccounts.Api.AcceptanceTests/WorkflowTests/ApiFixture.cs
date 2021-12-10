using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Commands.CreateApprenticeAccountCommand;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs;
using SFA.DAS.NServiceBus.Testing.Services;
using System;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;

#nullable enable

namespace SFA.DAS.ApprenticeAccounts.Api.AcceptanceTests.WorkflowTests
{
    public class ApiFixture
    {
        protected Fixture fixture = null!;
        protected TestContext context = null!;
        protected HttpClient client = null!;
        protected ApprenticeAccountsDbContext Database { get; private set; } = null!;
        protected TestableEventPublisher Published => context.Events;

        protected TimeSpan TimeBetweenActions = TimeSpan.FromDays(2);

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();
            fixture.Customizations.Add(new EmailPropertyCustomisation());

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

        protected async Task<CreateApprenticeAccountCommand> CreateAccount(
            Guid? apprenticeId = default, MailAddress? email = default, DateTime? dateOfBirth = default)
        {
            var create = fixture.Build<CreateApprenticeAccountCommand>()
                .With(p => p.ApprenticeId, (Guid id) => apprenticeId ?? id)
                .With(p => p.Email, (MailAddress gen) => (email ?? gen).ToString())
                .With(p => p.DateOfBirth, (DateTime gen) => dateOfBirth ?? gen)
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
        
        protected async Task<HttpResponseMessage> SendUpdateAccountRequest(Guid apprenticeId, MailAddress emailAddress)
        {
            var patch = new JsonPatchDocument<ApprenticeDto>()
                .Replace(x => x.Email, emailAddress.ToString());

            return await client.PatchValueAsync($"apprentices/{apprenticeId}", patch);
        }

        protected async Task<HttpResponseMessage> SendAcceptTermsOfUseRequest(Guid apprenticeId, bool accept = true)
        {
            var patch = new JsonPatchDocument<ApprenticeDto>()
                .Replace(x => x.TermsOfUseAccepted, accept);

            return await client.PatchValueAsync($"apprentices/{apprenticeId}", patch);
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
    }
}