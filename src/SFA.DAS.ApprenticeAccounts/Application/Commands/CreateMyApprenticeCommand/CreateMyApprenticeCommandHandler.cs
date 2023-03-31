using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.CreateApprenticeAccountCommand
{
    public class CreateMyApprenticeCommandHandler : IRequestHandler<CreateApprenticeAccountCommand>
    {
        private readonly IApprenticeContext _apprentices;

        public CreateMyApprenticeCommandHandler(IApprenticeContext apprentices)
            => _apprentices = apprentices;

        public Task<Unit> Handle(CreateApprenticeAccountCommand request, CancellationToken cancellationToken)
        {
            _apprentices.Add(new Apprentice(
                request.ApprenticeId,
                request.FirstName,
                request.LastName,
                new MailAddress(request.Email),
                request.DateOfBirth
                ));

            return Unit.Task;
        }
    }
}