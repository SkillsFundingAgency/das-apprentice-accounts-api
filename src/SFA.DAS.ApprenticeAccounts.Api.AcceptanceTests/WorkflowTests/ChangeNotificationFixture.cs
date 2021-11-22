using AutoFixture;
using AutoFixture.Dsl;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.ApprenticeCommitments.Api.Controllers;
using SFA.DAS.ApprenticeCommitments.Application.Commands.ChangeRegistrationCommand;
using SFA.DAS.ApprenticeCommitments.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.WorkflowTests
{
    public class ChangeNotificationFixture : ApiFixture
    {
        protected async Task ViewApprenticeship(ApprenticeshipDto apprenticeship)
        {
            var patch = new JsonPatchDocument<ApprenticeshipDto>()
                .Replace(a => a.LastViewed, context.Time.Now);

            var r3 = await client.PatchAsync(
                $"apprentices/{apprenticeship.ApprenticeId}/apprenticeships/{apprenticeship.Id}/revisions/{apprenticeship.RevisionId}",
                patch.GetStringContent());

            r3.EnsureSuccessStatusCode();
        }

        private protected async Task ChangeApprenticeship(ChangeBuilder change)
        {
            context.Time.Now = context.Time.Now.Add(TimeBetweenActions);
            var data = change.ChangedOn(context.Time.Now).Build();
            var r1 = await client.PutValueAsync("approvals", data);
            r1.EnsureSuccessStatusCode();
        }
    }

    internal class ConfirmationBuilder
    {
        private bool? employerCorrect = true;
        private bool? providerCorrect = true;
        private bool? detailsCorrect = true;

        internal ConfirmationBuilder AsIncomplete()
        {
            employerCorrect = providerCorrect = detailsCorrect = false;
            return this;
        }

        internal List<(string, object)> BuildAll()
        {
            var all = new List<(string, object)>();
            if (employerCorrect != null) all.Add(("EmployerConfirmation", new ConfirmEmployerRequest { EmployerCorrect = employerCorrect.Value }));
            if (providerCorrect != null) all.Add(("TrainingProviderConfirmation", new ConfirmTrainingProviderRequest { TrainingProviderCorrect = providerCorrect.Value }));
            if (detailsCorrect != null) all.Add(("ApprenticeshipDetailsConfirmation", new ConfirmApprenticeshipRequest { ApprenticeshipCorrect = detailsCorrect.Value }));
            return all;
        }

        internal ConfirmationBuilder ConfirmOnlyEmployer()
        {
            providerCorrect = detailsCorrect = null;
            employerCorrect = true;
            return this;
        }

        internal ConfirmationBuilder ConfirmOnlyProvider()
        {
            employerCorrect = detailsCorrect = null;
            providerCorrect = true;
            return this;
        }
    }

    internal class ChangeBuilder
    {
        private readonly Fixture fixture = new Fixture();
        private IPostprocessComposer<ChangeRegistrationCommand> change;

        internal ApprenticeshipDto Apprenticeship { get; }

        internal ChangeBuilder(ApprenticeshipDto apprenticeship)
        {
            this.Apprenticeship = apprenticeship;
            change = fixture.Build<ChangeRegistrationCommand>()
                .Without(x => x.CommitmentsContinuedApprenticeshipId)
                .With(x => x.CommitmentsApprenticeshipId, apprenticeship.CommitmentsApprenticeshipId);
        }

        internal ChangeBuilder ChangedOn(DateTimeOffset now)
        {
            change = change.With(p => p.CommitmentsApprovedOn, now.DateTime);
            return this;
        }

        internal ChangeRegistrationCommand Build()
        {
            return change.Create();
        }

        internal ChangeBuilder OnlyChangeEmployer()
        {
            return WithUnchangedProvider().WithUnchangedCourse();
        }

        internal ChangeBuilder OnlyChangeProvider()
        {
            return WithUnchangedEmployer().WithUnchangedCourse();
        }

        internal ChangeBuilder WithUnchangedProvider()
        {
            change = change
                .With(x => x.TrainingProviderId, Apprenticeship.TrainingProviderId)
                .With(x => x.TrainingProviderName, Apprenticeship.TrainingProviderName);
            return this;
        }

        internal ChangeBuilder WithUnchangedCourse()
        {
            change = change
                .With(x => x.CourseName, Apprenticeship.CourseName)
                .With(x => x.CourseLevel, Apprenticeship.CourseLevel)
                .With(x => x.CourseOption, Apprenticeship.CourseOption)
                .With(x => x.PlannedStartDate, Apprenticeship.PlannedStartDate)
                .With(x => x.PlannedEndDate, Apprenticeship.PlannedEndDate);
            return this;
        }

        internal ChangeBuilder WithUnchangedEmployer()
        {
            change = change
                .With(x => x.EmployerAccountLegalEntityId, Apprenticeship.EmployerAccountLegalEntityId)
                .With(x => x.EmployerName, Apprenticeship.EmployerName);
            return this;
        }
    }
}