using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticeArticle.GetAllApprenticeArticlesForApprentice;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.GetAllApprenticeArticlesByIdentifier
{
    public class GetAllApprenticeArticlesByIdentifierQueryHandler : IRequestHandler<GetAllApprenticeArticlesByIdentifierQuery, GetAllApprenticeArticlesForApprenticeDto>
    {
        private readonly IApprenticeArticleContext _apprenticeArticleContext;

        public GetAllApprenticeArticlesByIdentifierQueryHandler(IApprenticeArticleContext apprenticeArticleContext)
        {
            _apprenticeArticleContext = apprenticeArticleContext;
        }

        public async Task<GetAllApprenticeArticlesForApprenticeDto> Handle(GetAllApprenticeArticlesByIdentifierQuery request, CancellationToken cancellationToken)
        {
            var result = await _apprenticeArticleContext.GetArticlesByUserIdAndEntryId(request.Id, request.EntryId);
            return result.MapToApprenticeArticleDto();
        }
    }
}
