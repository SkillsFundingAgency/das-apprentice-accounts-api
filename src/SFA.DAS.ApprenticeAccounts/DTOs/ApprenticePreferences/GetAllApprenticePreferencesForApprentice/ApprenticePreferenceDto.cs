using System;

namespace SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences.GetAllApprenticePreferencesForApprentice
{
    public class ApprenticePreferenceDto
    {
        public int PreferenceId { get; set; }
        public string PreferenceMeaning { get; set; } = null!;
        public bool Status { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}
