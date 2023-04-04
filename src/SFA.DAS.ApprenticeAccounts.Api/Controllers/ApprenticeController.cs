using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAccounts.Application.Commands.CreateApprenticeAccountCommand;
using SFA.DAS.ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateApprenticeCommand;
using SFA.DAS.ApprenticeAccounts.Application.Queries.ApprenticesQuery;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Api.Controllers
{
    [ApiController]
    public class ApprenticesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApprenticesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("apprentices/{id}")]
        public async Task<IActionResult> GetApprentice(Guid id)
        {
            var result = await _mediator.Send(new ApprenticesQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("apprentices")]
        public async Task PostApprentice(CreateApprenticeAccountCommand command)
            => await _mediator.Send(command);

        [HttpPatch("apprentices/{id}")]
        public async Task UpdateApprentice(Guid id, JsonPatchDocument<ApprenticePatchDto> changes)
            => await _mediator.Send(new UpdateApprenticeCommand(id, changes));

        [HttpPost("apprentices/{id}/MyApprenticeships")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PostMyApprenticeship(Guid id, CreateMyApprenticeshipRequest command)
        {
            var request = (CreateMyApprenticeshipCommand)command;

            request.ApprenticeId = id;
            await _mediator.Send(request);
            return new NoContentResult();
        }
    }
}