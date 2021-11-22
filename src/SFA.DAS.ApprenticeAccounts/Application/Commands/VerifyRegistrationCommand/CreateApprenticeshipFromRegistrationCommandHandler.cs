using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.ApprenticeCommitments.Data;
using SFA.DAS.ApprenticeCommitments.Data.FuzzyMatching;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApprenticeshipFromRegistrationCommand
{
    public class CreateApprenticeshipFromRegistrationCommandHandler : IRequestHandler<CreateApprenticeshipFromRegistrationCommand>
    {
        private readonly IRegistrationContext _registrations;
        private readonly IApprenticeContext _apprentices;
        private readonly ApplicationSettings _applicationSettings;
        private readonly ILogger<CreateApprenticeshipFromRegistrationCommandHandler> _logger;

        public CreateApprenticeshipFromRegistrationCommandHandler(
            IRegistrationContext registrations,
            IApprenticeContext apprenticeRepository,
            ApplicationSettings applicationSettings,
            ILogger<CreateApprenticeshipFromRegistrationCommandHandler> logger)
        {
            _registrations = registrations;
            _apprentices = apprenticeRepository;
            _applicationSettings = applicationSettings;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateApprenticeshipFromRegistrationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Create apprenticeship for apprentice {request.ApprenticeId} from registration {request.RegistrationId}");

            var registration = await _registrations.GetById(request.RegistrationId);
            var apprentice = await _apprentices.GetById(request.ApprenticeId);

            var matcher = new FuzzyMatcher(_applicationSettings.FuzzyMatchingSimilarityThreshold); 

            registration.AssociateWithApprentice(apprentice, matcher);

            return Unit.Value;
        }
    }
}