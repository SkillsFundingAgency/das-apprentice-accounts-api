using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand
{
    public class CreateMyApprenticeCommandHandler : IRequestHandler<CreateMyApprenticeshipCommand>
    {
        private readonly IMyApprenticeshipContext _myApprenticeships;
    
        public CreateMyApprenticeCommandHandler(IMyApprenticeshipContext myApprenticeships)
            => _myApprenticeships = myApprenticeships;
    
        public Task<Unit> Handle(CreateMyApprenticeshipCommand request, CancellationToken cancellationToken)
        {
            _myApprenticeships.Add(new MyApprenticeship(Guid.NewGuid(),request.ApprenticeId,request.Uln,request.ApprenticeshipId,
                request.EmployerName,request.StartDate,request.EndDate,request.TrainingProviderId,request.TrainingProviderName,
                request.TrainingCode,request.StandardUId)

            );
    
            return Unit.Task;
        }
    }
}