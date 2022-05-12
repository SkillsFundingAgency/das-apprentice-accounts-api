﻿using System;

namespace SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences
{
    public class ApprenticePreferenceDto
    {
        public int PreferenceId { get; set; }
        public string PreferenceMeaning { get; set; }
        public bool Status { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}