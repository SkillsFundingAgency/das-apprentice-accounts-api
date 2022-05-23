using MediatR;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences.GetApprenticePreferencesByApprenticeId;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.GetApprenticePreferencesByApprenticeIdQuery
{
    public class GetApprenticePreferencesByIdQuery : IRequest<ApprenticePreferencesDto>
    {
        public Guid ApprenticeId { get; set; }
    }
}
