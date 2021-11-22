using MediatR;
using SFA.DAS.ApprenticeCommitments.Data;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.RegistrationFirstSeenCommand
{
    public class RegistrationFirstSeenCommandHandler : IRequestHandler<RegistrationFirstSeenCommand>
    {
        private readonly IRegistrationContext _registrations;

        public RegistrationFirstSeenCommandHandler(IRegistrationContext registrations)
        {
            _registrations = registrations;
        }

        public async Task<Unit> Handle(RegistrationFirstSeenCommand request, CancellationToken cancellationToken)
        {
            var registration = await _registrations.GetById(request.ApprenticeId);
            registration.ViewedByUser(request.SeenOn);
            return Unit.Value;
        }
    }
}