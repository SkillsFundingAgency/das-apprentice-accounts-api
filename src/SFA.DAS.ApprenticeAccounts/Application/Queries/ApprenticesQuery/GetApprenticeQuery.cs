using MediatR;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.ApprenticesQuery
{
    public class GetApprenticeQuery : IRequest<ApprenticeDto?>
    {
        public GetApprenticeQuery(string Id) => ApprenticeId = Id;

        public string ApprenticeId { get; set; }
    }
}