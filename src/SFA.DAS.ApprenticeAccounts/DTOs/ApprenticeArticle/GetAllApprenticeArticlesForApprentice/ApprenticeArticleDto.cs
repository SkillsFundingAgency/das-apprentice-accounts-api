using System;

namespace SFA.DAS.ApprenticeAccounts.DTOs.ApprenticeArticle.GetAllApprenticeArticlesForApprentice
{
    public class ApprenticeArticleDto
    {
        public string? EntryId { get; set; }
        public bool? IsSaved { get; set; }
        public bool? LikeStatus { get; set; }
    }
}