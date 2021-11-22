using MediatR;
using SFA.DAS.ApprenticeCommitments.DTOs;
using System;

namespace SFA.DAS.ApprenticeCommitments.Application.Queries.ApprenticesQuery
{
    public class ApprenticesQuery : IRequest<ApprenticeDto?>
    {
        public ApprenticesQuery(Guid Id) => ApprenticeId = Id;

        public Guid ApprenticeId { get; set; }
    }
}