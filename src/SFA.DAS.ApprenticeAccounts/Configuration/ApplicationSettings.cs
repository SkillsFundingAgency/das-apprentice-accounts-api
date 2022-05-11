#nullable disable

using System;

namespace SFA.DAS.ApprenticeAccounts.Configuration
{
    public class ApplicationSettings
    {
        public string DbConnectionString { get; set; }
        public TimeSpan TimeToWaitBeforeChangeOfApprenticeshipEmail { get; set; } = TimeSpan.FromHours(24);
        public int FuzzyMatchingSimilarityThreshold { get; set; }
        public DateTime TermsOfServiceUpdatedOn { get; set; }
    }
}