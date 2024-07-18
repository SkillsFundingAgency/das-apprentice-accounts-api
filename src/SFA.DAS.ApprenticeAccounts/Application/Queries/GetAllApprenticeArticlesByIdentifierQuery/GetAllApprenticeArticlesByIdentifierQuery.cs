using MediatR;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticeArticle.GetAllApprenticeArticlesForApprentice;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.GetAllApprenticeArticlesByIdentifier
{
    public class GetAllApprenticeArticlesByIdentifierQuery : IRequest<GetAllApprenticeArticlesForApprenticeDto>
    {
        public Guid Id { get; set; }
        public string EntryId { get; set; }
    }
}