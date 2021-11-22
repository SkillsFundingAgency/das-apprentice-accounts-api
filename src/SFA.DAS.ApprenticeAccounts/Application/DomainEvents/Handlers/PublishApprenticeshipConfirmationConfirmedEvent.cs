using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.ApprenticeCommitments.Exceptions;
using SFA.DAS.ApprenticeCommitments.Messages.Events;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.DomainEvents
{
    internal class PublishApprenticeshipConfirmationConfirmedEvent : INotificationHandler<RevisionConfirmed>
    {
        private readonly IMessageSession messageSession;
        private readonly ILogger<PublishApprenticeshipConfirmationConfirmedEvent> logger;

        public PublishApprenticeshipConfirmationConfirmedEvent(
            IMessageSession messageSession,
            ILogger<PublishApprenticeshipConfirmationConfirmedEvent> logger)
        {
            this.messageSession = messageSession;
            this.logger = logger;
        }

        public async Task Handle(RevisionConfirmed notification, CancellationToken cancellationToken)
        {
            if (notification.Revision.ConfirmedOn == null)
                throw new DomainException($"Apprenticeship {notification.Revision.ApprenticeshipId} revision {notification.Revision.Id} has not been confirmed");

            logger.LogInformation(
                "Publishing ApprenticeshipConfirmationConfirmedEvent for Apprentice {ApprenticeId}, Apprenticeship {ApprenticeshipId}, confirmed on {ConfirmedOn}",
                notification.Revision.Apprenticeship.ApprenticeId,
                notification.Revision.ApprenticeshipId,
                notification.Revision.ConfirmedOn.Value);

            await messageSession.Publish(new ApprenticeshipConfirmationConfirmedEvent
            {
                ApprenticeId = notification.Revision.Apprenticeship.ApprenticeId,
                ApprenticeshipId = notification.Revision.ApprenticeshipId,
                ConfirmationId = notification.Revision.Id,
                ConfirmedOn = notification.Revision.ConfirmedOn.Value,
                CommitmentsApprenticeshipId = notification.Revision.CommitmentsApprenticeshipId,
                CommitmentsApprovedOn = notification.Revision.CommitmentsApprovedOn,
            });
        }
    }
}