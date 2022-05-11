using SFA.DAS.ApprenticeAccounts.Data.Models;
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
        public bool ReacceptTermsOfUseRequired { get; set; }
        public bool IsPrivateBetaUser { get; set; }

        public static ApprenticeDto Create(Apprentice source, DateTime termsOfServiceUpdatedOn)
        {
            if (source == null)
                return null;

            var apprenticeDto = new ApprenticeDto
            {
                ApprenticeId = source.Id,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email.ToString(),
                DateOfBirth = source.DateOfBirth,
                IsPrivateBetaUser = source.IsPrivateBetaUser,
                TermsOfUseAccepted = source.TermsOfUseAccepted,
                ReacceptTermsOfUseRequired = source.TermsOfUseNeedsReaccepting(termsOfServiceUpdatedOn)
            };

            return apprenticeDto;
        }
    }
}
