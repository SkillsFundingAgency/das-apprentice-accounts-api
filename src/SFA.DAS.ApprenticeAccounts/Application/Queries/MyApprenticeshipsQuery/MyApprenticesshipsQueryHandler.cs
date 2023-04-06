#nullable enable
using MediatR;
using SFA.DAS.ApprenticeAccounts.Configuration;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using SFA.DAS.ApprenticeAccounts.DTOs.MyApprenticeships;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.MyApprenticeshipsQuery
{
    public class MyApprenticeshipsQueryHandler
        : IRequestHandler<MyApprenticeshipsQuery, ApprenticeMyApprenticeshipsDto?>
    {
        private readonly IApprenticeContext _apprentices;
        private readonly IMyApprenticeshipContext _myApprenticeships;
    

        public  MyApprenticeshipsQueryHandler
            (IApprenticeContext apprenticeshipRepository, ApplicationSettings settings, IMyApprenticeshipContext myApprenticeships)
        {
            _apprentices = apprenticeshipRepository;
            _myApprenticeships = myApprenticeships;
        }

        public async Task<ApprenticeMyApprenticeshipsDto?> Handle(
            MyApprenticeshipsQuery request,
            CancellationToken cancellationToken)
        {
            var apprentice = await _apprentices.Find(request.ApprenticeId);
            var myApprenticeships = await _myApprenticeships.FindAll(request.ApprenticeId);
           
            return apprentice != null ? ApprenticeMyApprenticeshipsDto.Create(apprentice, myApprenticeships) : null;
        }
    }
}