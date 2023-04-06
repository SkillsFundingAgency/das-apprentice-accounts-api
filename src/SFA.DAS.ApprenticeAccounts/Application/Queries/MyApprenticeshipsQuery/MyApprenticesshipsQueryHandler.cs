#nullable enable
using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.DTOs.MyApprenticeships;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.MyApprenticeshipsQuery
{
    public class MyApprenticeshipsQueryHandler
        : IRequestHandler<MyApprenticeshipsQuery, ApprenticeWithMyApprenticeshipsDto?>
    {
        private readonly IApprenticeContext _apprentices;
        private readonly IMyApprenticeshipContext _myApprenticeships;
    

        public  MyApprenticeshipsQueryHandler
            (IApprenticeContext apprenticeshipRepository,  IMyApprenticeshipContext myApprenticeships)
        {
            _apprentices = apprenticeshipRepository;
            _myApprenticeships = myApprenticeships;
        }

        public async Task<ApprenticeWithMyApprenticeshipsDto?> Handle(
            MyApprenticeshipsQuery request,
            CancellationToken cancellationToken)
        {
            var apprentice = await _apprentices.Find(request.ApprenticeId);
            var myApprenticeships = await _myApprenticeships.FindAll(request.ApprenticeId);
            return apprentice != null ? ApprenticeWithMyApprenticeshipsDto.Create(apprentice, myApprenticeships) : null;
        }
    }
}