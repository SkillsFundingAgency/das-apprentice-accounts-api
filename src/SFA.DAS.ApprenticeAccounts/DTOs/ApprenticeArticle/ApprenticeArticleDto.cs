using System;
using System.Diagnostics.CodeAnalysis;

#nullable enable

namespace SFA.DAS.ApprenticeAccounts.DTOs.ApprenticeArticle
{
    public class ApprenticeArticleDto
    {
        public Guid Id { get; set; }
        public string? EntryId { get; set; }
        public bool? IsSaved { get; set; }
        public bool? LikeStatus { get; set; }

        [return: NotNullIfNotNull("source")]
        public static ApprenticeArticleDto? Create(Data.Models.ApprenticeArticle source)
        {
            if (source == null) return null;

            var apprenticeArticleDto = new ApprenticeArticleDto
            {
                Id = source.Id,
                EntryId = source.EntryId,
                IsSaved = source.IsSaved,
                LikeStatus = source.LikeStatus,
            };

            return apprenticeArticleDto;
        }
    }
}
