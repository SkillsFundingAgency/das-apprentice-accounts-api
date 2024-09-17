using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences.GetAllApprenticePreferencesForApprentice;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences.GetApprenticePreferenceForApprenticeAndPreference;
using SFA.DAS.Testing.AutoFixture;
using System;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.DTOs.ApprenticePreferences
{
    public class WhenMappingFromGetSingleApprenticePreferenceValueToGetSingleApprenticePreferenceValueDto
    {
        [Test, MoqAutoData]
        public void AndRecordIsFound_MapToDto(int preferenceId, bool mockStatus, DateTime createdOn, DateTime updatedOn, string mockPreferenceMeaning, string mockPreferenceHint)
        {
            var preference = new Data.Models.Preference(preferenceId, mockPreferenceMeaning, mockPreferenceHint);
            var apprenticePreferences = new Data.Models.ApprenticePreferences(Guid.NewGuid(), preferenceId, mockStatus, createdOn, updatedOn)
            {
                Preference = preference
            };

            var result = new ApprenticePreferenceDto
            {
                PreferenceId = apprenticePreferences.PreferenceId,
                PreferenceMeaning = apprenticePreferences.Preference.PreferenceMeaning,
                Status = apprenticePreferences.Status
            };

            var response =  apprenticePreferences.MapToApprenticePreferenceDto();

            Assert.That(response.PreferenceId, Is.EqualTo(result.PreferenceId));
            Assert.That(response.PreferenceMeaning, Is.EqualTo(result.PreferenceMeaning));
            Assert.That(response.Status, Is.EqualTo(result.Status));
        }
    }
}
