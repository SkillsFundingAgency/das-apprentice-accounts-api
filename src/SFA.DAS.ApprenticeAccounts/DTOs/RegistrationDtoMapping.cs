using SFA.DAS.ApprenticeCommitments.Data.Models;

namespace SFA.DAS.ApprenticeCommitments.DTOs
{
    public static class RegistrationDtoMapping
    {
        public static RegistrationDto MapToRegistrationDto(this Registration registration)
        {
            return new RegistrationDto
            {
                RegistrationId = registration.RegistrationId,
                CreatedOn = registration.CreatedOn,
                ApprenticeshipId = registration.CommitmentsApprenticeshipId,
                FirstName = registration.FirstName,
                LastName = registration.LastName,
                DateOfBirth = registration.DateOfBirth,
                Email = registration.Email.ToString(),
                EmployerName = registration.Apprenticeship.EmployerName,
                EmployerAccountLegalEntityId = registration.Apprenticeship.EmployerAccountLegalEntityId,
                UserIdentityId = registration.ApprenticeId,
                TrainingProviderId = registration.Apprenticeship.TrainingProviderId,
                TrainingProviderName = registration.Apprenticeship.TrainingProviderName,
                CourseName = registration.Apprenticeship.Course.Name,
            };
        }
    }
}