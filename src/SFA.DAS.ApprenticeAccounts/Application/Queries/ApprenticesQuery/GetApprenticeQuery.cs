using MediatR;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.ApprenticesQuery
{
    public class GetApprenticeQuery : IRequest<ApprenticeDto?>
    {
        public GetApprenticeQuery(Guid Id) => ApprenticeId = Id;

        public Guid ApprenticeId { get; set; }
    }
}