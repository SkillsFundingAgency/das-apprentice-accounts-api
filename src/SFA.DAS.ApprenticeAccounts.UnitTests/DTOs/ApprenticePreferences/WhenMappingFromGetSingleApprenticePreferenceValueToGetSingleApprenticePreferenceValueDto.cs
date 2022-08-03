using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences.GetAllApprenticePreferencesForApprentice;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences.GetApprenticePreferenceForApprenticeAndPreference;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.DTOs.ApprenticePreferences
{
    public class WhenMappingFromGetSingleApprenticePreferenceValueToGetSingleApprenticePreferenceValueDto
    {
        [Test, MoqAutoData]
        public async Task AndRecordIsFound_MapToDto(int preferenceId, bool mockStatus, DateTime createdOn, DateTime updatedOn)
        {
            var preference = new Data.Models.Preference(preferenceId, "meaning");
            var apprenticePreferences = new Data.Models.ApprenticePreferences(Guid.NewGuid(), preferenceId, mockStatus, createdOn, updatedOn);
            apprenticePreferences.Preference = preference;
            
            var result = new ApprenticePreferenceDto()
            {
                PreferenceId = apprenticePreferences.PreferenceId,
                PreferenceMeaning = apprenticePreferences.Preference.PreferenceMeaning,
                Status = apprenticePreferences.Status
            };

            var response =  apprenticePreferences.MapToApprenticePreferenceDto();

            Assert.AreEqual(result.PreferenceId, response.PreferenceId);
            Assert.AreEqual(result.PreferenceMeaning, response.PreferenceMeaning);
            Assert.AreEqual(result.Status, response.Status);
        }
    }
}
