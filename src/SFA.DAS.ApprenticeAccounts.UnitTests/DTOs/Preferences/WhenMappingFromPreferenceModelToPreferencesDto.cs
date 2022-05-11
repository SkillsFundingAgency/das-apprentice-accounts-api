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
        public async Task ThenTheFieldsAreCorrectlyMapped(
            List<Preference> preferences) 
        {
            var response = new List<PreferenceDto>();

            foreach (Preference p in preferences)
            {
                var preferenceDtoOption = new PreferenceDto() { preferenceId = p.preferenceId , preferenceMeaning = p.preferenceMeaning};
                response.Add(preferenceDtoOption);
            }

            var result = preferences.MapToPreferenceDto();

            result.preferencesDto.Should().BeEquivalentTo(response);
        }
    }
}
