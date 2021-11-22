using AutoFixture;
using AutoFixture.Kernel;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeCommitments.Application.Commands.ChangeRegistrationCommand;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using SFA.DAS.ApprenticeCommitments.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Features
{
    [Binding]
    public class ConfirmationCountdownSteps
    {
        private readonly TestContext _context;
        private readonly Fixture _fixture = new Fixture();
        private readonly Apprentice _apprentice;
        private Revision _apprenticeship;

        public ConfirmationCountdownSteps(TestContext context)
        {
            _context = context;

            _apprentice = _fixture.Create<Apprentice>();
        }

        [Given("we have an existing apprenticeship that was approved on (.*)")]
        public async Task GivenWeHaveAnExistingApprenticeshipThatWasApprovedOn(DateTime approvedOn)
        {
            _fixture.Customizations.Add(
                     new FilteringSpecimenBuilder(
                         new FixedBuilder(approvedOn),
                         new ParameterSpecification(
                             typeof(DateTime),
                             "approvedOn")));

            _apprenticeship = _fixture.Create<Revision>();
            _apprentice.AddApprenticeship(_apprenticeship);

            _context.DbContext.Apprentices.Add(_apprentice);
            await _context.DbContext.SaveChangesAsync();
        }

        [When("we have received a change of circumstances that was approved on (.*)")]
        public async Task GivenWeHaveReceivedAChangeOfCircumstancesThatWasApprovedOn(DateTime approvedOn)
        {
            var change = _fixture.Build<ChangeRegistrationCommand>()
                .With(x => x.CommitmentsApprovedOn, approvedOn)
                .With(x => x.CommitmentsApprenticeshipId, _apprenticeship.CommitmentsApprenticeshipId)
                .Without(x => x.CommitmentsContinuedApprenticeshipId)
                .Create();

            await _context.Api.Put("approvals", change);
        }

        [When("retrieving the apprenticeship")]
        public async Task WhenRetrievingTheApprenticeship()
        {
            await _context.Api.Get($"apprentices/{_apprentice.Id}/apprenticeships/{_apprenticeship.ApprenticeshipId}");
        }

        [Then("the response should contain confirmation dealine (.*)")]
        public void ThenTheResponseShouldContainDaysRemaining(DateTime confirmBefore)
        {
            _context.Api.Response.EnsureSuccessStatusCode();

            var response = JsonConvert
                .DeserializeObject<ApprenticeshipDto>(_context.Api.ResponseContent);

            response.Should().BeEquivalentTo(new
            {
                ConfirmBefore = confirmBefore,
            });
        }
    }
}