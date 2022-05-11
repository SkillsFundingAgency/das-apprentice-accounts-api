using System.Collections.Generic;

namespace SFA.DAS.ApprenticeAccounts.Data.Models
{
    public class Preference : Entity
    {
        private Preference()
        {
            //for entity framework
        }

        public Preference(int preferenceId, string preferenceMeaning)
        {
            this.PreferenceId = preferenceId;
            this.PreferenceMeaning = preferenceMeaning;
        }

        public int PreferenceId { get; set; }
        public string PreferenceMeaning { get; set; }
        public ICollection<ApprenticePreferences> ApprenticePreferences { get; set; }
    }
}
