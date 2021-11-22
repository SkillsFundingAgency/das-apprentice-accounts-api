using SFA.DAS.ApprenticeCommitments.Data.Models;
using System.Diagnostics.CodeAnalysis;

#nullable enable

namespace SFA.DAS.ApprenticeCommitments.DTOs
{
    public static class ApprenticeDtoMapping
    {
        [return: NotNullIfNotNull("apprenticeship")]
        public static ApprenticeDto? MapToApprenticeDto(this Apprentice? apprentice)
        {
            if (apprentice == null) return null;

            return new ApprenticeDto
            {
                Id = apprentice.Id,
                FirstName = apprentice.FirstName,
                LastName = apprentice.LastName,
                Email = apprentice.Email.ToString(),
                DateOfBirth = apprentice.DateOfBirth,
                TermsOfUseAccepted = apprentice.TermsOfUseAccepted
            };
        }
    }
}