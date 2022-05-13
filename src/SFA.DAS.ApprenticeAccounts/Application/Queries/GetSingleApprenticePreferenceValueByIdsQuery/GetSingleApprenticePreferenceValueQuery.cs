using MediatR;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.ApprenticePreferencesQuery
{
    public class GetSingleApprenticePreferenceValueQuery : IRequest<GetSingleApprenticePreferenceDto>
    {
        public Guid ApprenticeId { get; set; }
        public int PreferenceId { get; set; }
    }
}
