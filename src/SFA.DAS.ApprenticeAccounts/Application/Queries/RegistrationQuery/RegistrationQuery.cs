using System;
using MediatR;

namespace SFA.DAS.ApprenticeCommitments.Application.Queries.RegistrationQuery
{
    public class RegistrationQuery : IRequest<RegistrationResponse>
    {
        public Guid ApprenticeId { get; set; }
    }
}
