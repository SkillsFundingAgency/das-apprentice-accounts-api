using MediatR;
using SFA.DAS.ApprenticeCommitments.Data.Models;

namespace SFA.DAS.ApprenticeCommitments.Application.DomainEvents
{
    internal class RegistrationAdded : INotification
    {
        public Registration Registration { get; }

        public RegistrationAdded(Registration registration)
            => Registration = registration;
    }
}