using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeCommitments.Data;
using SFA.DAS.ApprenticeCommitments.Infrastructure;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.ConfirmCommand
{
    public class ConfirmCommandHandler
        : IRequestHandler<ConfirmCommand>
    {
        private readonly ITimeProvider _time; 
        private readonly IApprenticeshipContext _apprenticeships;

        public ConfirmCommandHandler(IApprenticeshipContext apprenticeships, ITimeProvider time)
        {
            _apprenticeships = apprenticeships;
            _time = time;
        }

        public async Task<Unit> Handle(ConfirmCommand request, CancellationToken cancellationToken)
        {
            var apprenticeship = await _apprenticeships.GetById(request.ApprenticeId, request.ApprenticeshipId);
            apprenticeship.Confirm(request.RevisionId, request.Confirmations, _time.Now);
            return Unit.Value;
        }
    }
}