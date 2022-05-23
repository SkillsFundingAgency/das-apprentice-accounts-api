using SFA.DAS.ApprenticeAccounts.Infrastructure.Mediator;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateApprenticePreferencesCommand
{
    public class UpdateApprenticePreferencesCommand : IUnitOfWorkCommand
    {
        public Guid ApprenticeId { get; set; }
        public int PreferenceId { get; set; }
        public bool Status { get; set; }
    }
}
