using Newtonsoft.Json;
using System;

namespace SFA.DAS.ApprenticeAccounts.DTOs.ApprenticeArticle;
public class ApprenticeArticleSyncResponseDto
{
    public ApprenticeArticleSyncResponseDto() => ApprenticeArticles = Array.Empty<ApprenticeArticleSyncDto>();
    public ApprenticeArticleSyncResponseDto(ApprenticeArticleSyncDto[] apprentices) => ApprenticeArticles = apprentices;

    [JsonProperty("apprenticearticles")]
    public ApprenticeArticleSyncDto[] ApprenticeArticles {  get; set; }
}
