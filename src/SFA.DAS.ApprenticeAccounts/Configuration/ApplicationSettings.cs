#nullable disable

using System;

namespace SFA.DAS.ApprenticeCommitments.Configuration
{
    public class ApplicationSettings
    {
        public string DbConnectionString { get; set; }
        public TimeSpan TimeToWaitBeforeChangeOfApprenticeshipEmail { get; set; } = TimeSpan.FromHours(24);
        public int FuzzyMatchingSimilarityThreshold { get; set; }
    }
}