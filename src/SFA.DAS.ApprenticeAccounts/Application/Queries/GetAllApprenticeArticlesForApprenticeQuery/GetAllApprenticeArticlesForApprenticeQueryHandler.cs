using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticeArticle.GetAllApprenticeArticlesForApprentice;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.GetAllApprenticeArticlesForApprenticeQuery
{
    public class GetAllApprenticeArticlesForApprenticeQueryHandler : IRequestHandler<GetAllApprenticeArticlesForApprenticeQuery, GetAllApprenticeArticlesForApprenticeDto>
    {
        private readonly IApprenticeArticleContext _apprenticeArticleContext;

        public GetAllApprenticeArticlesForApprenticeQueryHandler(IApprenticeArticleContext apprenticeArticleContext)
        {
            _apprenticeArticleContext = apprenticeArticleContext;
        }

        public async Task<GetAllApprenticeArticlesForApprenticeDto> Handle(GetAllApprenticeArticlesForApprenticeQuery request, CancellationToken cancellationToken)
        {
            var result = await _apprenticeArticleContext.GetAllArticlesByUserId(request.Id);
            return result.MapToApprenticeArticleDto();
        }
    }
}
