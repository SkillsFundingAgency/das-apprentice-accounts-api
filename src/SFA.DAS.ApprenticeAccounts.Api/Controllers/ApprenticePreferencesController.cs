using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateAllApprenticePreferencesCommand;
using SFA.DAS.ApprenticeAccounts.Application.Queries.GetAllApprenticePreferencesForApprenticeQuery;
using SFA.DAS.ApprenticeAccounts.Application.Queries.GetApprenticePreferenceForApprenticeAndPreferenceQuery;
using SFA.DAS.ApprenticeAccounts.Exceptions;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Api.Controllers
{
    [ApiController]
    public class ApprenticePreferencesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApprenticePreferencesController(IMediator mediator) => _mediator = mediator;

        [HttpGet("apprenticepreferences/{apprenticeId}")]
        public async Task<IActionResult> GetAllApprenticePreferencesForApprentice(Guid apprenticeId)
        {
            var result = await _mediator.Send(new GetAllApprenticePreferencesForApprenticeQuery
            {
                ApprenticeId = apprenticeId
            });
            return Ok(result);
        }

        [HttpGet("apprenticepreferences/{apprenticeId}/{preferenceId}")]
        public async Task<IActionResult> GetApprenticePreferenceForApprenticeAndPreference(Guid apprenticeId,
            int preferenceId)
        {
            var result = await _mediator.Send(new GetApprenticePreferenceForApprenticeAndPreferenceQuery
            {
                ApprenticeId = apprenticeId,
                PreferenceId = preferenceId
            });

            if (result.PreferenceId == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }


        [HttpPost("apprenticepreferences/{apprenticeId}/{preferenceId}/{status}")]
        public async Task<IActionResult> UpdateApprenticePreference(Guid apprenticeId, int preferenceId, bool status)
        {
            try
            {
                var result = await _mediator.Send(new Application.Commands.UpdateApprenticePreferenceCommand.UpdateApprenticePreferenceCommand
                {
                    ApprenticeId = apprenticeId,
                    PreferenceId = preferenceId,
                    Status = status
                });


                return Ok();
            }
            catch (InvalidInputException iie)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("apprenticepreferences/{apprenticeId}")]
        public async Task<IActionResult> UpdateAllApprenticePreferences(
                [FromBody] UpdateAllApprenticePreferencesCommand command, Guid apprenticeId)
        {
            try
            {
                command.ApprenticeId = apprenticeId;
                var result = await _mediator.Send(command);
                return Ok();
            }
            catch (InvalidInputException iie)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
