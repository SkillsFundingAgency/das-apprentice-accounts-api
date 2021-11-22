using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Application.Commands.ConfirmCommand;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    public class ConfirmApprenticeshipDetailsRequest
    {
        public bool ApprenticeshipDetailsCorrect { get; set; }
    }

    [ApiController]
    public class ConfirmApprenticeshipDetailsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConfirmApprenticeshipDetailsController(IMediator mediator) => _mediator = mediator;

        [HttpPost("apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/revisions/{revisionId}/ApprenticeshipDetailsConfirmation")]
        [Obsolete("Use PATCH /apprentices/{id}/apprenticeships/{id}/revisions/{id}/confirmations")]
        public async Task ConfirmTrainingProvider(
            Guid apprenticeId, long apprenticeshipId, long revisionId,
            [FromBody] ConfirmApprenticeshipDetailsRequest request)
        {
            await _mediator.Send(new ConfirmCommand(
                apprenticeId, apprenticeshipId, revisionId,
                new Confirmations { ApprenticeshipDetailsCorrect = request.ApprenticeshipDetailsCorrect }));
        }
    }
}