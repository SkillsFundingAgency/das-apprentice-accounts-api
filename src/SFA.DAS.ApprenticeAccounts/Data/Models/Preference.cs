using System.Collections;
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
            this.preferenceId = preferenceId;
            this.preferenceMeaning = preferenceMeaning;
        }

        public int preferenceId { get; set; }
        public string preferenceMeaning { get; set; }
        public ICollection<ApprenticePreferences> apprenticePreferences { get; set; }
    }
}
