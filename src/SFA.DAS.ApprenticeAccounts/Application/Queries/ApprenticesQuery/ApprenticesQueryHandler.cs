using MediatR;
using SFA.DAS.ApprenticeCommitments.Data;
using SFA.DAS.ApprenticeCommitments.DTOs;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace SFA.DAS.ApprenticeCommitments.Application.Queries.ApprenticesQuery
{
    public class ApprenticesQueryHandler
        : IRequestHandler<ApprenticesQuery, ApprenticeDto?>
    {
        private readonly IApprenticeContext _apprentices;

        public ApprenticesQueryHandler(IApprenticeContext apprenticeshipRepository)
            => _apprentices = apprenticeshipRepository;

        public async Task<ApprenticeDto?> Handle(
            ApprenticesQuery request,
            CancellationToken cancellationToken)
        {
            var a = await _apprentices.Find(request.ApprenticeId);
            return a.MapToApprenticeDto();
        }
    }
}