using MediatR;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using SFA.DAS.ApprenticeAccounts.Infrastructure.Mediator;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.CreateOrUpdateApprenticeAccount;

public class CreateOrUpdateApprenticeAccountCommand : IRequest<ApprenticeDto>, IUnitOfWorkCommand
{
    public required string Email { get; set; }
    public required string GovUkIdentifier { get; set; }
}