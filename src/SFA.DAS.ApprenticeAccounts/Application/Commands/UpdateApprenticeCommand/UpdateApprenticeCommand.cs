using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using SFA.DAS.ApprenticeAccounts.Infrastructure.Mediator;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateApprenticeCommand
{
    public class UpdateApprenticeCommand : IRequest<bool>, IUnitOfWorkCommand
    {
        public UpdateApprenticeCommand(Guid apprenticeId, JsonPatchDocument<ApprenticePatchDto> updates)
        {
            ApprenticeId = apprenticeId;
            Updates = updates;
        }

        public Guid ApprenticeId { get; }
        public JsonPatchDocument<ApprenticePatchDto> Updates { get; }
    }
}