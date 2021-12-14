using System;

#nullable disable

namespace SFA.DAS.ApprenticeAccounts.DTOs
{
    public class ApprenticeDto
    {
        public Guid ApprenticeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid UserIdentityId { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool TermsOfUseAccepted { get; set; }
    }
}
