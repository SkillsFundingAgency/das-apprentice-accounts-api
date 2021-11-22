﻿using System;

namespace SFA.DAS.ApprenticeCommitments.DTOs
{
    public class ApprenticeshipRevisionDto
    {
        public long RevisionId { get; set; }
        public long CommitmentsApprenticeshipId { get; set; }
        public DateTime ApprovedOn { get; set; }

        public string EmployerName { get; set; } = null!;
        public long EmployerAccountLegalEntityId { get; set; }
        public long TrainingProviderId { get; set; }
        public string TrainingProviderName { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public int CourseLevel { get; set; }
        public string? CourseOption { get; set; } = null!;
        public int CourseDuration { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }

        public bool? TrainingProviderCorrect { get; set; }
        public bool? EmployerCorrect { get; set; }
        public bool? RolesAndResponsibilitiesCorrect { get; set; }
        public bool? ApprenticeshipDetailsCorrect { get; set; }
        public bool? HowApprenticeshipDeliveredCorrect { get; set; }
        public DateTime ConfirmBefore { get; set; }
        public DateTime? ConfirmedOn { get; set; }

        public DateTime? LastViewed { get; set; }
    }
}