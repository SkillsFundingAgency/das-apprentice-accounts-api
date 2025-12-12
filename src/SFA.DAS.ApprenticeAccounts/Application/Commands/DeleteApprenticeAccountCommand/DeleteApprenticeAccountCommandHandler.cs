using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.DeleteApprenticeAccountCommand;
public class DeleteApprenticeAccountCommandHandler : IRequestHandler<DeleteApprenticeAccountCommand, DeleteApprenticeAccountResponse>
{
    private readonly IApprenticeContext _apprentices;
    private readonly IMyApprenticeshipContext _myApprenticeship;

    public DeleteApprenticeAccountCommandHandler(IApprenticeContext apprentices, IMyApprenticeshipContext myApprenticeship)
    {
        _apprentices = apprentices;
        _myApprenticeship = myApprenticeship;
    }

    public async Task<DeleteApprenticeAccountResponse> Handle(DeleteApprenticeAccountCommand request, CancellationToken cancellationToken)
    {
        var apprenticeAccount = await _apprentices.Find(request.ApprenticeId);
        var myApprenticeship = await _myApprenticeship.FindByApprenticeId(request.ApprenticeId);

        if (myApprenticeship != null)
        {
            return new DeleteApprenticeAccountResponse
            {
                Success = false,
                Message = "Apprentice Account is already tied to an Apprenticeship"
            };
        }
      
        if (apprenticeAccount != null)
        {
            await _apprentices.DeleteById(apprenticeAccount.Id);
        }

        return new DeleteApprenticeAccountResponse
        {
            Success = true,
            Message = "Apprentice Account deleted Successfully"
        };
    }
}
