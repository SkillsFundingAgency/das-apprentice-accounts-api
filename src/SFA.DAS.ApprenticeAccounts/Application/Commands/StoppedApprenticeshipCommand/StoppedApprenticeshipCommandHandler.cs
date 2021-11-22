using MediatR;
using SFA.DAS.ApprenticeCommitments.Data;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using SFA.DAS.ApprenticeCommitments.Exceptions;
using SFA.DAS.ApprenticeCommitments.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.StoppedApprenticeshipCommand
{
    public class ChangeRegistrationCommandHandler : IRequestHandler<StoppedApprenticeshipCommand>
    {
        private readonly IRevisionContext _revisions;
        private readonly ITimeProvider _timeProvider;

        public ChangeRegistrationCommandHandler(IRevisionContext revisions, ITimeProvider timeProvider) =>
            (_revisions, _timeProvider) = (revisions, timeProvider);

        public async Task<Unit> Handle(StoppedApprenticeshipCommand request, CancellationToken cancellationToken)
        {
            var apprenticeship =
                await _revisions.FindLatestByCommitmentsApprenticeshipId(request.CommitmentsApprenticeshipId)
                ?? throw new EntityNotFoundException(nameof(Revision), request.CommitmentsApprenticeshipId.ToString());

            apprenticeship.StoppedReceivedOn = _timeProvider.Now;

            return Unit.Value;
        }
    }
}
