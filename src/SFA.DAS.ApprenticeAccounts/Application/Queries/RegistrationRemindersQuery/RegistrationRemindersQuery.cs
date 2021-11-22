using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApprenticeCommitments.DTOs;

namespace SFA.DAS.ApprenticeCommitments.Application.Queries.RegistrationRemindersQuery
{
    public class RegistrationRemindersQuery : IRequest<RegistrationRemindersResponse>
    {
        public DateTime CutOffDateTime { get; set; }
    }
}
