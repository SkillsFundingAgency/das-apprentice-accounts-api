using MediatR;
using SFA.DAS.ApprenticeAccounts.DTOs.MyApprenticeships;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.MyApprenticeshipsQuery;

public class MyApprenticeshipsQuery : IRequest<ApprenticeWithMyApprenticeshipsDto>
{
    public MyApprenticeshipsQuery(Guid id) => ApprenticeId = id;

    public Guid ApprenticeId
    {
        get;
        set;
    }
}