using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.ApprenticePreferencesQuery
{
    public class GetSingleApprenticePreferenceValueQueryHandler : IRequestHandler<GetSingleApprenticePreferenceValueQuery, GetSingleApprenticePreferenceDto>
    {
        private readonly IApprenticePreferencesContext _apprenticePreferencesContext;

        public GetSingleApprenticePreferenceValueQueryHandler(IApprenticePreferencesContext apprenticePreferencesContext)
        {
            _apprenticePreferencesContext = apprenticePreferencesContext;
        }

        public Task<GetSingleApprenticePreferenceDto> Handle(GetSingleApprenticePreferenceValueQuery request, CancellationToken cancellationToken)
        {
            var preference =  _apprenticePreferencesContext.GetSinglePreferenceValueAsync(request.ApprenticeId, request.PreferenceId);
           
            if (preference == null)
            {
                return Task.FromResult(new GetSingleApprenticePreferenceDto());
            }

            return preference.ApprenticePreferenceToSinglePreferenceValueDtoMapping();
        }
    }
}
