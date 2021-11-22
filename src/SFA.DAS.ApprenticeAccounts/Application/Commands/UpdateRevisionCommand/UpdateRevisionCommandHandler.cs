using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeCommitments.Data;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.UpdateRevisionCommand
{
    public class UpdateRevisionCommandHandler : IRequestHandler<UpdateRevisionCommand>
    {
        private readonly IRevisionContext _revisions;
        private readonly ILogger<UpdateRevisionCommandHandler> _logger;

        public UpdateRevisionCommandHandler(IRevisionContext revisions, ILogger<UpdateRevisionCommandHandler> logger)
        {
            _revisions = revisions;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateRevisionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Updating revision {request.RevisionId} for apprenticeship {request.ApprenticeshipId} on apprentice {request.ApprenticeId} - {JsonConvert.SerializeObject(request.Updates)}");
            var app = await _revisions.GetById(request.ApprenticeId, request.ApprenticeshipId, request.RevisionId);
            request.Updates.ApplyTo(app);
            return Unit.Value;
        }
    }
}