using AutoFixture;
using FluentAssertions;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System.Collections.Generic;
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
        private Preference _preferenceOne;
        private Preference _preferenceTwo;

        public GetPreferencesSteps(TestContext context)
        {
            _context = context;
        }

        [Given(@"there are preferences")]
        public async Task GivenThereArePreferences()
        {
            _preferenceOne = _fixture.Build<Preference>().Create();
            _preferenceTwo = _fixture.Build<Preference>().Create();

            var preferences = new List<Preference>() { _preferenceOne, _preferenceTwo };

            _context.DbContext.Preference.AddRange(preferences);
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
                .Should().BeAs(new[]
                    {
                                    new
                                    {
                                        _preferenceOne.PreferenceId,
                                        _preferenceOne.PreferenceMeaning,
                                        _preferenceOne.PreferenceHint
                                    },
                        new
                        {
                            _preferenceTwo.PreferenceId,
                            _preferenceTwo.PreferenceMeaning,
                            _preferenceTwo.PreferenceHint
                        }

                });
        }
    }
}
