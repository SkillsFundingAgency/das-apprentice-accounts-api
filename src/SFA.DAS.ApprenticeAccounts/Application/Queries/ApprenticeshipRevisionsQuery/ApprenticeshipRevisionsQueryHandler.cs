using MediatR;
using SFA.DAS.ApprenticeCommitments.Data;
using SFA.DAS.ApprenticeCommitments.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Queries.ApprenticeshipRevisionsQuery
{
    public class ApprenticeshipRevisionsQueryHandler : IRequestHandler<ApprenticeshipRevisionsQuery, ApprenticeshipRevisionsDto?>
    {
        private readonly IApprenticeshipContext _apprenticeshipRepository;

        public ApprenticeshipRevisionsQueryHandler(IApprenticeshipContext apprenticeshipRepository)
            => _apprenticeshipRepository = apprenticeshipRepository;

        public async Task<ApprenticeshipRevisionsDto?> Handle(
            ApprenticeshipRevisionsQuery request,
            CancellationToken cancellationToken)
        {
            var entity = await _apprenticeshipRepository
                .FindForApprentice(request.ApprenticeId, request.ApprenticeshipId);

            return entity?.MapToApprenticeshipRevisionsDto();
        }
    }
}