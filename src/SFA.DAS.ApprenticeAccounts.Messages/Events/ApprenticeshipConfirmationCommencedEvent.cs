using System;

namespace SFA.DAS.ApprenticeCommitments.Messages.Events
{
    public class ApprenticeshipConfirmationCommencedEvent
    {
        public Guid ApprenticeId { get; set; }
        public long? ApprenticeshipId { get; set; }
        public long? ConfirmationId { get; set; }
        public DateTime ConfirmationOverdueOn { get; set; }
        public long CommitmentsApprenticeshipId { get; set; }
        public DateTime CommitmentsApprovedOn { get; set; }
    }

    public class ApprenticeshipRegisteredEvent
    {
        public Guid RegistrationId { get; set; }
    }
}