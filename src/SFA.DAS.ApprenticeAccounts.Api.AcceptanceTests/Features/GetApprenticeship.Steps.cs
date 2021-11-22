using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using SFA.DAS.ApprenticeCommitments.DTOs;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "GetApprenticeship")]
    public class GetApprenticeshipSteps
    {
        private readonly TestContext _context;
        private Fixture _fixture = new Fixture();
        private Apprentice _apprentice;
        private Revision _revision;
        private Revision _newerRevision;

        public GetApprenticeshipSteps(TestContext context)
        {
            _context = context;
            _apprentice = _fixture.Build<Apprentice>()
                .With(a => a.TermsOfUseAccepted, true)
                .Create();

            var startDate = new System.DateTime(2000, 01, 01);
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

        [Given(@"the apprenticeship exists and it's associated with this apprentice")]
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
            // the GetApprenticeship feature finds our one as the latest approval
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

        [Given(@"there is no apprenticeship")]
        public void GivenThereIsNoApprenticeship()
        {
        }

        [Given(@"the apprenticeship exists, but it's associated with another apprentice")]
        public async Task GivenTheApprenticeshipExistsButItSAssociatedWithAnotherApprentice()
        {
            var anotherApprentice = _fixture.Create<Apprentice>();
            anotherApprentice.AddApprenticeship(_revision);

            _context.DbContext.Apprentices.Add(anotherApprentice);
            _context.DbContext.Apprentices.Add(_apprentice);
            await _context.DbContext.SaveChangesAsync();
        }

        [When(@"we try to retrieve the apprenticeship")]
        public async Task WhenWeTryToRetrieveTheApprenticeship()
        {
            await _context.Api.Get($"apprentices/{_apprentice.Id}/apprenticeships/{_revision.ApprenticeshipId}");
        }

        [Then(@"the result should return ok")]
        public void ThenTheResultShouldReturnOk()
        {
            _context.Api.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Then(@"the response should match the expected apprenticeship values")]
        public async Task ThenTheResponseShouldMatchTheExpectedApprenticeshipValues()
        {
            var content = await _context.Api.Response.Content.ReadAsStringAsync();
            var a = JsonConvert.DeserializeObject<ApprenticeshipDto>(content);
            a.Should().NotBeNull();
            a.Id.Should().Be(_revision.ApprenticeshipId);
            a.CommitmentsApprenticeshipId.Should().Be(_revision.CommitmentsApprenticeshipId);
            a.EmployerName.Should().Be(_revision.Details.EmployerName);
            a.EmployerAccountLegalEntityId.Should().Be(_revision.Details.EmployerAccountLegalEntityId);
            a.TrainingProviderName.Should().Be(_revision.Details.TrainingProviderName);
            a.TrainingProviderCorrect.Should().Be(_revision.TrainingProviderCorrect);
            a.EmployerCorrect.Should().Be(_revision.EmployerCorrect);
            a.ApprenticeshipDetailsCorrect.Should().Be(_revision.ApprenticeshipDetailsCorrect);
            a.HowApprenticeshipDeliveredCorrect.Should().Be(_revision.HowApprenticeshipDeliveredCorrect);
            a.CourseName.Should().Be(_revision.Details.Course.Name);
            a.CourseLevel.Should().Be(_revision.Details.Course.Level);
            a.CourseOption.Should().Be(_revision.Details.Course.Option);
            a.PlannedStartDate.Should().Be(_revision.Details.Course.PlannedStartDate);
            a.PlannedEndDate.Should().Be(_revision.Details.Course.PlannedEndDate);
            a.CourseDuration.Should().Be(32 + 1); // Duration is inclusive of start and end months
        }

        [Then(@"the response should match the newer apprenticeship values")]
        public async Task ThenTheResponseShouldMatchTheNewerApprenticeshipValues()
        {
            var content = await _context.Api.Response.Content.ReadAsStringAsync();
            var a = JsonConvert.DeserializeObject<ApprenticeshipDto>(content);
            a.Should().NotBeNull();
            a.Id.Should().Be(_newerRevision.ApprenticeshipId);
            a.CommitmentsApprenticeshipId.Should().Be(_newerRevision.CommitmentsApprenticeshipId);
            a.EmployerName.Should().Be(_newerRevision.Details.EmployerName);
            a.EmployerAccountLegalEntityId.Should().Be(_newerRevision.Details.EmployerAccountLegalEntityId);
            a.TrainingProviderName.Should().Be(_newerRevision.Details.TrainingProviderName);
            a.TrainingProviderCorrect.Should().Be(_newerRevision.TrainingProviderCorrect);
            a.EmployerCorrect.Should().Be(_newerRevision.EmployerCorrect);
            a.ApprenticeshipDetailsCorrect.Should().Be(_newerRevision.ApprenticeshipDetailsCorrect);
            a.HowApprenticeshipDeliveredCorrect.Should().Be(_newerRevision.HowApprenticeshipDeliveredCorrect);
            a.CourseName.Should().Be(_newerRevision.Details.Course.Name);
            a.CourseLevel.Should().Be(_newerRevision.Details.Course.Level);
            a.CourseOption.Should().Be(_revision.Details.Course.Option);
            a.PlannedStartDate.Should().Be(_newerRevision.Details.Course.PlannedStartDate);
            a.PlannedEndDate.Should().Be(_newerRevision.Details.Course.PlannedEndDate);
            a.CourseDuration.Should().Be(32 + 1); // Duration is inclusive of start and end months
        }



        [Then("all revisions should have the same apprenticeship ID")]
        public async Task ThenAllCommitmentRevisionsShouldHaveTheSameApprenticeshipID()
        {
            var apprentice = await _context.DbContext.Apprentices.FindAsync(_apprentice.Id);
            apprentice.Apprenticeships.SelectMany(x => x.Revisions)
                .Should().NotBeEmpty()
                .And.OnlyContain(a => a.ApprenticeshipId == _revision.ApprenticeshipId);
        }

        [Then(@"the result should return NotFound")]
        public void ThenTheResultShouldReturnNotFound()
        {
            _context.Api.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}