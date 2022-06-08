using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences.GetApprenticePreferenceForApprenticeAndPreference;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.GetApprenticePreferenceForApprenticeAndPreferenceQuery
{
    public class GetApprenticePreferenceForApprenticeAndPreferenceQueryHandler : IRequestHandler<
        GetApprenticePreferenceForApprenticeAndPreferenceQuery, GetApprenticePreferenceForApprenticeAndPreferenceDto>
    {
        private readonly IApprenticePreferencesContext _apprenticePreferencesContext;

        public GetApprenticePreferenceForApprenticeAndPreferenceQueryHandler(
            IApprenticePreferencesContext apprenticePreferencesContext) =>
            _apprenticePreferencesContext = apprenticePreferencesContext;

        public async Task<GetApprenticePreferenceForApprenticeAndPreferenceDto> Handle(
            GetApprenticePreferenceForApprenticeAndPreferenceQuery request, CancellationToken cancellationToken)
        {
            var preference =
                await _apprenticePreferencesContext.GetApprenticePreferenceForApprenticeAndPreference(
                    request.ApprenticeId, request.PreferenceId);

            if (preference == null)
            {
                return await Task.FromResult(new GetApprenticePreferenceForApprenticeAndPreferenceDto());
            }

            return preference.MapToApprenticePreferenceDto();
        }
    }
}