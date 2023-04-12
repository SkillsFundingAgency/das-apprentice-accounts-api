using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.DTOs.MyApprenticeships;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.MyApprenticeshipsQuery
{
    public class MyApprenticeshipQueryHandler
        : IRequestHandler<MyApprenticeshipQuery, ApprenticeWithMyApprenticeshipsDto?>
    {
        private readonly IApprenticeContext _apprentices;

        public  MyApprenticeshipQueryHandler
            (IApprenticeContext apprenticeshipRepository)
        {
            _apprentices = apprenticeshipRepository;
        }

        public async Task<ApprenticeWithMyApprenticeshipsDto?> Handle(
            MyApprenticeshipQuery request,
            CancellationToken cancellationToken)
        {
            var apprentice = await _apprentices.Find(request.ApprenticeId);
            return apprentice != null ? ApprenticeWithMyApprenticeshipsDto.Create(apprentice) : null;
        }
    }
}