using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences.GetSingleApprenticePreferenceByIds
{
    public static class GetSinglePreferenceValueMapping
    {
        public static async Task<GetSingleApprenticePreferenceDto> ApprenticePreferenceToSinglePreferenceValueDtoMapping(this Data.Models.ApprenticePreferences apprenticePreferences)
        {
            GetSingleApprenticePreferenceDto dto = new GetSingleApprenticePreferenceDto()
            {
                PreferenceId = apprenticePreferences.PreferenceId,
                PreferenceMeaning = apprenticePreferences.Preference.PreferenceMeaning,
                Status = apprenticePreferences.Status,
                UpdatedOn = apprenticePreferences.UpdatedOn
            };
            return dto;
        }
    }
}
