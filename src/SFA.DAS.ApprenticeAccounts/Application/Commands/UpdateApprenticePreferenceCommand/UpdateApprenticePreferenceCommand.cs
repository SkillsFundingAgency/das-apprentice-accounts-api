using MediatR;
using SFA.DAS.ApprenticeAccounts.Infrastructure.Mediator;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateApprenticePreferenceCommand
{
    public class UpdateApprenticePreferenceCommand : IRequest<Unit>, IUnitOfWorkCommand
    {
        public Guid ApprenticeId { get; set; }
        public int PreferenceId { get; set; }
        public bool Status { get; set; }
    }
}
