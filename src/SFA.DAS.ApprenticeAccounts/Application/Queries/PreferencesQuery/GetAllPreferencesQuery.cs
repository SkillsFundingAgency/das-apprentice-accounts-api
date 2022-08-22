using MediatR;
using SFA.DAS.ApprenticeAccounts.DTOs.Preferences.GetAllPreferences;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.PreferencesQuery
{
    public class GetAllPreferencesQuery : IRequest<GetAllPreferencesDto>
    {
    }
}
