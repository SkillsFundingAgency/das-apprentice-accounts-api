using AutoFixture;
using FluentAssertions;
using SFA.DAS.ApprenticeCommitments.Api.Controllers;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using SFA.DAS.ApprenticeCommitments.Messages.Events;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Features
{
    [Binding]
    [Scope(Feature = "ConfirmApprenticeship")]
    public class ConfirmApprenticeshipSteps
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly TestContext _context;
        private readonly Apprentice _apprentice;
        private readonly Revision _revision;
        private bool ApprenticeshipConfirmed { get; set; }

        public ConfirmApprenticeshipSteps(TestContext context)
        {
            _context = context;

            _apprentice = _fixture.Create<Apprentice>();
            _revision = _fixture.Create<Revision>();
            _apprentice.AddApprenticeship(_revision);
        }

        [Given("we have an apprenticeship waiting to be confirmed")]
        public async Task GivenWeHaveAnApprenticeshipWaitingToBeConfirmed()
        {
            _revision.Confirm(new Confirmations
            {
                EmployerCorrect = true,
                TrainingProviderCorrect = true,
                ApprenticeshipDetailsCorrect = true,
                RolesAndResponsibilitiesCorrect = true,
                HowApprenticeshipDeliveredCorrect = true,
            }, _fixture.Create<DateTimeOffset>());
            _context.DbContext.Apprentices.Add(_apprentice);
            await _context.DbContext.SaveChangesAsync();
        }

        [Given("we have an apprenticeship not ready to be confirmed")]
        public async Task GivenWeHaveAnApprenticeshipNotReadyToBeConfirmed()
        {
            _context.DbContext.Apprentices.Add(_apprentice);
            await _context.DbContext.SaveChangesAsync();
        }

        [Given("a ConfirmApprenticeshipRequest stating the apprenticeship is confirmed")]
        public void GivenAConfirmApprenticeshipRequestStatingTheApprenticeshipIsConfirmed()
        {
            ApprenticeshipConfirmed = true;
        }

        [When("the apprentice confirms the apprenticeship")]
        public async Task WhenTheApprenticeConfirmsTheApprenticeship()
        {
            var command = new ConfirmApprenticeshipRequest
            {
                ApprenticeshipCorrect = ApprenticeshipConfirmed,
            };

            await _context.Api.Post(
                $"apprentices/{_apprentice.Id}/apprenticeships/{_revision.ApprenticeshipId}/revisions/{_revision.Id}/ApprenticeshipConfirmation",
                command);
        }

        [Then("the response is OK")]
        public void ThenTheResponseIsOK()
        {
            _context.Api.Response.EnsureSuccessStatusCode();
        }

        [Then("the response is BadRequest")]
        public void ThenTheResponseIsBadRequest()
        {
            _context.Api.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Then("the apprenticeship record is updated to show confirmed")]
        public void ThenTheApprenticeshipRecordIsUpdatedToShoConfirmed()
        {
            _context.DbContext.Revisions.Should().ContainEquivalentOf(new
            {
                _revision.ApprenticeshipId,
                ConfirmedOn = _context.Time.Now,
            });
        }

        [Then("the apprenticeship record is untouched")]
        public void ThenTheApprenticeshipRecordIsUntouched()
        {
            _context.DbContext.Revisions.Should().ContainEquivalentOf(new
            {
                _revision.ApprenticeshipId,
                ConfirmedOn = (DateTime?)null,
            });
        }

        [Then("the Confirmation Confirmed event is published")]
        public void ThenTheConfirmationStartedEventIsPublished()
        {
            var latest = _context.DbContext.Revisions.Single();

            _context.Messages.PublishedMessages.Should().ContainEquivalentOf(new
            {
                Message = new
                {
                    ApprenticeId = _apprentice.Id,
                    latest.ApprenticeshipId,
                    ConfirmationId = latest.Id,
                    latest.ConfirmedOn,
                    latest.CommitmentsApprovedOn,
                    latest.CommitmentsApprenticeshipId,
                }
            });
        }
    }
}