using MediatR;
using SFA.DAS.ApprenticeAccounts.DTOs;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.ApprenticesQuery
{
    public class ApprenticesQuery : IRequest<ApprenticeDto?>
    {
        public ApprenticesQuery(Guid Id) => ApprenticeId = Id;

        public Guid ApprenticeId { get; set; }
    }
}