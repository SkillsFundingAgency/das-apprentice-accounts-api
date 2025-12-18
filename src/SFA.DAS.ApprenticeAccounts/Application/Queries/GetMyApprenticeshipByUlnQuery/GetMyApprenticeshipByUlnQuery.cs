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
    public GetMyApprenticeshipByUlnQuery(long uln) => Uln = uln;

    public long Uln { get; set; }
}
