using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.Preferences;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.DTOs.Preferences
{
    public class WhenMappingFromPreferenceModelToPreferencesDto
    {
        [Test, RecursiveMoqAutoData]
        public async Task ThenTheFieldsAreCorrectlyMapped() 
        {
            var response = new List<PreferenceDto>();

            var preferences = new List<Preference>(2) { new Preference(1, "Test Meaning"), new Preference(2, "Test Meaning Two")}; //add some more prefs

            foreach (Preference p in preferences)
            {
                var preferenceDtoOption = new PreferenceDto() 
                { PreferenceId = p.PreferenceId,
                  PreferenceMeaning = p.PreferenceMeaning};
                response.Add(preferenceDtoOption);
            }

            var result = preferences.MapToPreferenceDto();

            result.preferencesDto.Should().BeEquivalentTo(response);
        }
    }
}
