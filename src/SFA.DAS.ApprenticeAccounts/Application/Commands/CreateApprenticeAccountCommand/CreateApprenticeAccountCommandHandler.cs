using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.CreateApprenticeAccountCommand
{
    public class CreateApprenticeAccountCommandHandler : IRequestHandler<CreateApprenticeAccountCommand, Unit>
    {
        private readonly IApprenticeContext _apprentices;

        public CreateApprenticeAccountCommandHandler(IApprenticeContext apprentices)
            => _apprentices = apprentices;

        public Task<Unit> Handle(CreateApprenticeAccountCommand request, CancellationToken cancellationToken)
        {
            _apprentices.Add(new Apprentice(
                request.ApprenticeId,
                request.FirstName,
                request.LastName,
                new MailAddress(request.Email),
                request.DateOfBirth,
                request.GovUkIdentifier,
                request.AppLastLoggedIn
                ));

            return Unit.Task;
        }
    }
}