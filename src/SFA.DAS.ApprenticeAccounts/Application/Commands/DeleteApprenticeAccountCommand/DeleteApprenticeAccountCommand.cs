using MediatR;
using SFA.DAS.ApprenticeAccounts.Infrastructure.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.DeleteApprenticeAccountCommand;
public class DeleteApprenticeAccountCommand : IRequest<Unit>, IUnitOfWorkCommand
{
    public Guid ApprenticeId { get; set; }
}
