using MediatR;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences.GetSingleApprenticePreferenceByIds;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.GetSingleApprenticePreferenceValueByIdsQuery
{
    public class GetSingleApprenticePreferenceValueQuery : IRequest<GetSingleApprenticePreferenceDto>
    {
        public Guid ApprenticeId { get; set; }
        public int PreferenceId { get; set; }
    }
}
