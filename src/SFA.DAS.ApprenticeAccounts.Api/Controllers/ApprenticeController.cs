using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAccounts.Application.Commands.CreateApprenticeAccountCommand;
using SFA.DAS.ApprenticeAccounts.Application.Commands.CreateRegistrationCommand;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateApprenticeCommand;
using SFA.DAS.ApprenticeAccounts.Application.Queries.ApprenticesQuery;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Api.Controllers
{
    public class ChangeEmailAddressRequest
    {
        public string Email { get; set; }
    }

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
        public async Task UpdateApprentice(Guid id, JsonPatchDocument<Apprentice> changes)
            => await _mediator.Send(new UpdateApprenticeCommand(id, changes));

        [HttpPost("apprentices/{id}/email")]
        [Obsolete("Use PATCH /apprentices/{id}")]
        public async Task<IActionResult> CreateRegistration(Guid id, ChangeEmailAddressRequest request)
        {
            await _mediator.Send(new ChangeEmailAddressCommand(id, request.Email));
            return Accepted();
        }
    }
}