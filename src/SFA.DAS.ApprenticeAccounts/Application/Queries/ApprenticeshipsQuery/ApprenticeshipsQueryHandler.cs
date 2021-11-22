using MediatR;
using SFA.DAS.ApprenticeCommitments.Data;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using SFA.DAS.ApprenticeCommitments.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace SFA.DAS.ApprenticeCommitments.Application.Queries.ApprenticeshipsQuery
{
    public class ApprenticeshipsQueryHandler
        : IRequestHandler<ApprenticeshipsQuery, ApprenticeshipsResponse>
    {
        private readonly IApprenticeshipContext _apprenticeshipRepository;

        public ApprenticeshipsQueryHandler(IApprenticeshipContext apprenticeshipRepository)
            => _apprenticeshipRepository = apprenticeshipRepository;

        public async Task<ApprenticeshipsResponse> Handle(
            ApprenticeshipsQuery request,
            CancellationToken cancellationToken)
        {
            List<Apprenticeship> apprenticeships = await _apprenticeshipRepository
                .FindAllForApprentice(request.ApprenticeId);

            var dtos = apprenticeships
                .ConvertAll(x => x.MapToApprenticeshipDto())
                .OrderByDescending(x => x.ApprovedOn)
                .ToList();

            return new ApprenticeshipsResponse(dtos);
        }
    }
}