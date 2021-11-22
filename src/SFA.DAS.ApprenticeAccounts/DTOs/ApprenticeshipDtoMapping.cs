using SFA.DAS.ApprenticeCommitments.Data.Models;
using System.Diagnostics.CodeAnalysis;

#nullable enable

namespace SFA.DAS.ApprenticeCommitments.DTOs
{
    public static class ApprenticeshipDtoMapping
    {
        [return: NotNullIfNotNull("apprenticeship")]
        public static ApprenticeshipDto? MapToApprenticeshipDto(this Apprenticeship? apprenticeship)
        {
            if (apprenticeship == null) return null;

            var latest = apprenticeship.LatestRevision;

            return new ApprenticeshipDto
            {
                Id = apprenticeship.Id,
                ApprenticeId = apprenticeship.ApprenticeId,
                RevisionId = latest.Id,
                LastViewed = latest.LastViewed,
                CommitmentsApprenticeshipId = latest.CommitmentsApprenticeshipId,
                EmployerName = latest.Details.EmployerName,
                EmployerAccountLegalEntityId = latest.Details.EmployerAccountLegalEntityId,
                TrainingProviderId = latest.Details.TrainingProviderId,
                TrainingProviderName = latest.Details.TrainingProviderName,
                TrainingProviderCorrect = latest.TrainingProviderCorrect,
                ApprenticeshipDetailsCorrect = latest.ApprenticeshipDetailsCorrect,
                HowApprenticeshipDeliveredCorrect = latest.HowApprenticeshipDeliveredCorrect,
                EmployerCorrect = latest.EmployerCorrect,
                RolesAndResponsibilitiesCorrect = latest.RolesAndResponsibilitiesCorrect,
                CourseName = latest.Details.Course.Name,
                CourseLevel = latest.Details.Course.Level,
                CourseOption = latest.Details.Course.Option,
                CourseDuration = latest.Details.Course.CourseDuration,
                PlannedStartDate = latest.Details.Course.PlannedStartDate,
                PlannedEndDate = latest.Details.Course.PlannedEndDate,
                ConfirmedOn = latest.ConfirmedOn,
                ConfirmBefore = latest.ConfirmBefore,
                ApprovedOn = latest.CommitmentsApprovedOn,
                ChangeOfCircumstanceNotifications = apprenticeship.ChangeOfCircumstanceNotifications,
                StoppedReceivedOn = latest.StoppedReceivedOn,
            };
        }
    }
}