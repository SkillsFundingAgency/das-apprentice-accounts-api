using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApprenticeAccounts.DTOs.ApprenticeArticle.GetAllApprenticeArticlesForApprentice
{
    public static class GetAllApprenticeArticlesForApprenticeMapping
    {
        public static GetAllApprenticeArticlesForApprenticeDto MapToApprenticeArticleDto(
            this IEnumerable<Data.Models.ApprenticeArticle> apprenticeArticle)
        {
            var apprenticeArticlesDto = new GetAllApprenticeArticlesForApprenticeDto
            {
                ApprenticeArticles = new List<ApprenticeArticleDto>(apprenticeArticle.Select(ap =>
                    new ApprenticeArticleDto()
                    {
                        LikeStatus = ap.LikeStatus,
                        EntryId = ap.EntryId,
                        IsSaved = ap.IsSaved,
                        SaveTime = ap.SaveTime
                    }))
            };

            return apprenticeArticlesDto;
        }
    }
}