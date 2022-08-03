using SFA.DAS.ApprenticeAccounts.Infrastructure.Mediator;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateAllApprenticePreferencesCommand
{
    public class UpdateAllApprenticePreferencesCommand : IUnitOfWorkCommand
    {
        public List<UpdateApprenticePreferenceCommand.UpdateApprenticePreferenceCommand> ApprenticePreferences { get; set; } = null!;
    }
}
