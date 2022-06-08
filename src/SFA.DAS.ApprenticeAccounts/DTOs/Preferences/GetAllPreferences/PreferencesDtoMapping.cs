using SFA.DAS.ApprenticeAccounts.Data.Models;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeAccounts.DTOs.Preferences.GetAllPreferences
{
    public static class PreferencesDtoMapping
    {
        public static GetAllPreferencesDto MapToPreferenceDto(this IEnumerable<Preference> preferences)
        {
            var preferencesDto = new GetAllPreferencesDto { Preferences = new List<PreferenceDto>() };

            foreach (var preference in preferences)
            {
                preferencesDto.Preferences.Add(new PreferenceDto
                {
                    PreferenceId = preference.PreferenceId, PreferenceMeaning = preference.PreferenceMeaning
                });
            }

            return preferencesDto;
        }
    }
}