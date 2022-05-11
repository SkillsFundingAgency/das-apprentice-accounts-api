using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.DTOs.Preferences;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.PreferencesQuery
{
    public class GetAllPreferencesQueryHandler : IRequestHandler<GetAllPreferencesQuery, PreferencesDto>
    {
        private readonly IPreferencesContext _preferencesContext;

        public GetAllPreferencesQueryHandler(IPreferencesContext preferencesContext)
        {
            _preferencesContext = preferencesContext;
        }
        public async Task<PreferencesDto> Handle(GetAllPreferencesQuery request, CancellationToken cancellationToken)
        {
            var response = _preferencesContext.GetAllPreferencesAsync();
            return response.MapToPreferenceDto();
        }
    }
}
