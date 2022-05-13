using MediatR;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.ApprenticePreferencesQuery
{
    public class GetApprenticePreferencesByIdQuery : IRequest<ApprenticePreferencesDto>
    {
        public Guid ApprenticeId { get; set; }
    }
}
