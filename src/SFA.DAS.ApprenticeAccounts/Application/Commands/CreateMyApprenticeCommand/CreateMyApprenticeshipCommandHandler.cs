using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand
{
    public class CreateMyApprenticeshipCommandHandler : IRequestHandler<MyApprenticeship>
    {
        private readonly IMyApprenticeshipContext _myApprenticeships;
    
        public CreateMyApprenticeshipCommandHandler(IMyApprenticeshipContext myApprenticeships)
            => _myApprenticeships = myApprenticeships;
    
        public Task<Unit> Handle(MyApprenticeship command, CancellationToken cancellationToken)
        {
            _myApprenticeships.Add( command);
            return Unit.Task;
        }
    }
}