using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.DeleteApprenticeAccountCommand;
public class DeleteApprenticeAccountCommandHandler : IRequestHandler<DeleteApprenticeAccountCommand, Unit>
{
    private readonly IApprenticeContext _apprentices;

    public DeleteApprenticeAccountCommandHandler(IApprenticeContext apprentices)
    {
        _apprentices = apprentices;
    }

    public async Task<Unit> Handle(DeleteApprenticeAccountCommand request, CancellationToken cancellationToken)
    {
        var apprenticeAccount = await _apprentices.Find(request.ApprenticeId);

        if (apprenticeAccount != null)
        {
            await _apprentices.DeleteById(apprenticeAccount.Id);
        }

        return await Unit.Task;
    }
}
