using MediatR;
using SFA.DAS.ApprenticeAccounts.DTOs.MyApprenticeship;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.MyApprenticeshipQuery;

public class MyApprenticeshipQuery : IRequest<ApprenticeWithMyApprenticeshipsDto>
{
    public MyApprenticeshipQuery(Guid id) => ApprenticeId = id;

    public Guid ApprenticeId { get; set; }
}