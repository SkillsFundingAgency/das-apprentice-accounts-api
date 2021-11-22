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
    [Scope(Feature = "ConfirmRolesAndResponsibilities")]
    public sealed class ConfirmRolesAndResponsibilitiesSteps
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly TestContext _context;
        private readonly Apprentice _apprentice;
        private readonly Revision _revision;
        private bool? RolesAndResponsibilitiesCorrect { get; set; }        

        public ConfirmRolesAndResponsibilitiesSteps(TestContext context)
        {
            _context = context;

            _apprentice = _fixture.Create<Apprentice>();
            _revision = _fixture.Create<Revision>();
            _apprentice.AddApprenticeship(_revision);
        }

        [Given("we have an apprenticeship waiting to be confirmed")]
        public async Task GivenWeHaveAnApprenticeshipWaitingToBeConfirmed()
        {
            _context.DbContext.Apprentices.Add(_apprentice);
            await _context.DbContext.SaveChangesAsync();
        }

        [Given("we have an apprenticeship that has previously had its roles and responsibilities confirmed")]
        public async Task GivenWeHaveAnApprenticeshipThatHasPreviouslyHadItsRolesAndResponsibilitiesConfirmed()
        {
            _revision.Confirm(new Confirmations { RolesAndResponsibilitiesCorrect = true }, _fixture.Create<DateTimeOffset>());
            await GivenWeHaveAnApprenticeshipWaitingToBeConfirmed();
        }

        [Given("a ConfirmRolesAndResponsibilitiesRequest stating the roles and responsibilities are correct")]
        public void GivenAConfirmRolesAndResponsibilitiesRequestStatingTheRolesAndResponsibilitiesAreCorrect()
        {
            RolesAndResponsibilitiesCorrect = true;
        }

        [Given("a ConfirmRolesAndResponsibilitiesRequest stating the roles and responsibilities are incorrect")]
        public void GivenAConfirmRolesAndResponsibilitiesRequestStatingTheRolesAndResponsibilitiesAreIncorrect()
        {
            RolesAndResponsibilitiesCorrect = false;
        }

        [When("we send the confirmation")]
        public async Task WhenWeSendTheConfirmation()
        {
            var command = new ConfirmRolesAndResponsibilitiesRequest
            {
                RolesAndResponsibilitiesCorrect = (bool)RolesAndResponsibilitiesCorrect,
            };

            await _context.Api.Post(
                $"apprentices/{_apprentice.Id}/apprenticeships/{_revision.ApprenticeshipId}/revisions/{_revision.Id}/RolesAndResponsibilitiesConfirmation",
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
                RolesAndResponsibilitiesCorrect,
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
