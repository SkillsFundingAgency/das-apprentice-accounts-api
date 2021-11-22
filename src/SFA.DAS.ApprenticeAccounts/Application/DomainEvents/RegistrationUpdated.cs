using MediatR;
using SFA.DAS.ApprenticeCommitments.Data.Models;

namespace SFA.DAS.ApprenticeCommitments.Application.DomainEvents
{
    public class RegistrationUpdated : INotification
    {
        public Registration Registration { get; }

        public RegistrationUpdated(Registration registration)
            => Registration = registration;
    }
}
