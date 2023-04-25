using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand;
using SFA.DAS.ApprenticeAccounts.Application.Queries.MyApprenticeshipQuery;
using System.Threading.Tasks;
using System;
using MediatR;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateMyApprenticeCommand;
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
        var result = await _mediator.Send(command);
        var uri = new Uri($"apprentices/{id}/MyApprenticeship",UriKind.Relative);

        return new CreatedResult(uri,result);
    }

    [HttpGet("apprentices/{id}/MyApprenticeship")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyApprenticeship(Guid id, int? apprenticeshipId)
    {
        var result = await _mediator.Send(new MyApprenticeshipQuery(id, apprenticeshipId));
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Route("apprentices/{id}/MyApprenticeship/{myApprenticeshipId}")]
    public async Task<IActionResult> UpdateMyApprenticeship(Guid id, Guid myApprenticeshipId, [FromBody] UpdateMyApprenticeshipRequest request)
   {
        var command = (UpdateMyApprenticeshipCommand)request;
        command.ApprenticeId = id;
        command.MyApprenticeshipId = myApprenticeshipId;

        await _mediator.Send(command);
        return NoContent();
   }
}
