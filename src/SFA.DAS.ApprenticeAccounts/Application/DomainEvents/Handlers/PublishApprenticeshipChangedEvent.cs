using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.ApprenticeCommitments.Messages.Events;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.DomainEvents
{
    internal class PublishApprenticeshipChangedEvent : INotificationHandler<ApprenticeshipChanged>
    {
        private readonly IMessageSession _messageSession;
        private readonly ILogger<PublishApprenticeshipChangedEvent> _logger;

        public PublishApprenticeshipChangedEvent(IMessageSession messageSession, ILogger<PublishApprenticeshipChangedEvent> logger)
        {
            _messageSession = messageSession;
            _logger = logger;
        }

        public async Task Handle(ApprenticeshipChanged notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Publishing ApprenticeshipChangedEvent for {apprentice} -- {apprenticeship}",
                notification.Apprenticeship.ApprenticeId, notification.Apprenticeship.Id);

            await _messageSession.Publish(new ApprenticeshipChangedEvent
            {
                ApprenticeId = notification.Apprenticeship.ApprenticeId,
                ApprenticeshipId = notification.Apprenticeship.Id,
            });
        }
    }
}