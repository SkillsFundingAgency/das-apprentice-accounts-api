using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using SFA.DAS.ApprenticeCommitments.Messages.Events;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.DomainEvents
{
    internal class PublishApprenticeshipConfirmationCommencedEvent
        : INotificationHandler<RegistrationAdded>
        , INotificationHandler<RevisionAdded>
    {
        private readonly IMessageSession messageSession;
        private readonly ILogger<PublishApprenticeshipConfirmationCommencedEvent> logger;

        public PublishApprenticeshipConfirmationCommencedEvent(
            IMessageSession messageSession,
            ILogger<PublishApprenticeshipConfirmationCommencedEvent> logger)
        {
            this.messageSession = messageSession;
            this.logger = logger;
        }

        public async Task Handle(RegistrationAdded notification, CancellationToken cancellationToken)
        {
            // The revision isn't technically added until the apprentice
            // confirms their identity, however it's possible that the apprentice
            // never does.  Approvals can use this event to prompt the apprentice
            // when the confirmation is overdue.
            var pretend = new Revision(
                notification.Registration.CommitmentsApprenticeshipId,
                notification.Registration.CommitmentsApprovedOn,
                notification.Registration.Apprenticeship);

            logger.LogInformation(
                "RegistrationAdded - Publishing ApprenticeshipConfirmationCommencedEvent for Apprentice {ApprenticeId}, Apprenticeship {ApprenticeshipId}",
                notification.Registration.RegistrationId,
                notification.Registration.CommitmentsApprenticeshipId);

            await messageSession.Publish(new ApprenticeshipConfirmationCommencedEvent
            {
                ApprenticeId = notification.Registration.RegistrationId,
                ConfirmationOverdueOn = pretend.ConfirmBefore,
                CommitmentsApprenticeshipId = notification.Registration.CommitmentsApprenticeshipId,
                CommitmentsApprovedOn = notification.Registration.CommitmentsApprovedOn,
            });
        }

        public async Task Handle(RevisionAdded notification, CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "RevisionAdded - Publishing ApprenticeshipConfirmationCommencedEvent for Apprentice {ApprenticeId}, Apprenticeship {ApprenticeshipId}",
                notification.Revision.Apprenticeship.ApprenticeId,
                notification.Revision.ApprenticeshipId);

            await messageSession.Publish(new ApprenticeshipConfirmationCommencedEvent
            {
                ApprenticeId = notification.Revision.Apprenticeship.ApprenticeId,
                ApprenticeshipId = notification.Revision.ApprenticeshipId,
                ConfirmationId = notification.Revision.Id,
                ConfirmationOverdueOn = notification.Revision.ConfirmBefore,
                CommitmentsApprenticeshipId = notification.Revision.CommitmentsApprenticeshipId,
                CommitmentsApprovedOn = notification.Revision.CommitmentsApprovedOn,
            });
        }
    }
}