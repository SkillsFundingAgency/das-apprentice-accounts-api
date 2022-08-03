using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.DTOs.Preferences.GetAllPreferences;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.PreferencesQuery
{
    public class GetAllPreferencesQueryHandler : IRequestHandler<GetAllPreferencesQuery, GetAllPreferencesDto>
    {
        private readonly IPreferencesContext _preferencesContext;

        public GetAllPreferencesQueryHandler(IPreferencesContext preferencesContext) =>
            _preferencesContext = preferencesContext;

        public async Task<GetAllPreferencesDto> Handle(GetAllPreferencesQuery request,
            CancellationToken cancellationToken)
        {
            var response = await _preferencesContext.GetAllPreferences();
            return response.MapToPreferenceDto();
        }
    }
}