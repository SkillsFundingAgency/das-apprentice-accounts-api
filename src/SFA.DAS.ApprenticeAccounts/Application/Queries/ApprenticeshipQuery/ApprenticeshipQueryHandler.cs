using MediatR;
using SFA.DAS.ApprenticeCommitments.Data;
using SFA.DAS.ApprenticeCommitments.DTOs;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace SFA.DAS.ApprenticeCommitments.Application.Queries.ApprenticeshipQuery
{
    public class ApprenticeshipQueryHandler : IRequestHandler<ApprenticeshipQuery, ApprenticeshipDto?>
    {
        private readonly IApprenticeshipContext _apprenticeshipRepository;

        public ApprenticeshipQueryHandler(IApprenticeshipContext apprenticeshipRepository)
            => _apprenticeshipRepository = apprenticeshipRepository;

        public async Task<ApprenticeshipDto?> Handle(
            ApprenticeshipQuery request,
            CancellationToken cancellationToken)
        {
            var entity = await _apprenticeshipRepository
                .FindForApprentice(request.ApprenticeId, request.ApprenticeshipId);

            return entity?.MapToApprenticeshipDto();
        }
    }
}