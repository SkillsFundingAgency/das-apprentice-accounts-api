using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Application.Commands.ConfirmCommand;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    public class ConfirmHowApprenticeshipWillBeDeliveredRequest
    {
        public bool HowApprenticeshipDeliveredCorrect { get; set; }
    }

    [ApiController]
    public class ConfirmHowApprenticeshipWillBeDeliveredController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConfirmHowApprenticeshipWillBeDeliveredController(IMediator mediator) => _mediator = mediator;

        [HttpPost("apprentices/{apprenticeId}/apprenticeships/{apprenticeshipId}/revisions/{revisionId}/HowApprenticeshipWillBeDeliveredConfirmation")]
        [Obsolete("Use PATCH /apprentices/{id}/apprenticeships/{id}/revisions/{id}/confirmations")]
        public async Task HowApprenticeshipWillBeDelivered(
            Guid apprenticeId, long apprenticeshipId, long revisionId,
            [FromBody] ConfirmHowApprenticeshipWillBeDeliveredRequest request)
        {
            await _mediator.Send(new ConfirmCommand(
                apprenticeId, apprenticeshipId, revisionId,
                new Confirmations { HowApprenticeshipDeliveredCorrect = request.HowApprenticeshipDeliveredCorrect }));
        }
    }
}