using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAccounts.Application.Queries.PreferencesQuery;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Api.Controllers
{
    [ApiController]
    public class PreferencesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PreferencesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/preferences")]
        public async Task<IActionResult> GetPreferences()
        {
            var result = await _mediator.Send(new GetAllPreferencesQuery());

            return Ok(result.Preferences);

        }
    }
}
