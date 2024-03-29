﻿using System;

namespace SFA.DAS.ApprenticeAccounts.Data.Models
{
    public class ApprenticePreferences : Entity
    {
        private ApprenticePreferences()
        {
            //for entity framework
        }

        public ApprenticePreferences(Guid apprenticeId, int preferenceId, bool status, DateTime createdOn, DateTime updatedOn)
        {
            this.ApprenticeId = apprenticeId;
            this.PreferenceId = preferenceId;
            this.Status = status;
            this.CreatedOn = createdOn;
            this.UpdatedOn = updatedOn;
        }

        public Guid ApprenticeId { get; set; }
        public Apprentice Apprentice { get; set; } = null!;
        public int PreferenceId { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Preference Preference { get; set; } = null!;
    }
}
