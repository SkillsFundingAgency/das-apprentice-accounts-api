using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Application.Commands.ChangeRegistrationCommand;
using SFA.DAS.ApprenticeCommitments.Application.Commands.CreateRegistrationCommand;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    [ApiController]
    public class ApprovalController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApprovalController(IMediator mediator) => _mediator = mediator;

        [HttpPost("approvals")]
        public async Task ApprenticeshipCreated(CreateRegistrationCommand command)
            => await _mediator.Send(command);

        [HttpPut("approvals")]
        public async Task ChangeOfCircumstances([FromBody] ChangeRegistrationCommand request)
            => await _mediator.Send(request);
    }
}