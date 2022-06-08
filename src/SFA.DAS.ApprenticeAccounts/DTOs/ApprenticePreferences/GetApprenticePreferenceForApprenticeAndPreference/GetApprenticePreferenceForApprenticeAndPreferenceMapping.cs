namespace SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences.GetApprenticePreferenceForApprenticeAndPreference
{
    public static class GetApprenticePreferenceForApprenticeAndPreferenceMapping
    {
        public static GetApprenticePreferenceForApprenticeAndPreferenceDto
            MapToApprenticePreferenceDto(
                this Data.Models.ApprenticePreferences apprenticePreferences)
        {
            var apprenticePreferenceDto = new GetApprenticePreferenceForApprenticeAndPreferenceDto
            {
                PreferenceId = apprenticePreferences.PreferenceId,
                PreferenceMeaning = apprenticePreferences.Preference.PreferenceMeaning,
                Status = apprenticePreferences.Status,
                UpdatedOn = apprenticePreferences.UpdatedOn
            };
            return apprenticePreferenceDto;
        }
    }
}