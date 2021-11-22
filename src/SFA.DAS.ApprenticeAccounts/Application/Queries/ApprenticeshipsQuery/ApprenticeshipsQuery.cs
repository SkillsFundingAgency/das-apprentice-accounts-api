using MediatR;
using System;

namespace SFA.DAS.ApprenticeCommitments.Application.Queries.ApprenticeshipsQuery
{
    public class ApprenticeshipsQuery : IRequest<ApprenticeshipsResponse>
    {
        public ApprenticeshipsQuery(Guid Id) => ApprenticeId = Id;

        public Guid ApprenticeId { get; set; }
    }
}