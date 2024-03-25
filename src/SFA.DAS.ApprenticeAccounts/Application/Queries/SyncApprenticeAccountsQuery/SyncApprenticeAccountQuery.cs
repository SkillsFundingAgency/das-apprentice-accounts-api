using MediatR;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.SyncApprenticeAccountsQuery;

public class SyncApprenticeAccountQuery : IRequest<ApprenticeSyncResponseDto>
{
    public SyncApprenticeAccountQuery(DateTime? updatedSince, Guid[] apprenticeIDs)
    {
        UpdatedSince = updatedSince;
        ApprenticeIDs = apprenticeIDs;
    }

    [JsonProperty("updatedSince")]
    public DateTime? UpdatedSince { get; set; }

    [JsonProperty("apprenticeIds")]
    public Guid[] ApprenticeIDs { get; set; } = Array.Empty<Guid>();
}
