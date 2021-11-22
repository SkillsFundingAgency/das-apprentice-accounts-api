using AutoFixture;
using FluentAssertions;
using SFA.DAS.ApprenticeCommitments.Api.Controllers;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using System;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Features
{
    [Binding]
    [Scope(Feature = "ConfirmTrainingProvider")]
    [Scope(Feature = "ConfirmEmployer")]
    [Scope(Feature = "ConfirmApprenticeshipDetails")]
    public class ConfirmTrainingProviderandOrEmployerSteps
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly TestContext _context;
        private readonly Apprentice _apprentice;
        private readonly Revision _revision;
        private bool? TrainingProviderCorrect { get; set; }
        private bool? EmployerCorrect { get; set; }
        private bool? ApprenticeshipDetailsCorrect { get; set; }
        private string endpoint;
        private object command;
        private DateTimeOffset _anytime;

        public ConfirmTrainingProviderandOrEmployerSteps(TestContext context)
        {
            _context = context;

            _apprentice = _fixture.Create<Apprentice>();
            _revision = _fixture.Create<Revision>();
            _apprentice.AddApprenticeship(_revision);
            _anytime = _fixture.Create<DateTimeOffset>();
        }

        [Given("we have an apprenticeship waiting to be confirmed")]
        public async Task GivenWeHaveAnApprenticeshipWaitingToBeConfirmed()
        {
            _context.DbContext.Apprentices.Add(_apprentice);
            await _context.DbContext.SaveChangesAsync();
        }

        [Given("we have an apprenticeship that has previously had its training provider positively confirmed")]
        public async Task GivenWeHaveAnApprenticeshipThatHasPreviouslyHadItsTrainingProviderConfirmed()
        {
            _revision.Confirm(new Confirmations { TrainingProviderCorrect = true }, _anytime);
            await GivenWeHaveAnApprenticeshipWaitingToBeConfirmed();
        }

        [Given("we have an apprenticeship that has previously had its employer positively confirmed")]
        public async Task GivenWeHaveAnApprenticeshipThatHasPreviouslyHadItsEmployerPositivelyConfirmed()
        {
            _revision.Confirm(new Confirmations { EmployerCorrect = true }, _anytime);
            await GivenWeHaveAnApprenticeshipWaitingToBeConfirmed();
        }

        [Given("we have an apprenticeship that has previously had its apprenticeship details positively confirmed")]
        public async Task GivenWeHaveAnApprenticeshipThatHasPreviouslyHadItsApprenticeshipDetailsPositivelyConfirmed()
        {
            _revision.Confirm(new Confirmations { ApprenticeshipDetailsCorrect = true }, _anytime);
            await GivenWeHaveAnApprenticeshipWaitingToBeConfirmed();
        }

        [Given("a ConfirmTrainingProviderRequest stating the training provider is correct")]
        public void GivenAConfirmTrainingProviderRequestStatingTheTrainingProviderIsCorrect()
        {
            endpoint = "TrainingProviderConfirmation";
            TrainingProviderCorrect = true;
            command = new ConfirmTrainingProviderRequest
            {
                TrainingProviderCorrect = true,
            };
        }

        [Given("a ConfirmTrainingProviderRequest stating the training provider is incorrect")]
        public void GivenAConfirmTrainingProviderRequestStatingTheTrainingProviderIsIncorrect()
        {
            endpoint = "TrainingProviderConfirmation";
            TrainingProviderCorrect = false;
            command = new ConfirmTrainingProviderRequest
            {
                TrainingProviderCorrect = false,
            };
        }

        [Given("a ConfirmEmployerRequest stating the employer is correct")]
        public void GivenAConfirmEmployerRequestStatingTheEmployerIsCorrect()
        {
            endpoint = "EmployerConfirmation";
            EmployerCorrect = true;
            command = new ConfirmEmployerRequest
            {
                EmployerCorrect = true,
            };
        }

        [Given("a ConfirmEmployerRequest stating the employer is incorrect")]
        public void GivenAConfirmEmployerRequestStatingTheEmployerIsIncorrect()
        {
            endpoint = "EmployerConfirmation";
            EmployerCorrect = false;
            command = new ConfirmEmployerRequest
            {
                EmployerCorrect = false,
            };
        }

        [Given("a ConfirmApprenticeshipDetailsRequest stating the training provider is correct")]
        public void GivenAConfirmApprenticeshipDetailsRequestStatingTheTrainingProviderIsCorrect()
        {
            endpoint = "ApprenticeshipDetailsConfirmation";
            ApprenticeshipDetailsCorrect = true;
            command = new ConfirmApprenticeshipDetailsRequest
            {
                ApprenticeshipDetailsCorrect = true,
            };
        }

        [Given("a ConfirmApprenticeshipDetailsRequest stating the training provider is incorrect")]
        public void GivenAConfirmApprenticeshipDetailsRequestStatingTheTrainingProviderIsIncorrect()
        {
            endpoint = "ApprenticeshipDetailsConfirmation";
            ApprenticeshipDetailsCorrect = false;
            command = new ConfirmApprenticeshipDetailsRequest
            {
                ApprenticeshipDetailsCorrect = false,
            };
        }

        [When("we send the confirmation")]
        public async Task WhenWeSendTheConfirmation()
        {
            await _context.Api.Post(
                $"apprentices/{_apprentice.Id}/apprenticeships/{_revision.ApprenticeshipId}/revisions/{_revision.Id}/{endpoint}",
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

        [Then("the apprenticeship record is updated")]
        public void ThenTheApprenticeshipRecordIsUpdated()
        {
            _context.DbContext.Revisions.Should().ContainEquivalentOf(new
            {
                _revision.ApprenticeshipId,
                TrainingProviderCorrect,
                EmployerCorrect,
                ApprenticeshipDetailsCorrect,
            });
        }

        [Then("the apprenticeship record remains unchanged")]
        public void ThenTheApprenticeshipRecordRemainsUnchanged()
        {
            _context.DbContext.Revisions
                .Should().ContainEquivalentOf(_revision);
        }
    }
}