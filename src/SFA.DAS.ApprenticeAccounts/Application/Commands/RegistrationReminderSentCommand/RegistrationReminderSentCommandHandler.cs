using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeCommitments.Data;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.RegistrationReminderSentCommand
{
    public class RegistrationReminderSentCommandHandler : IRequestHandler<RegistrationReminderSentCommand>
    {
        private readonly IRegistrationContext _registrations;

        public RegistrationReminderSentCommandHandler(IRegistrationContext registrations)
        {
            _registrations = registrations;
        }

        public async Task<Unit> Handle(RegistrationReminderSentCommand request, CancellationToken cancellationToken)
        {
            var registration = await _registrations.GetById(request.ApprenticeId);
            registration.SignUpReminderSent(request.SentOn);
            return Unit.Value;
        }
    }
}