using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using SFA.DAS.ApprenticeCommitments.DTOs;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Features
{
    [Binding]
    [Scope(Feature = "GetApprenticeshipRevisions")]
    public class GetApprenticeshipRevisionsSteps
    {
        private readonly TestContext _context;
        private readonly Fixture _fixture = new Fixture();
        private readonly Apprentice _apprentice;
        private readonly Revision _revision;
        private Revision _newerRevision;

        public GetApprenticeshipRevisionsSteps(TestContext context)
        {
            _context = context;
            _apprentice = _fixture.Build<Apprentice>()
                .With(e => e.TermsOfUseAccepted, true)
                .Create();

            var startDate = new DateTime(2000, 01, 01);
            _fixture.Register(() => new CourseDetails(
                _fixture.Create("CourseName"), 1, null,
                startDate, startDate.AddMonths(32), 33));

            _revision = _fixture.Build<Revision>()
                .Do(a => a.Confirm(new Confirmations
                {
                    EmployerCorrect = true,
                    TrainingProviderCorrect = true,
                    ApprenticeshipDetailsCorrect = true,
                    RolesAndResponsibilitiesCorrect = true,
                    HowApprenticeshipDeliveredCorrect = true,
                    ApprenticeshipCorrect = true,
                }, DateTime.Now))
                .Create();

            _newerRevision = _fixture.Build<Revision>()
                .Do(a => a.Confirm(new Confirmations
                {
                    TrainingProviderCorrect = true,
                    EmployerCorrect = true,
                    ApprenticeshipDetailsCorrect = true,
                    HowApprenticeshipDeliveredCorrect = true,
                }, DateTimeOffset.UtcNow))
                .Create();
        }

        [Given("the apprenticeship exists and it's associated with this apprentice")]
        public async Task GivenTheApprenticeshipExistsAndItSAssociatedWithThisApprentice()
        {
            _apprentice.AddApprenticeship(_revision);
            _context.DbContext.Apprentices.Add(_apprentice);
            await _context.DbContext.SaveChangesAsync();
        }

        [Given("many apprenticeships exists and are associated with this apprentice")]
        public async Task GivenManyApprenticeshipExistsAndAreAssociatedWithThisApprentice()
        {
            // Ensure previous approvals happened before the one we will later assert on, so
            // the GetApprenticeshipRevision feature finds our one as the latest approval
            _fixture.Register((int i) => _revision.CommitmentsApprovedOn.AddDays(-i));

            _apprentice.AddApprenticeship(_fixture.Create<Revision>());
            _apprentice.AddApprenticeship(_fixture.Create<Revision>());
            _apprentice.AddApprenticeship(_revision);
            _context.DbContext.Apprentices.Add(_apprentice);
            await _context.DbContext.SaveChangesAsync();
        }

        [Given("the apprenticeships exists, has many revisions, and is associated with this apprentice")]
        public async Task GivenTheApprenticeshipsExistsHasManyCommitmentRevisionsAndIsAssociatedWithThisApprentice()
        {
            _apprentice.AddApprenticeship(_revision);
            _context.DbContext.Apprentices.Add(_apprentice);
            await _context.DbContext.SaveChangesAsync();

            _apprentice.Apprenticeships.First().Revise(
                _revision.CommitmentsApprenticeshipId,
                _fixture.Create<ApprenticeshipDetails>(),
                _revision.CommitmentsApprovedOn.AddDays(1));

            _newerRevision = _apprentice.Apprenticeships.First().Revisions.Last();
            _newerRevision.Confirm(new Confirmations
            {
                TrainingProviderCorrect = true,
                EmployerCorrect = true,
                ApprenticeshipDetailsCorrect = true,
                HowApprenticeshipDeliveredCorrect = true,
            }, DateTimeOffset.UtcNow);

            await _context.DbContext.SaveChangesAsync();
        }

        [Given("there is no apprenticeship")]
        public void GivenThereIsNoApprenticeship()
        {
        }

        [Given("the apprenticeship exists, but it's associated with another apprentice")]
        public async Task GivenTheApprenticeshipExistsButItSAssociatedWithAnotherApprentice()
        {
            var anotherApprentice = _fixture.Create<Apprentice>();
            anotherApprentice.AddApprenticeship(_revision);

            _context.DbContext.Apprentices.Add(anotherApprentice);
            _context.DbContext.Apprentices.Add(_apprentice);
            await _context.DbContext.SaveChangesAsync();
        }

        [When("we try to retrieve the apprenticeship revisions")]
        public async Task WhenWeTryToRetrieveTheApprenticeship()
        {
            await _context.Api.Get($"apprentices/{_apprentice.Id}/apprenticeships/{_revision.ApprenticeshipId}/revisions");
        }

        [Then("the result should return ok")]
        public void ThenTheResultShouldReturnOk()
        {
            _context.Api.Response.Should().Be200Ok();
        }

        [Then("the response should match the expected apprenticeship values")]
        public void ThenTheResponseShouldMatchTheExpectedApprenticeshipValues()
        {
            var apprenticeship = _context.DbContext.Apprenticeships
                .Include(x => x.Revisions)
                .Where(x => x.Id == _revision.ApprenticeshipId)
                .First();

            _context.Api.Response
                .Should().BeAs(new
                {
                    _revision.ApprenticeshipId,
                    Revisions = apprenticeship.Revisions.Select(x => new
                    {
                        RevisionId = x.Id,
                        x.LastViewed,
                        x.Details.EmployerName,
                        x.Details.EmployerAccountLegalEntityId,
                        x.Details.TrainingProviderId,
                        x.Details.TrainingProviderName,
                        CourseName = x.Details.Course.Name,
                        CourseLevel = x.Details.Course.Level,
                        x.Details.Course.PlannedStartDate,
                        x.Details.Course.PlannedEndDate,
                        x.EmployerCorrect,
                        x.TrainingProviderCorrect,
                        x.ApprenticeshipDetailsCorrect,
                        x.HowApprenticeshipDeliveredCorrect,
                        x.RolesAndResponsibilitiesCorrect,
                        CourseDuration = 32 + 1, // Duration is inclusive of start and end months
                    }
                    ),
                });
        }

        [Then("the result should return NotFound")]
        public void ThenTheResultShouldReturnNotFound()
        {
            _context.Api.Response.Should().Be404NotFound();
        }
    }
}