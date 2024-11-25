using MediatR;
using SFA.DAS.ApprenticeAccounts.Infrastructure.Mediator;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.CreateApprenticeAccountCommand
{
    public class CreateApprenticeAccountCommand : IRequest<Unit>, IUnitOfWorkCommand
    {
        public Guid ApprenticeId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? GovUkIdentifier { get; set; }
        public DateTime? AppLastLoggedIn { get; set; }
    }
}