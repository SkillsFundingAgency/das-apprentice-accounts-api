using SFA.DAS.ApprenticeAccounts.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApprenticeAccounts.DTOs.Preferences.GetAllPreferences
{
    public static class PreferencesDtoMapping
    {
        public static GetAllPreferencesDto MapToPreferenceDto(this IEnumerable<Preference> preferences)
        {
            var preferencesReturned = preferences.ToList();
            var preferencesDto = new GetAllPreferencesDto
            {
                Preferences = new List<PreferenceDto>(
                    preferencesReturned.Select(p => new PreferenceDto
                    {
                        PreferenceId = p.PreferenceId, PreferenceMeaning = p.PreferenceMeaning
                    }))
            };
                return preferencesDto;
        }
    }
}