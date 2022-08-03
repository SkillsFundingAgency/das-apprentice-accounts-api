using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences.GetAllApprenticePreferencesForApprentice
{
    public static class GetAllApprenticePreferencesForApprenticeMapping
    {
        public static GetAllApprenticePreferencesForApprenticeDto MapToApprenticePreferencesDto(
            this IEnumerable<Data.Models.ApprenticePreferences> apprenticePreferences)
        {
            var apprenticePreferencesDto = new GetAllApprenticePreferencesForApprenticeDto
            {
                ApprenticePreferences = new List<ApprenticePreferenceDto>(apprenticePreferences.Select(ap =>
                    new ApprenticePreferenceDto()
                    {
                        PreferenceId = ap.PreferenceId,
                        PreferenceMeaning = ap.Preference.PreferenceMeaning,
                        Status = ap.Status
                    }))
            };

            return apprenticePreferencesDto;
        }
    }
}