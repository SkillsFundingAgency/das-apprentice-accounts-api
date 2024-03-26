using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAccounts.Application.Commands.CreateApprenticeAccountCommand;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateApprenticeCommand;
using SFA.DAS.ApprenticeAccounts.Application.Queries.ApprenticesQuery;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using System;
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
            var result = await _mediator.Send(new GetApprenticeQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("apprentices")]
        public async Task PostApprentice(CreateApprenticeAccountCommand command)
            => await _mediator.Send(command);

        [HttpPatch("apprentices/{id}")]
        public async Task<IActionResult> UpdateApprentice(Guid id, JsonPatchDocument<ApprenticePatchDto> changes)
        {
            var result = await _mediator.Send(new UpdateApprenticeCommand(id, changes));
            if (result == false) return NotFound();
            return Ok(result);
        }

        [HttpPost("apprentices/sync")]
        public async Task<IActionResult> SyncApprentices(Guid[] apprenticeIds, DateTime? updatedSinceDate)
        {
            var result = await _mediator.Send(new GetApprenticesQuery(updatedSinceDate, apprenticeIds));
            return Ok(result);
        }
    }
}
