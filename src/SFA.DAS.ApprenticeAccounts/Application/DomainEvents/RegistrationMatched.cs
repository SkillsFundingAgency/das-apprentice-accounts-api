using MediatR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using SFA.DAS.ApprenticeCommitments.Messages.Events;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.DomainEvents
{
    internal class RegistrationMatched : INotification
    {
        public Registration Registration { get; }
        public Apprentice Apprentice { get; }

        public RegistrationMatched(Registration registration, Apprentice apprentice)
        {
            Registration = registration;
            Apprentice = apprentice;
        }
    }

    internal class RegistrationMatchedHandler : INotificationHandler<RegistrationMatched>
    {
        private readonly IMessageSession messageSession;
        private readonly ILogger<RegistrationMatchedHandler> logger;

        public RegistrationMatchedHandler(IMessageSession messageSession, ILogger<RegistrationMatchedHandler> logger)
        {
            this.messageSession = messageSession;
            this.logger = logger;
        }

        public async Task Handle(RegistrationMatched notification, CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "RegistrationMatched - Publishing ApprenticeshipEmailAddressConfirmedEvent for Apprentice {ApprenticeId}, CommitmentApprenticeship {CommitmentsApprenticeshipId}",
                notification.Apprentice.Id,
                notification.Registration.CommitmentsApprenticeshipId);

            await messageSession.Publish(new ApprenticeshipEmailAddressConfirmedEvent
            {
                ApprenticeId = notification.Apprentice.Id,
                CommitmentsApprenticeshipId = notification.Registration.CommitmentsApprenticeshipId,
            });
        }
    }
}