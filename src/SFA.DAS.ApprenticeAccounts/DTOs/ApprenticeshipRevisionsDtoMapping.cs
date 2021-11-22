using SFA.DAS.ApprenticeCommitments.Data.Models;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

#nullable enable

namespace SFA.DAS.ApprenticeCommitments.DTOs
{
    public static class ApprenticeshipRevisionsDtoMapping
    {
        [return: NotNullIfNotNull("apprenticeship")]
        public static ApprenticeshipRevisionsDto? MapToApprenticeshipRevisionsDto(this Apprenticeship? apprenticeship)
        {
            if (apprenticeship == null) return null;

            return new ApprenticeshipRevisionsDto
            {
                ApprenticeId = apprenticeship.ApprenticeId,
                ApprenticeshipId = apprenticeship.Id,
                Revisions = apprenticeship.Revisions.Select(MapToApprenticeshipRevisionDto).ToList(),
            };
        }

        public static ApprenticeshipRevisionDto MapToApprenticeshipRevisionDto(this Revision revision)
        {
            return new ApprenticeshipRevisionDto
            {
                RevisionId = revision.Id,
                CommitmentsApprenticeshipId = revision.CommitmentsApprenticeshipId,
                ApprovedOn = revision.CommitmentsApprovedOn,
                EmployerName = revision.Details.EmployerName,
                EmployerAccountLegalEntityId = revision.Details.EmployerAccountLegalEntityId,
                TrainingProviderId = revision.Details.TrainingProviderId,
                TrainingProviderName = revision.Details.TrainingProviderName,
                CourseName = revision.Details.Course.Name,
                CourseLevel = revision.Details.Course.Level,
                CourseOption = revision.Details.Course.Option,
                CourseDuration = revision.Details.Course.CourseDuration,
                PlannedStartDate = revision.Details.Course.PlannedStartDate,
                PlannedEndDate = revision.Details.Course.PlannedEndDate,
                TrainingProviderCorrect = revision.TrainingProviderCorrect,
                EmployerCorrect = revision.EmployerCorrect,
                ApprenticeshipDetailsCorrect = revision.ApprenticeshipDetailsCorrect,
                HowApprenticeshipDeliveredCorrect = revision.HowApprenticeshipDeliveredCorrect,
                RolesAndResponsibilitiesCorrect = revision.RolesAndResponsibilitiesCorrect,
                ConfirmBefore = revision.ConfirmBefore,
                ConfirmedOn = revision.ConfirmedOn,
                LastViewed = revision.LastViewed
            };
        }
    }
}