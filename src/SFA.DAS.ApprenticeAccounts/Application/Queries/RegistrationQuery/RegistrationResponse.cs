using System;

#nullable disable

namespace SFA.DAS.ApprenticeCommitments.Application.Queries.RegistrationQuery
{
    public class RegistrationResponse
    {
        public Guid RegistrationId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public bool HasViewedVerification { get; set; }
        public bool HasCompletedVerification { get; set; }
    }
}