using MediatR;
using SFA.DAS.ApprenticeAccounts.Infrastructure.Mediator;
using System;

#nullable disable

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.ChangeEmailAddressCommand
{
    public class ChangeEmailAddressCommand : IRequest<Unit>, IUnitOfWorkCommand
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