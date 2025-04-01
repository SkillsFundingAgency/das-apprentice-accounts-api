using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.AddUpdateApprenticeArticleCommand;

public class AddOrUpdateApprenticeArticleRequest
{
    public Guid Id { get; set; }
    public string EntryId { get; set; }
    public string EntryTitle { get; set; }
    public bool? IsSaved { get; set; }
    public bool? LikeStatus { get; set; }
    public DateTime? SaveTime { get; set; }
}