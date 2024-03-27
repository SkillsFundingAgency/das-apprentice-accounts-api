using Newtonsoft.Json;
using System;

namespace SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
public class ApprenticeSyncResponseDto
{
    public ApprenticeSyncResponseDto() => Apprentices = Array.Empty<ApprenticeSyncDto>();
    public ApprenticeSyncResponseDto(ApprenticeSyncDto[] apprentices) => Apprentices = apprentices;

    [JsonProperty("apprentices")]
    public ApprenticeSyncDto[] Apprentices {  get; set; }
}
