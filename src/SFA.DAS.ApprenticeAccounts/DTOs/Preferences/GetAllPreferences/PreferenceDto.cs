namespace SFA.DAS.ApprenticeAccounts.DTOs.Preferences.GetAllPreferences
{
    public class PreferenceDto
    {
        public int PreferenceId { get; set; }
        public string PreferenceMeaning { get; set; } = null!;
        public string PreferenceHint { get; set; } = null!;
    }
}