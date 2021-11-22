using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Api.Types;
using SFA.DAS.ApprenticeCommitments.Application.Commands.RegistrationFirstSeenCommand;
using SFA.DAS.ApprenticeCommitments.Application.Commands.RegistrationReminderSentCommand;
using SFA.DAS.ApprenticeCommitments.Application.Commands.StoppedApprenticeshipCommand;
using SFA.DAS.ApprenticeCommitments.Application.Queries.RegistrationQuery;
using SFA.DAS.ApprenticeCommitments.Application.Queries.RegistrationRemindersQuery;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RegistrationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("registrations/{apprenticeId}")]
        public async Task<IActionResult> GetRegistration(Guid apprenticeId)
        {
            var response = await _mediator.Send(new RegistrationQuery { ApprenticeId = apprenticeId });

            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpGet("registrations/reminders")]
        public async Task<IActionResult> GetRegistrationsNeedingReminders(DateTime invitationCutOffTime)
        {
            var response = await _mediator.Send(new RegistrationRemindersQuery { CutOffDateTime = invitationCutOffTime });
            return Ok(response);
        }

        [HttpPost("registrations/{apprenticeId}/reminder")]
        public async Task RegistrationReminderSent(Guid apprenticeId, [FromBody] RegistrationReminderSentRequest request)
            => await _mediator.Send(new RegistrationReminderSentCommand(apprenticeId, request.SentOn));

        [HttpPost("registrations/{apprenticeId}/firstseen")]
        public async Task RegistrationFirstSeen(Guid apprenticeId, [FromBody] RegistrationFirstSeenRequest request)
            => await _mediator.Send(new RegistrationFirstSeenCommand(apprenticeId, request.SeenOn));

        [HttpPost("registrations/stopped")]
        public async Task StoppedApprenticeship([FromBody] StoppedApprenticeshipCommand request)
            => await _mediator.Send(request);
    }
}