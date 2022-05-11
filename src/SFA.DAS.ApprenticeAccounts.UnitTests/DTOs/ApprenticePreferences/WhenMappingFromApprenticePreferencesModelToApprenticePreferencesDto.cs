using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.DTOs.ApprenticePreferences
{
    public class WhenMappingFromApprenticePreferencesModelToApprenticePreferencesDto
    {
        [Test, RecursiveMoqAutoData]
        public async Task ThenTheFieldsAreCorrectlyMapped(List<Data.Models.ApprenticePreferences> apprenticePreferences)
        {
            var response = new List<ApprenticePreferenceDto>();

            foreach (Data.Models.ApprenticePreferences a in apprenticePreferences)
            {
                var apprenticePreferenceDtoOption = new ApprenticePreferenceDto() 
                { PreferenceId = a.PreferenceId,
                  PreferenceMeaning = a.Preference.PreferenceMeaning,
                  Enabled = a.Enabled,
                  UpdatedOn = a.UpdatedOn};
                response.Add(apprenticePreferenceDtoOption);
            }

            var result = apprenticePreferences.MapToApprenticePreferenceDto();

            result.ApprenticePreferences.Should().BeEquivalentTo(response);
        }
    }
}
