using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand;
using SFA.DAS.ApprenticeAccounts.Application.Queries.MyApprenticeshipQuery;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateMyApprenticeshipCommand;
using System.Threading.Tasks;
using System;
using MediatR;
using System.Net;


namespace SFA.DAS.ApprenticeAccounts.Api.Controllers;

[ApiController]
public class MyApprenticeshipController : ControllerBase
{
    private readonly IMediator _mediator;

    public MyApprenticeshipController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("apprentices/{id}/MyApprenticeship")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostMyApprenticeship(Guid id, CreateMyApprenticeshipRequest request)
    {
        var command = (CreateMyApprenticeshipCommand)request;
        command.ApprenticeId = id;

        try
        {
            var result = await _mediator.Send(command);
            var uri = new Uri($"apprentices/{id}/MyApprenticeship", UriKind.Relative);
            return new CreatedResult(uri, result);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains(CreateMyApprenticeshipCommandValidator.ApprenticeIdNotPresent)
                || ex.Message.Contains(CreateMyApprenticeshipCommandValidator.ApprenticeIdNotValid))
                return NotFound();

            return BadRequest(ex.Message);
        }
    }

    [HttpGet("apprentices/{id}/MyApprenticeship")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyApprenticeship(Guid id)
    {
        var result = await _mediator.Send(new MyApprenticeshipQuery(id));
        if (result.Apprentice == null)
        {
            return BadRequest();
        }

        if (result.MyApprenticeship == null)
        {
            return NotFound();
        }

        return Ok(result.MyApprenticeship);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Route("apprentices/{id}/MyApprenticeship")]
    public async Task<IActionResult> UpdateMyApprenticeship(Guid id, 
        [FromBody] UpdateMyApprenticeshipRequest request)
    {
        var command = (UpdateMyApprenticeshipCommand)request;
        command.ApprenticeId = id;

        await _mediator.Send(command);
        return NoContent();
    }
}
