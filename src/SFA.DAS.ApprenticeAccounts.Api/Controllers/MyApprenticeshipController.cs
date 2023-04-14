﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand;
using System.Threading.Tasks;
using System;
using MediatR;
using SFA.DAS.ApprenticeAccounts.Application.Queries.MyApprenticeshipsQuery;

namespace SFA.DAS.ApprenticeAccounts.Api.Controllers;

[ApiController]
public class MyApprenticeshipController : ControllerBase
{
    private readonly IMediator _mediator;

    public MyApprenticeshipController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("apprentices/{apprenticeId}/MyApprenticeship")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostMyApprenticeship(Guid apprenticeId, CreateMyApprenticeshipRequest request)
    {
        var command = (CreateMyApprenticeshipCommand)request;
        command.ApprenticeId = apprenticeId;
        var result = await _mediator.Send(command);
        var uri = new Uri($"apprentices/{apprenticeId}/MyApprenticeship",UriKind.Relative);

        return new CreatedResult(uri,result);
    }

    [HttpGet("apprentices/{apprenticeId}/MyApprenticeship")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyApprenticeship(Guid apprenticeId)
    {
        var result = await _mediator.Send(new MyApprenticeshipQuery(apprenticeId));
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}
