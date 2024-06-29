using Microsoft.Azure.Amqp.Framing;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticeArticle;
using System.Diagnostics.CodeAnalysis;

#nullable enable

namespace SFA.DAS.ApprenticeAccounts.DTOs.ApprenticeArticle
{
    public static class ApprenticeArticleDtoMapping
    {
        [return: NotNullIfNotNull("apprenticeArticle")]
        public static ApprenticeArticleDto? MapToApprenticeArticleDto(this Data.Models.ApprenticeArticle? apprenticeArticle)
        {
            if (apprenticeArticle == null) return null;

            return new ApprenticeArticleDto
            {
                Id = apprenticeArticle.Id,
                EntryId = apprenticeArticle.EntryId,
                IsSaved = apprenticeArticle.IsSaved,
                LikeStatus = apprenticeArticle.LikeStatus,
            };
        }
    }
}