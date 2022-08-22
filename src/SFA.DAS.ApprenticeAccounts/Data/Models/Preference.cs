using System.Collections.Generic;

namespace SFA.DAS.ApprenticeAccounts.Data.Models
{
    public class Preference : Entity
    {
        private Preference()
        {
            //for entity framework
        }

        public Preference(int preferenceId, string preferenceMeaning, string preferenceHint)
        {
            this.PreferenceId = preferenceId;
            this.PreferenceMeaning = preferenceMeaning;
            this.PreferenceHint = preferenceHint;
        }

        public int PreferenceId { get; set; }
        public string PreferenceMeaning { get; set; } = null!;
        public string PreferenceHint { get; set; } = null!;
        public ICollection<ApprenticePreferences> ApprenticePreferences { get; private set; } = new List<ApprenticePreferences>();
    }
}
