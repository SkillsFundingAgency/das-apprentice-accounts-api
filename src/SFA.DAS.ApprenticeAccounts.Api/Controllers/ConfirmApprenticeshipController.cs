using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Application.Commands.ConfirmCommand;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    public class ConfirmApprenticeshipRequest
    {
        public bool ApprenticeshipCorrect { get; set; }
    }

    public class ConfirmApprenticeshipController : Controller
    {
        private readonly IMediator _mediator;

        public ConfirmApprenticeshipController(IMediator mediator) => _mediator = mediator;

        [HttpPost("apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/revisions/{revisionId}/ApprenticeshipConfirmation")]
        [Obsolete("Use PATCH /apprentices/{id}/apprenticeships/{id}/revisions/{id}/confirmations")]
        public async Task ConfirmTrainingProvider(
            Guid apprenticeId, long apprenticeshipId, long revisionId,
            [FromBody] ConfirmApprenticeshipRequest request)
        {
            await _mediator.Send(new ConfirmCommand(
                apprenticeId, apprenticeshipId, revisionId,
                new Confirmations { ApprenticeshipCorrect = request.ApprenticeshipCorrect }));
        }
    }
}