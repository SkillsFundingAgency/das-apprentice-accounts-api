using System;

namespace SFA.DAS.ApprenticeAccounts.Data.Models
{
    public class ApprenticePreferences : Entity
    {
        private ApprenticePreferences()
        {
            //for entity framework
        }

        public ApprenticePreferences(Guid apprenticeId, int preferenceId, int enabled, DateTime createdOn, DateTime updatedOn)
        {
            this.ApprenticeId = apprenticeId;
            this.PreferenceId = preferenceId;
            this.Enabled = enabled;
            this.CreatedOn = createdOn;
            this.UpdatedOn = updatedOn;
        }

        public Guid ApprenticeId { get; set; }
        public int PreferenceId { get; set; }
        public int Enabled { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Preference Preference { get; set; }

    }
}
