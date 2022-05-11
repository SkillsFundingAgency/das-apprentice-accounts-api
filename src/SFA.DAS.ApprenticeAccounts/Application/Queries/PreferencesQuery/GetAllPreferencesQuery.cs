using MediatR;
using SFA.DAS.ApprenticeAccounts.DTOs.Preferences;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.PreferencesQuery
{
    public class GetAllPreferencesQuery : IRequest<PreferencesDto>
    {
    }
}
