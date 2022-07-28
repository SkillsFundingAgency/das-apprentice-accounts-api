using AutoFixture;
using FluentAssertions;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeAccounts.Api.AcceptanceTests.Features
{
    [Binding]
    [Scope(Feature = "GetApprenticePreferences")]
    public class GetApprenticePreferencesSteps
    {
        private readonly TestContext _context;
        private readonly Fixture _fixture = new Fixture();
        private readonly ApprenticePreferences _apprenticePreferences;
        private readonly Apprentice _apprentice;

        public GetApprenticePreferencesSteps(TestContext context)
        {
            _context = context;
            _apprenticePreferences = _fixture.Build<ApprenticePreferences>().Create();
            _apprentice = _fixture.Build<Apprentice>().Create();

            var startDate = new System.DateTime(2000, 01, 01);
        }

        [Given("there is one apprentice")]
        public async Task GivenThereIsOneApprentice()
        {
            _context.DbContext.ApprenticePreferences.Add(_apprenticePreferences);
            await _context.DbContext.SaveChangesAsync();
        }

        [Given("there is no apprentice")]
        public void GivenThereIsNoApprentice()
        {
        }

        [When("we try to retrieve the apprentice preferences")]
        public async Task WhenWeTryToRetrieveTheApprenticeship()
        {
            await _context.Api.Get($"apprenticepreferences/{_apprentice.Id}");
        }

        [Then("the result should return ok")]
        public void ThenTheResultShouldReturnOk()
        {
            _context.Api.Response.Should().Be200Ok();
        }

        [Then("the response should match the expected apprentice values")]
        public void ThenTheResponseShouldMatchTheExpectedApprenticeValues()
        {
            _context.Api.Response
                .Should().BeAs(new
                {
                    ApprenticePreferences = _apprenticePreferences.ApprenticeId,
                    _apprenticePreferences.PreferenceId,
                    _apprenticePreferences.Status
                });
        }

        [Then("the result should return Not Found")]
        public void ThenTheResultShouldReturnNotFound()
        {
            _context.Api.Response.Should().Be404NotFound();
        }
    }
}