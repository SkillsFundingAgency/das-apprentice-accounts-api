using System;
using SFA.DAS.ApprenticeCommitments.Infrastructure.Mediator;

#nullable enable

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.RegistrationFirstSeenCommand
{
    public class RegistrationFirstSeenCommand : IUnitOfWorkCommand
    {
        public RegistrationFirstSeenCommand(Guid apprenticeId, DateTime seenOn)
        {
            ApprenticeId = apprenticeId;
            SeenOn = seenOn;
        }

        public Guid ApprenticeId { get; }
        public DateTime SeenOn { get; }
    }
}