using System;

namespace SFA.DAS.ApprenticeAccounts.Data.Models
{
    public class ApprenticePreferences : Entity
    {
        private ApprenticePreferences()
        {
            //for entity framework
        }

        public ApprenticePreferences(Guid apprenticeId, int preferenceId, int status, DateTime createdOn, DateTime updatedOn)
        {
            ApprenticeId = apprenticeId;
            PreferenceId = preferenceId;
            Status = status;
            CreatedOn = createdOn;
            UpdatedOn = updatedOn;
        }

        public Guid ApprenticeId { get; set; }
        public Apprentice Apprentice { get; set; }
        public int PreferenceId { get; set; }
        public int Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Preference Preference { get; set; }
    }
}
