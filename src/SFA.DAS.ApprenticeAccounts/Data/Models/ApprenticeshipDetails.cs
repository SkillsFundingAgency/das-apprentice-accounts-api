#nullable enable

using System;

namespace SFA.DAS.ApprenticeCommitments.Data.Models
{
    public sealed class ApprenticeshipDetails : IEquatable<ApprenticeshipDetails>
    {
        private ApprenticeshipDetails()
        {
            // Private constructor for entity framework
        }

        public ApprenticeshipDetails(
            long employerAccountLegalEntityId, string employerName,
            long trainingProviderId, string trainingProviderName,
            CourseDetails course)
        {
            EmployerAccountLegalEntityId = employerAccountLegalEntityId;
            EmployerName = employerName;
            TrainingProviderId = trainingProviderId;
            TrainingProviderName = trainingProviderName;
            Course = course;
        }

        public long EmployerAccountLegalEntityId { get; private set; }
        public string EmployerName { get; private set; } = null!;

        public long TrainingProviderId { get; private set; }
        public string TrainingProviderName { get; private set; } = null!;

        public CourseDetails Course { get; private set; } = null!;

        public bool EmployerIsEquivalent(ApprenticeshipDetails? other)
            => other != null &&
            EmployerAccountLegalEntityId == other.EmployerAccountLegalEntityId &&
            EmployerName == other.EmployerName;

        internal bool ProviderIsEquivalent(ApprenticeshipDetails other)
            => other != null &&
            TrainingProviderId == other.TrainingProviderId &&
            TrainingProviderName == other.TrainingProviderName;

        internal bool ApprenticeshipIsEquivalent(ApprenticeshipDetails other)
            => other != null &&
            Course.IsEquivalent(other.Course);

        public override bool Equals(object? obj) => obj switch
        {
            ApprenticeshipDetails other => Equals(other),
            _ => false,
        };

        public bool Equals(ApprenticeshipDetails? other) =>
            other != null &&
            other.EmployerAccountLegalEntityId == EmployerAccountLegalEntityId &&
            other.EmployerName == EmployerName &&
            other.TrainingProviderId == TrainingProviderId &&
            other.TrainingProviderName == TrainingProviderName &&
            Course.IsEquivalent(other.Course);

        public override int GetHashCode() =>
            System.HashCode.Combine(
                EmployerAccountLegalEntityId,
                EmployerName,
                TrainingProviderId,
                TrainingProviderName,
                Course);
    }
}