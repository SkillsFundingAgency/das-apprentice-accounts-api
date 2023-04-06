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
    public async Task<IActionResult> PostMyApprenticeship(Guid apprenticeId, CreateMyApprenticeshipRequest command)
    {
        var request = (CreateMyApprenticeshipCommand)command;

        request.ApprenticeId = apprenticeId;
        await _mediator.Send(request);
        return new NoContentResult();
    }
}
