using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeCommitments.Data;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using SFA.DAS.ApprenticeCommitments.Infrastructure.Mediator;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.UpdateApprenticeshipCommand
{
    public class UpdateApprenticeshipCommand : IUnitOfWorkCommand
    {
        public UpdateApprenticeshipCommand(Guid apprenticeId, long apprenticeshipId, JsonPatchDocument<Apprenticeship> updates)
        {
            ApprenticeId = apprenticeId;
            ApprenticeshipId = apprenticeshipId;
            Updates = updates;
        }

        public Guid ApprenticeId { get; }
        public long ApprenticeshipId { get; }
        public JsonPatchDocument<Apprenticeship> Updates { get; }
    }

    public class UpdateApprenticeshipCommandHandler : IRequestHandler<UpdateApprenticeshipCommand>
    {
        private readonly IApprenticeshipContext _apprenticeships;
        private readonly ILogger<UpdateApprenticeshipCommandHandler> _logger;

        public UpdateApprenticeshipCommandHandler(IApprenticeshipContext apprenticeships, ILogger<UpdateApprenticeshipCommandHandler> logger)
        {
            _apprenticeships = apprenticeships;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateApprenticeshipCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Updating {request.ApprenticeId}/{request.ApprenticeshipId} - {JsonConvert.SerializeObject(request.Updates)}");
            var app = await _apprenticeships.GetById(request.ApprenticeId, request.ApprenticeshipId);
            request.Updates.ApplyTo(app);
            return Unit.Value;
        }
    }
}