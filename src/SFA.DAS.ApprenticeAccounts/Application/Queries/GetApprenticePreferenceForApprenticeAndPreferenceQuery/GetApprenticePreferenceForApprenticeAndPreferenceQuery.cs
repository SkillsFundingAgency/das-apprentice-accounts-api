using MediatR;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences.GetApprenticePreferenceForApprenticeAndPreference;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.GetApprenticePreferenceForApprenticeAndPreferenceQuery
{
    public class GetApprenticePreferenceForApprenticeAndPreferenceQuery : IRequest<GetApprenticePreferenceForApprenticeAndPreferenceDto>
    {
        public Guid ApprenticeId { get; set; }
        public int PreferenceId { get; set; }
    }
}
