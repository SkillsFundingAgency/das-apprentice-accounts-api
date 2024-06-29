using MediatR;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticeArticle.GetAllApprenticeArticlesForApprentice;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.GetAllApprenticeArticlesForApprenticeQuery
{
    public class GetAllApprenticeArticlesForApprenticeQuery : IRequest<GetAllApprenticeArticlesForApprenticeDto>
    {
        public Guid Id { get; set; }
    }
}
