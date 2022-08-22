using System;

namespace SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences.GetApprenticePreferenceForApprenticeAndPreference
{
    public class GetApprenticePreferenceForApprenticeAndPreferenceDto
    {
        public int PreferenceId { get; set; }
        public string PreferenceMeaning { get; set; } = null!;
        public bool Status { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
