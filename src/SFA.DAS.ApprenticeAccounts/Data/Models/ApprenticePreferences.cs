using System;

namespace SFA.DAS.ApprenticeAccounts.Data.Models
{
    public class ApprenticePreferences : Entity
    {
        private ApprenticePreferences()
        {
            //for entity framework
        }

        public ApprenticePreferences(Guid ApprenticeId, int PreferenceId, bool Status, DateTime CreatedOn, DateTime UpdatedOn)
        {
            this.ApprenticeId = ApprenticeId;
            this.PreferenceId = PreferenceId;
            this.Status = Status;
            this.CreatedOn = CreatedOn;
            this.UpdatedOn = UpdatedOn;
        }

        public Guid ApprenticeId { get; set; }
        public Apprentice Apprentice { get; set; }
        public int PreferenceId { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Preference Preference { get; set; }
    }
}
