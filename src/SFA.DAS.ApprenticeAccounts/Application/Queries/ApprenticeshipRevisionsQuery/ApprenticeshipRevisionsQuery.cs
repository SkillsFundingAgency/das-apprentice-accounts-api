using MediatR;
using SFA.DAS.ApprenticeCommitments.DTOs;
using System;

namespace SFA.DAS.ApprenticeCommitments.Application.Queries.ApprenticeshipRevisionsQuery
{
    public class ApprenticeshipRevisionsQuery : IRequest<ApprenticeshipRevisionsDto>
    {
        public ApprenticeshipRevisionsQuery(Guid apprenticeId, long apprenticeshipId)
        {
            ApprenticeId = apprenticeId;
            ApprenticeshipId = apprenticeshipId;
        }

        public Guid ApprenticeId { get; }
        public long ApprenticeshipId { get; }
    }
}