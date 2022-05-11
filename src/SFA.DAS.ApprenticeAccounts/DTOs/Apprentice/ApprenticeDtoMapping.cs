using SFA.DAS.ApprenticeAccounts.Data.Models;
using System.Diagnostics.CodeAnalysis;

#nullable enable

namespace SFA.DAS.ApprenticeAccounts.DTOs.Apprentice
{
    public static class ApprenticeDtoMapping
    {
        [return: NotNullIfNotNull("apprenticeship")]
        public static ApprenticeDto? MapToApprenticeDto(this Data.Models.Apprentice? apprentice)
        {
            if (apprentice == null) return null;

            return new ApprenticeDto
            {
                ApprenticeId = apprentice.Id,
                FirstName = apprentice.FirstName,
                LastName = apprentice.LastName,
                Email = apprentice.Email.ToString(),
                DateOfBirth = apprentice.DateOfBirth,
                TermsOfUseAccepted = apprentice.TermsOfUseAccepted
            };
        }
    }
}