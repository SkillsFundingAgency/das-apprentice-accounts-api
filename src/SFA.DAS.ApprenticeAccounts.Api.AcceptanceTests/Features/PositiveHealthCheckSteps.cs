using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Io.Cucumber.Messages;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeAccounts.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "PositiveHealthCheck")]
    public class PositiveHealthCheckSteps
    {
        private readonly TestContext _context;

        public PositiveHealthCheckSteps(TestContext context)
        {
            _context = context;
        }

        [Given(@"the api has started")]
        public void GivenTheApiHasStarted()
        {
        }
        
        [Given(@"the database is online")]
        public void GivenTheDatabaseIsOnline()
        {
        }
        
        [When(@"the ping endpoint is called")]
        public async Task WhenThePingEndpointIsCalled()
        {
            await _context.Api.Get("ping");
        }
        
        [When(@"the health endpoint is called")]
        public async Task WhenTheHealthEndpointIsCalled()
        {
            await _context.Api.Get("health");
        }

        [Then(@"the result should be ok")]
        public void ThenTheResultShouldBeOk()
        {
            _context.Api.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
   
        [Then(@"the result should be healthy")]
        public async Task ThenTheResultShouldBeHealthy()
        {
            var response = await _context.Api.Response.Content.ReadAsStringAsync();
            response.Should().Contain("Healthy");
        }
    }
}
