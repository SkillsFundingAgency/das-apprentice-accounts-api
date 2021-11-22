using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeCommitments.Data;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.ChangeRegistrationCommand
{
    public class ChangeRegistrationCommandHandler : IRequestHandler<ChangeRegistrationCommand>
    {
        private readonly IApprenticeshipContext _apprenticeships;
        private readonly IRegistrationContext _registrations;
        private readonly ILogger<ChangeRegistrationCommandHandler> _logger;

        public ChangeRegistrationCommandHandler(IApprenticeshipContext apprenticeships, IRegistrationContext registrations, ILogger<ChangeRegistrationCommandHandler> logger)
        {
            _apprenticeships = apprenticeships;
            _registrations = registrations;
            _logger = logger;
        }

        public async Task<Unit> Handle(ChangeRegistrationCommand command, CancellationToken cancellationToken)
        {
            var apprenticeshipId = command.CommitmentsContinuedApprenticeshipId ?? command.CommitmentsApprenticeshipId;

            var apprenticeship = await _apprenticeships.FindByCommitmentsApprenticeshipId(apprenticeshipId);

            if (apprenticeship == null)
            {
                _logger.LogWarning("No confirmed apprenticeship {apprenticeshipId} found", apprenticeshipId);
                await UpdateOrCreateRegistration(command, apprenticeshipId);
            }
            else
            {
                _logger.LogInformation("Updating apprenticeship {apprenticeshipId}", apprenticeshipId);
                apprenticeship.Revise(command.CommitmentsApprenticeshipId, BuildApprenticeshipDetails(command), command.CommitmentsApprovedOn);
            }

            return Unit.Value;
        }

        private async Task UpdateOrCreateRegistration(ChangeRegistrationCommand command, long apprenticeshipId)
        {
            var registration = await _registrations.FindByCommitmentsApprenticeshipId(apprenticeshipId);

            if (registration == null)
            {
                _logger.LogInformation("Adding registration for apprenticeship {apprenticeshipId} because it didn't exist", apprenticeshipId);
                await _registrations.AddAsync(
                    new Registration(Guid.NewGuid(), command.CommitmentsApprenticeshipId,
                        command.CommitmentsApprovedOn,
                        BuildPersonalDetails(command),
                        BuildApprenticeshipDetails(command)));
            }
            else
            {
                _logger.LogInformation("Updating registration for apprenticeship {apprenticeshipId}", apprenticeshipId);
                registration.RenewApprenticeship(command.CommitmentsApprenticeshipId, command.CommitmentsApprovedOn,
                    BuildApprenticeshipDetails(command), BuildPersonalDetails(command));
            }
        }

        private static PersonalInformation BuildPersonalDetails(ChangeRegistrationCommand command)
            => new PersonalInformation(
                command.FirstName,
                command.LastName,
                command.DateOfBirth,
                new MailAddress(command.Email));

        private static ApprenticeshipDetails BuildApprenticeshipDetails(ChangeRegistrationCommand command)
        {
            var details = new ApprenticeshipDetails(
                command.EmployerAccountLegalEntityId,
                command.EmployerName,
                command.TrainingProviderId,
                command.TrainingProviderName,
                new CourseDetails(
                    command.CourseName,
                    command.CourseLevel,
                    command.CourseOption,
                    command.PlannedStartDate,
                    command.PlannedEndDate,
                    command.CourseDuration));
            return details;
        }
    }
}