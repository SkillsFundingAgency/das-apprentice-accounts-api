using MediatR;
using SFA.DAS.ApprenticeAccounts.DTOs.MyApprenticeships;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.MyApprenticeshipsQuery;

public class MyApprenticeshipQuery : IRequest<ApprenticeWithMyApprenticeshipsDto>
{
    public MyApprenticeshipQuery(Guid id) => ApprenticeId = id;

    public Guid ApprenticeId
    {
        get;
        set;
    }
}