using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand
{
    public class CreateMyApprenticeshipCommandHandler : IRequestHandler<CreateMyApprenticeshipCommand>
    {
        private readonly IMyApprenticeshipContext _myApprenticeships;
    
        public CreateMyApprenticeshipCommandHandler(IMyApprenticeshipContext myApprenticeships)
            => _myApprenticeships = myApprenticeships;
    
        public Task<Unit> Handle(CreateMyApprenticeshipCommand command, CancellationToken cancellationToken)
        {
            _myApprenticeships.Add(new MyApprenticeship(Guid.NewGuid(),command));
    
            return Unit.Task;
        }
    }
}