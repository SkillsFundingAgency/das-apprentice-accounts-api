using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.DTOs.MyApprenticeship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.GetMyApprenticeshipByUlnQuery;
public class GetMyApprenticeshipByUlnQueryHandler : IRequestHandler<GetMyApprenticeshipByUlnQuery, ApprenticeWithMyApprenticeshipDto>
{    
    private readonly IMyApprenticeshipContext _myApprenticeships;

    public GetMyApprenticeshipByUlnQueryHandler
        (IMyApprenticeshipContext myApprenticeships)
    {        
        _myApprenticeships = myApprenticeships;
    }

    public async Task<ApprenticeWithMyApprenticeshipDto> Handle(
        GetMyApprenticeshipByUlnQuery request,
        CancellationToken cancellationToken)
    {
        var myApprenticeship = await _myApprenticeships.FindByUln(request.Uln);

        return new ApprenticeWithMyApprenticeshipDto
        {
            MyApprenticeship = myApprenticeship
        };
    }
}
