using SFA.DAS.ApprenticeAccounts.Data.Models;
using System;
using System.Diagnostics.CodeAnalysis;

#nullable enable

namespace SFA.DAS.ApprenticeAccounts.DTOs
{
    public class ApprenticeDto
    {
        public Guid ApprenticeId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public Guid UserIdentityId { get; set; }
        public string Email { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public bool TermsOfUseAccepted { get; set; }
        public bool ReacceptTermsOfUseRequired { get; set; }

        [return: NotNullIfNotNull("source")]
        public static ApprenticeDto? Create(Apprentice source, DateTime termsOfServiceUpdatedOn)
        {
            if (source == null) return null;

            var termsOfUseNeedsReaccepting = source.TermsOfUseNeedsReaccepting(termsOfServiceUpdatedOn);

            var apprenticeDto = new ApprenticeDto
            {
                ApprenticeId = source.Id,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email.ToString(),
                DateOfBirth = source.DateOfBirth,
                TermsOfUseAccepted = source.TermsOfUseAccepted && !termsOfUseNeedsReaccepting,
                ReacceptTermsOfUseRequired = termsOfUseNeedsReaccepting
            };

            return apprenticeDto;
        }
    }
}
