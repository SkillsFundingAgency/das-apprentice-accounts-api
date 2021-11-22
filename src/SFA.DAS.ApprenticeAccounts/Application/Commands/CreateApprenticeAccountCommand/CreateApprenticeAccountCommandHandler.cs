using MediatR;
using SFA.DAS.ApprenticeCommitments.Data;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApprenticeAccountCommand
{
    public class CreateApprenticeAccountCommandHandler : IRequestHandler<CreateApprenticeAccountCommand>
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
                request.DateOfBirth
                ));

            return Unit.Task;
        }
    }
}