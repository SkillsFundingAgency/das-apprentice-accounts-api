using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateMyApprenticeshipCommand;
public class UpdateMyApprenticeshipCommandHandler : IRequestHandler<UpdateMyApprenticeshipCommand, Unit>
{
    private readonly IMyApprenticeshipContext _myApprenticeships;

    public UpdateMyApprenticeshipCommandHandler(IMyApprenticeshipContext myApprenticeships)
    {
        _myApprenticeships = myApprenticeships;
    }

    public async Task<Unit> Handle(UpdateMyApprenticeshipCommand command, CancellationToken cancellationToken)
    {
        var myApprenticeship = await _myApprenticeships.FindByApprenticeId(command.ApprenticeId);

        if (myApprenticeship == null)
        {
            return Unit.Value;
        }

        myApprenticeship.UpdateMyApprenticeship(command);
        _myApprenticeships.Update(myApprenticeship);

        return Unit.Value;
    }
}