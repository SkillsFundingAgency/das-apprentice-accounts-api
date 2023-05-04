using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand
{
    public class CreateMyApprenticeshipCommandHandler : IRequestHandler<CreateMyApprenticeshipCommand>
    {
        private readonly IMyApprenticeshipContext _myApprenticeships;

        public CreateMyApprenticeshipCommandHandler(IMyApprenticeshipContext myApprenticeships)
        {
            _myApprenticeships = myApprenticeships;
        }

        public async Task<Unit> Handle(CreateMyApprenticeshipCommand command, CancellationToken cancellationToken)
        {
            _myApprenticeships.Add( command);
            return await Unit.Task;
        }
    }
}