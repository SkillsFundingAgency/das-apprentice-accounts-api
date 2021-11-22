using System;

#nullable disable

namespace SFA.DAS.ApprenticeCommitments.DTOs
{
    public class RegistrationDto
    {
        public Guid RegistrationId { get; set; }
        public long ApprenticeshipId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string EmployerName { get; set; }
        public long EmployerAccountLegalEntityId { get; set; }
        public long TrainingProviderId { get; set; }
        public string TrainingProviderName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? UserIdentityId { get; set; }
        public string CourseName { get; set; }
    }
}