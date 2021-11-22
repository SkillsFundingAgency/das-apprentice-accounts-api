using SFA.DAS.ApprenticeCommitments.Infrastructure.Mediator;
using System;

#nullable disable

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.CreateRegistrationCommand
{
    public class ChangeEmailAddressCommand : IUnitOfWorkCommand
    {
        public ChangeEmailAddressCommand(Guid apprenticeId, string email)
        {
            ApprenticeId = apprenticeId;
            Email = email;
        }

        public Guid ApprenticeId { get; }
        public string Email { get; }
    }
}