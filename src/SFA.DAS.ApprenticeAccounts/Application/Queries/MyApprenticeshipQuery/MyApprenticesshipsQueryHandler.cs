using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.DTOs.MyApprenticeship;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.MyApprenticeshipQuery
{
    public class MyApprenticeshipQueryHandler
        : IRequestHandler<MyApprenticeshipQuery, ApprenticeDto?>
    {
        private readonly IApprenticeContext _apprentices;
        private readonly IMyApprenticeshipContext _myApprenticeships;

        public  MyApprenticeshipQueryHandler
            (IApprenticeContext apprenticeshipRepository, IMyApprenticeshipContext myApprenticeships)
        {
            _apprentices = apprenticeshipRepository;
            _myApprenticeships = myApprenticeships;
        }

        public async Task<ApprenticeDto?> Handle(
            Queries.MyApprenticeshipQuery.MyApprenticeshipQuery request,
            CancellationToken cancellationToken)
        {
            var apprentice = await _apprentices.Find(request.ApprenticeId);
            if (apprentice == null) return null;
            var myApprenticeships = await _myApprenticeships.FindAll(request.ApprenticeId);
            return ApprenticeDto.Create(apprentice, myApprenticeships);
        }
    }
}