using AutoFixture;
using FluentAssertions;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeAccounts.Api.AcceptanceTests.Features
{
    [Binding]
    [Scope(Feature = "GetPreferences")]
    public class GetPreferencesSteps
    {
        private readonly TestContext _context;
        private readonly Fixture _fixture = new Fixture();
        private Preference _preference;

        public GetPreferencesSteps(TestContext context)
        {
            _context = context;
            var startDate = new DateTime(2000, 01, 01);
        }

        [Given(@"there is one apprentice")]
        public async Task GivenThereIsOneApprentice()
        {
            _preference = _fixture.Build<Preference>().Create();
            _context.DbContext.Preference.Add(_preference);
            await _context.DbContext.SaveChangesAsync();
        }

        [When(@"we try to retrieve the preferences")]
        public async Task WhenWeTryToRetrieveThePreferences() => await _context.Api.Get("/preferences");

        [Then(@"the result should return ok")]
        public void ThenTheResultShouldReturnOk() => _context.Api.Response.Should().Be200Ok();

        [Then(@"the response should match the expected preference values")]
        public void ThenTheResponseShouldMatchTheExpectedPreferenceValues()
        {
            _context.Api.Response
                .Should().BeAs(new
                {
                    Preference = new[] { new { _preference.PreferenceId, _preference.PreferenceMeaning } }
                });
        }
    }
}
