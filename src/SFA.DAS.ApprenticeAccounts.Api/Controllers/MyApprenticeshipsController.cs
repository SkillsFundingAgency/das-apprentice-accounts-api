using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand;
using System.Threading.Tasks;
using System;
using MediatR;

namespace SFA.DAS.ApprenticeAccounts.Api.Controllers;

[ApiController]
public class MyApprenticeshipsController: ControllerBase
{
    private readonly IMediator _mediator;

    public MyApprenticeshipsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("apprentices/{apprenticeId}/MyApprenticeships")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostMyApprenticeship(Guid apprenticeId, CreateMyApprenticeshipRequest request)
    {
        var command = (CreateMyApprenticeshipCommand)request;

        command.ApprenticeId = apprenticeId;
        await _mediator.Send(command);
        return new NoContentResult();
    }
}
