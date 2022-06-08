using System.Collections.Generic;

namespace SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences.GetAllApprenticePreferencesForApprentice
{
    public static class GetAllApprenticePreferencesForApprenticeMapping
    {
        public static GetAllApprenticePreferencesForApprenticeDto MapToApprenticePreferencesDto(
            this IEnumerable<Data.Models.ApprenticePreferences> apprenticePreferences)
        {
            var apprenticePreferencesDto = new GetAllApprenticePreferencesForApprenticeDto
            {
                ApprenticePreferences = new List<ApprenticePreferenceDto>()
            };

            foreach (var apprenticePreference in apprenticePreferences)
            {
                apprenticePreferencesDto.ApprenticePreferences.Add(new ApprenticePreferenceDto
                {
                    PreferenceId = apprenticePreference.PreferenceId,
                    PreferenceMeaning = apprenticePreference.Preference.PreferenceMeaning,
                    Status = apprenticePreference.Status,
                    UpdatedOn = apprenticePreference.UpdatedOn
                });
            }

            return apprenticePreferencesDto;
        }
    }
}