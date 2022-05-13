using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAccounts.Application.Queries.ApprenticePreferencesQuery;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Api.Controllers
{
    [ApiController]
    public class ApprenticePreferencesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApprenticePreferencesController(IMediator mediator)
        {
            _mediator = mediator;
        }

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
    }
}
