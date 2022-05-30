using SFA.DAS.ApprenticeAccounts.Infrastructure.Mediator;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateApprenticePreferencesCommand
{
    public class UpdateApprenticePreferencesCommand : IUnitOfWorkCommand
    {
        public List<UpdateApprenticePreferenceCommand> ApprenticePreferences { get; set; }
    }
}
