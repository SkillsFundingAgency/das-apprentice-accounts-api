using SFA.DAS.ApprenticeAccounts.Data.Models;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeAccounts.DTOs.Preferences
{
    public static class PreferencesDtoMapping
    {
        public static PreferencesDto MapToPreferenceDto(this List<Preference> preferences)
        {
            var dto = new PreferencesDto()
            {
                Preferences = new List<PreferenceDto>()
            };

            foreach(Preference p in preferences)
            {
                dto.Preferences.Add(new PreferenceDto() { PreferenceId = p.PreferenceId, PreferenceMeaning = p.PreferenceMeaning});
            }

            return dto;
        }
    }
}
