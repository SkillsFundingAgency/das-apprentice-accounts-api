#nullable enable
using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using System.Threading;
using System.Threading.Tasks;
namespace SFA.DAS.ApprenticeAccounts.Application.Queries.ApprenticesQuery
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