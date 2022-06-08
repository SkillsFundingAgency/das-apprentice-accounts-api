using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences.GetAllApprenticePreferencesForApprentice;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.GetAllApprenticePreferencesForApprenticeQuery
{
    public class GetAllApprenticePreferencesForApprenticeQueryHandler : IRequestHandler<GetAllApprenticePreferencesForApprenticeQuery, GetAllApprenticePreferencesForApprenticeDto>
    {
        private readonly IApprenticePreferencesContext _apprenticePreferencesContext;

        public GetAllApprenticePreferencesForApprenticeQueryHandler(IApprenticePreferencesContext apprenticePreferencesContext)
        {
            _apprenticePreferencesContext = apprenticePreferencesContext;
        }

        public async Task<GetAllApprenticePreferencesForApprenticeDto> Handle(GetAllApprenticePreferencesForApprenticeQuery request, CancellationToken cancellationToken)
        {
            var result = await _apprenticePreferencesContext.GetAllApprenticePreferencesForApprentice(request.ApprenticeId);
            return result.MapToApprenticePreferencesDto();
        }
    }
}
