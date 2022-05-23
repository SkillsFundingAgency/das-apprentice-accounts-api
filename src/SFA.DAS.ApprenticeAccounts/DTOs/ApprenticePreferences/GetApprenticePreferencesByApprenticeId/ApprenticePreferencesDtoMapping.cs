﻿using System.Collections.Generic;

namespace SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences.GetApprenticePreferencesByApprenticeId
{
    public static class ApprenticePreferencesDtoMapping
    {
        public static ApprenticePreferencesDto MapToApprenticePreferenceDto(this List<Data.Models.ApprenticePreferences> apprenticePreferences)
        {
            var dto = new ApprenticePreferencesDto()
            {
                ApprenticePreferences = new List<ApprenticePreferenceDto>()
            };

            foreach (Data.Models.ApprenticePreferences apprenticePreference in apprenticePreferences)
            {
                dto.ApprenticePreferences.Add(new ApprenticePreferenceDto() 
                { PreferenceId = apprenticePreference.PreferenceId, 
                  PreferenceMeaning = apprenticePreference.Preference.PreferenceMeaning, 
                  Status = apprenticePreference.Status, 
                  UpdatedOn = apprenticePreference.UpdatedOn });
            }
            return dto;
        }
    }
}
