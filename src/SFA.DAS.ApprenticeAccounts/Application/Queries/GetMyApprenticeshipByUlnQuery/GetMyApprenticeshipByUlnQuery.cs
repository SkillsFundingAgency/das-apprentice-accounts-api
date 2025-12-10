using MediatR;
using SFA.DAS.ApprenticeAccounts.DTOs.MyApprenticeship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.GetMyApprenticeshipByUlnQuery;
public class GetMyApprenticeshipByUlnQuery : IRequest<ApprenticeWithMyApprenticeshipDto>
{
    public GetMyApprenticeshipByUlnQuery(int uln) => Uln = uln;

    public int Uln { get; set; }
}
