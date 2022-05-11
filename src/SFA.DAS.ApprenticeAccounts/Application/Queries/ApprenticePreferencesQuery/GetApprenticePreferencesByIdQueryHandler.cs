using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.ApprenticePreferencesQuery
{
    public class GetApprenticePreferencesByIdQueryHandler : IRequestHandler<GetApprenticePreferencesByIdQuery, ApprenticePreferencesDto>
    {
        private readonly IApprenticePreferencesContext _apprenticePreferences;

        public GetApprenticePreferencesByIdQueryHandler(IApprenticePreferencesContext apprenticePreferences)
        {
            _apprenticePreferences = apprenticePreferences;
        }

        public async Task<ApprenticePreferencesDto> Handle(GetApprenticePreferencesByIdQuery request, CancellationToken cancellationToken)
        {
            var result = _apprenticePreferences.GetApprenticePreferencesByIdAsync(request.ApprenticeId);
            return result.MapToApprenticePreferenceDto();
        }
    }
}
