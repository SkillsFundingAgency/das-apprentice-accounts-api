using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.DomainEvents.Handlers
{
    internal class PublishApprenticeEmailAddressChanged : INotificationHandler<ApprenticeEmailAddressChanged>
    {
        private readonly IMessageSession _messageSession;
        private readonly ILogger<PublishApprenticeEmailAddressChanged> _logger;

        public PublishApprenticeEmailAddressChanged(IMessageSession messageSession, ILogger<PublishApprenticeEmailAddressChanged> logger)
        {
            _messageSession = messageSession;
            _logger = logger;
        }

        public async Task Handle(ApprenticeEmailAddressChanged notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Publishing ApprenticeEmailAddressChanged for {apprentice}",
                notification.Apprentice.Id);

            await _messageSession.Publish(new Messages.Events.ApprenticeEmailAddressChanged
            {
                ApprenticeId = notification.Apprentice.Id,
                ChangedOn = DateTime.Now,
            });
        }
    }
}