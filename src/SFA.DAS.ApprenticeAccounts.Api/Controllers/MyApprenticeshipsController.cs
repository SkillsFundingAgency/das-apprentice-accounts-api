using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand;
using System.Threading.Tasks;
using System;
using MediatR;
using SFA.DAS.ApprenticeAccounts.Application.Queries.MyApprenticeshipsQuery;

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

    [HttpGet("apprentices/{apprenticeId}/MyApprenticeships")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyApprenticeship(Guid apprenticeId)
    {

        var result = await _mediator.Send(new MyApprenticeshipsQuery(apprenticeId));
        if (result == null) return NotFound();
        return Ok(result);
    }
}
