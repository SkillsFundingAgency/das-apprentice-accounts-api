using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Application.Commands.ConfirmCommand;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using System;
using System.Threading.Tasks;
using SFA.DAS.ApprenticeCommitments.Application.Commands.UpdateRevisionCommand;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    [ApiController]
    public class RevisionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RevisionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPatch("apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/revisions/{revisionId}")]
        public async Task PatchRevision(
            Guid apprenticeId,
            long apprenticeshipId,
            long revisionId,
            [FromBody] JsonPatchDocument<Revision> changes)
        {
            await _mediator.Send(new UpdateRevisionCommand(apprenticeId, apprenticeshipId, revisionId, changes));
        }

        [HttpPatch("apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/revisions/{revisionId}/confirmations")]
        public async Task ConfirmApprenticeship(
            Guid apprenticeId,
            long apprenticeshipId,
            long revisionId,
            [FromBody] Confirmations confirmations)
        {
            await _mediator.Send(new ConfirmCommand(apprenticeId, apprenticeshipId, revisionId, confirmations));
        }
    }
}