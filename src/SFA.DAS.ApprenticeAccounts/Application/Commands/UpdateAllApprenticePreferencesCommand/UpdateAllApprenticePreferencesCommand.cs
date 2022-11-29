using MediatR;
using SFA.DAS.ApprenticeAccounts.Infrastructure.Mediator;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateAllApprenticePreferencesCommand
{
    public class UpdateAllApprenticePreferencesCommand : IRequest<Unit>, IUnitOfWorkCommand
    {
        public List<UpdateApprenticePreferenceCommand> ApprenticePreferences { get; set; } = null!;
        public Guid ApprenticeId { get; set; }
    }
}
