using SFA.DAS.ApprenticeCommitments.DTOs;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeCommitments.Application.Queries.ApprenticeshipsQuery
{
    public class ApprenticeshipsResponse
    {
        public ApprenticeshipsResponse(List<ApprenticeshipDto> apprenticeships)
            => Apprenticeships = apprenticeships;

        public List<ApprenticeshipDto> Apprenticeships { get; }
    }
}