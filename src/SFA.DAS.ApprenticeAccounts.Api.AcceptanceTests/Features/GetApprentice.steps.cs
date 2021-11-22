using AutoFixture;
using FluentAssertions;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeAccounts.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "GetApprentice")]
    public class GetApprenticeSteps
    {
        private readonly TestContext _context;
        private readonly Fixture _fixture = new Fixture();
        private readonly Apprentice _apprentice;

        public GetApprenticeSteps(TestContext context)
        {
            _context = context;
            _apprentice = _fixture.Build<Apprentice>().Create();

            var startDate = new System.DateTime(2000, 01, 01);
        }

        [Given("there is one apprentice")]
        public async Task GiveThereIsOneApprentice()
        {
            _context.DbContext.Apprentices.Add(_apprentice);
            await _context.DbContext.SaveChangesAsync();
        }

        [Given("there is no apprentice")]
        public void GivenThereIsNoApprentice()
        {
        }

        [When("we try to retrieve the apprentice")]
        public async Task WhenWeTryToRetrieveTheApprenticeship()
        {
            await _context.Api.Get($"apprentices/{_apprentice.Id}");
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
                    _apprentice.Id,
                    _apprentice.FirstName,
                    _apprentice.LastName,
                });
        }

        [Then("the result should return Not Found")]
        public void ThenTheResultShouldReturnNotFound()
        {
            _context.Api.Response.Should().Be404NotFound();
        }
    }
}