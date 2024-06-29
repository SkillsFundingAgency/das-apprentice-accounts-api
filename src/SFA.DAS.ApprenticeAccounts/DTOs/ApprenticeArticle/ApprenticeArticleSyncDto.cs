using Newtonsoft.Json;
using System;

#nullable enable

namespace SFA.DAS.ApprenticeAccounts.DTOs.ApprenticeArticle;
public class ApprenticeArticleSyncDto
{
    [JsonProperty("id")]
    public Guid Id { get; set; }

    [JsonProperty("entryid")]
    public string? EntryId { get; set; } = null!;

    [JsonProperty("issaved")]
    public bool? IsSaved { get; set; } = null!;

    [JsonProperty("likestatus")]
    public bool? LikeStatus { get; set; }


    public static ApprenticeArticleSyncDto MapToSyncResponse(Data.Models.ApprenticeArticle apprenticeArticle)
    {
        return new ApprenticeArticleSyncDto()
        {
            Id = apprenticeArticle.Id,
            EntryId = apprenticeArticle.EntryId,
            IsSaved = apprenticeArticle.IsSaved,
            LikeStatus = apprenticeArticle.LikeStatus,
        };
    }
}
