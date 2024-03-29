﻿using AutoFixture;
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
        private ApprenticePreferences _apprenticePreferences;
        private Apprentice _apprentice;
        private Preference _preference;

        public GetApprenticePreferencesSteps(TestContext context)
        {
            _context = context;
        }

        [Given(@"there is an apprentice with preferences")]
        public async Task GivenThereIsAnApprenticeWithPreferences()
        {
            _preference = _fixture.Build<Preference>().Create();
            _apprentice = _fixture.Build<Apprentice>().Without(x => x.TermsOfUseAccepted).Create();
            _apprenticePreferences = _fixture.Build<ApprenticePreferences>().With(x => x.Apprentice, _apprentice)
                .With(x => x.Preference, _preference).Create();
            _context.DbContext.ApprenticePreferences.Add(_apprenticePreferences);
            await _context.DbContext.SaveChangesAsync();
        }

        [When(@"we try to retrieve the apprentice preferences")]
        public async Task WhenWeTryToRetrieveTheApprenticePreferences() =>
            await _context.Api.Get($"apprenticepreferences/{_apprenticePreferences.ApprenticeId}");

        [When(@"we try to retrieve the apprentice preference")]
        public async Task WhenWeTryToRetrieveTheApprenticePreference() =>
    await _context.Api.Get($"apprenticepreferences/{_apprenticePreferences.ApprenticeId}/{_apprenticePreferences.PreferenceId}");

        [Then(@"the result should return ok")]
        public void ThenTheResultShouldReturnOk() => _context.Api.Response.Should().Be200Ok();

        [Then(@"the response should match the expected apprentice preferences values")]
        public void ThenTheResponseShouldMatchTheExpectedApprenticePreferencesValues() =>
            _context.Api.Response
                .Should().BeAs(new
                {
                    ApprenticePreferences = new[]
                    {
                        new
                        {
                            _apprenticePreferences.PreferenceId,
                            _apprenticePreferences.Preference.PreferenceMeaning,
                            _apprenticePreferences.Status
                        }
                    }
                });


        [Then(@"the response should match the expected apprentice preference value")]
        public void ThenTheResponseShouldMatchTheExpectedApprenticePreferenceValue() =>
            _context.Api.Response
                .Should().BeAs(new
                {
                    _apprenticePreferences.PreferenceId,
                    _apprenticePreferences.Preference.PreferenceMeaning,
                    _apprenticePreferences.Status
                });
    }
}