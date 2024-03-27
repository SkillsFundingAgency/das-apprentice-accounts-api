using MediatR;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.ApprenticesQuery;

public class GetApprenticesQuery : IRequest<ApprenticeSyncResponseDto>
{
    public GetApprenticesQuery(DateTime? updatedSince, Guid[] apprenticeIds)
    {
        UpdatedSince = updatedSince;
        ApprenticeIds = apprenticeIds;
    }

    [JsonProperty("updatedSince")]
    public DateTime? UpdatedSince { get; set; }

    [JsonProperty("apprenticeIds")]
    public Guid[] ApprenticeIds { get; set; } = Array.Empty<Guid>();
}
