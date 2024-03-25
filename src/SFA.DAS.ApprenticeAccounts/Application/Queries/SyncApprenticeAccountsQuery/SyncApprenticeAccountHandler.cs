using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.SyncApprenticeAccountsQuery;

public class SyncApprenticeAccountHandler : IRequestHandler<SyncApprenticeAccountQuery, ApprenticeSyncResponseDto>
{
    private readonly ILogger<SyncApprenticeAccountHandler> _logger;

    private readonly IApprenticeContext _apprentices;

    public SyncApprenticeAccountHandler(ILogger<SyncApprenticeAccountHandler> logger, IApprenticeContext apprenticeshipRepository)
    {
        _logger = logger;
        _apprentices = apprenticeshipRepository;
    }

    public async Task<ApprenticeSyncResponseDto> Handle(SyncApprenticeAccountQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            _logger.LogInformation("Beginning apprentice sync for date: {QueryDate} and Ids: [{ApprenticeIDs}]",
                request.UpdatedSince?.ToString(),
                string.Join(',', request.ApprenticeIDs)
            );

            if (!request.ApprenticeIDs.Any())
            {
                new ApprenticeSyncResponseDto();
            }

            var apprentices = _apprentices.Entities.AsNoTracking()
                .Where(a => request.ApprenticeIDs.Contains(a.Id) && 
                            (!request.UpdatedSince.HasValue || a.UpdatedOn.Date > request.UpdatedSince.Value.Date));

            if(!apprentices.Any())
            {
                return new ApprenticeSyncResponseDto();
            }

            ApprenticeSyncDto[] syncedApprentices = apprentices.Select(apprentice => ApprenticeSyncDto.MapToSyncResponse(apprentice)).ToArray();

            return new ApprenticeSyncResponseDto(syncedApprentices);
        }
        catch(Exception _exception) 
        {
            _logger.LogError(_exception, "Unable to sync apprentice records");
            return new ApprenticeSyncResponseDto();
        }
    }
}
