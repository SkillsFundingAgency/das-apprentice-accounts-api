using System;
using SFA.DAS.ApprenticeCommitments.Infrastructure.Mediator;

#nullable enable

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.RegistrationReminderSentCommand
{
    public class RegistrationReminderSentCommand : IUnitOfWorkCommand
    {
        public RegistrationReminderSentCommand(Guid apprenticeId, DateTime sentOn)
        {
            ApprenticeId = apprenticeId;
            SentOn = sentOn;
        }

        public Guid ApprenticeId { get; }
        public DateTime SentOn { get; }
    }
}