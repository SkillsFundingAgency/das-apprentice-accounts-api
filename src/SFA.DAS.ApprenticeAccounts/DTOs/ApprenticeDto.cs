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
        public DateTime TermsOfUseAcceptedOn { get; set; }
        public bool TermsOfUsePreviouslyAccepted { get; set; } = false;
        public bool IsPrivateBetaUser { get; set; }

        public ApprenticeDto Create(Apprentice source, DateTime termsOfServiceUpdatedOn)
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
                IsPrivateBetaUser = source.IsPrivateBetaUser
            };

            if (source.IsPrivateBetaUser &&
                source.TermsOfUseAccepted &&
                source.TermsOfUseAcceptedOn <= termsOfServiceUpdatedOn)
            {
                apprenticeDto.TermsOfUseAccepted = false;
                apprenticeDto.TermsOfUseAcceptedOn = DateTime.MinValue;
                apprenticeDto.TermsOfUsePreviouslyAccepted = true;
            }
            else
            {
                apprenticeDto.TermsOfUseAccepted = source.TermsOfUseAccepted;
                apprenticeDto.TermsOfUseAcceptedOn = source.TermsOfUseAcceptedOn ?? DateTime.MinValue;
            }

            return apprenticeDto;
        }
    }
}
