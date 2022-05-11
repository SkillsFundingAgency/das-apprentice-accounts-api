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
                preferencesDto = new List<PreferenceDto>()
            };

            foreach(Preference p in preferences)
            {
                dto.preferencesDto.Add(new PreferenceDto() { preferenceId = p.preferenceId, preferenceMeaning = p.preferenceMeaning});
            }

            return dto;
        }
    }
}
