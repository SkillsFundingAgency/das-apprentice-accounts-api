using MediatR;
using SFA.DAS.ApprenticeAccounts.DTOs.MyApprenticeship;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.MyApprenticeshipQuery;

public class MyApprenticeshipQuery : IRequest<ApprenticeDto>
{
    public MyApprenticeshipQuery(Guid id, int? apprenticeshipId)
    {
        ApprenticeId = id;
        ApprenticeshipId = apprenticeshipId;
    }

    public Guid ApprenticeId { get; set; }
    public int? ApprenticeshipId { get; set; }
}