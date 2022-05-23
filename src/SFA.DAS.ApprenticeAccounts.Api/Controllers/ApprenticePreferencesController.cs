using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateApprenticePreferencesCommand;
using SFA.DAS.ApprenticeAccounts.Application.Queries.GetApprenticePreferencesByApprenticeIdQuery;
using SFA.DAS.ApprenticeAccounts.Application.Queries.GetSingleApprenticePreferenceValueByIdsQuery;
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
        public async Task<IActionResult> GetApprenticePreferencesById(Guid apprenticeId)
        {
            var result = await _mediator.Send(new GetApprenticePreferencesByIdQuery() { ApprenticeId = apprenticeId });
            return Ok(result.ApprenticePreferences);
        }

        [HttpGet("apprenticepreferences/{apprenticeId}/{preferenceId}")]
        public async Task<IActionResult> GetSingleApprenticePreferenceValue(Guid apprenticeId, int preferenceId)
        {
            var result = await _mediator.Send(new GetSingleApprenticePreferenceValueQuery()
            {
                ApprenticeId = apprenticeId, PreferenceId = preferenceId
            });

            if (result.PreferenceId == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }


        [HttpPost("apprenticepreferences/{apprenticeId}/{preferenceId}/{status}")]
        public async Task<IActionResult> UpdateApprenticePreferences(Guid apprenticeId, int preferenceId, bool status)
        {
            try
            {
                var result = await _mediator.Send(new UpdateApprenticePreferencesCommand
                {
                    ApprenticeId = apprenticeId, PreferenceId = preferenceId, Status = status
                });


                return Ok();
            }
            catch (Exception e)
            {
                if (e is InvalidOperationException)
                {
                    return NotFound();
                }
                return BadRequest();
            }
        }
    }
}
