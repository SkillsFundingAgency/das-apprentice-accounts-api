using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.ApprenticesQuery;

public class GetApprenticesHandler : IRequestHandler<GetApprenticesQuery, ApprenticeSyncResponseDto>
{
    private readonly IApprenticeContext _apprentices;

    public GetApprenticesHandler(IApprenticeContext apprenticeshipRepository)
    {
        _apprentices = apprenticeshipRepository;
    }

    public async Task<ApprenticeSyncResponseDto> Handle(GetApprenticesQuery request, CancellationToken cancellationToken)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (!request.ApprenticeIds.Any())
            return new ApprenticeSyncResponseDto();

        var apprentices = await _apprentices.GetForSync(request.ApprenticeIds, request.UpdatedSince);

        if (!apprentices.Any())
            return new ApprenticeSyncResponseDto();

        var syncedApprentices = apprentices.Select(apprentice => ApprenticeSyncDto.MapToSyncResponse(apprentice)).ToArray();

        return new ApprenticeSyncResponseDto(syncedApprentices);
    }
}
