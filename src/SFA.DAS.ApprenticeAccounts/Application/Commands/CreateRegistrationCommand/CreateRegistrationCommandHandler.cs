using MediatR;
using SFA.DAS.ApprenticeCommitments.Data;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.CreateRegistrationCommand
{
    public class CreateRegistrationCommandHandler : IRequestHandler<CreateRegistrationCommand>
    {
        private readonly IRegistrationContext _registrations;

        public CreateRegistrationCommandHandler(IRegistrationContext registrations)
            => _registrations = registrations;

        public async Task<Unit> Handle(CreateRegistrationCommand request, CancellationToken cancellationToken)
        {
            await _registrations.AddAsync(new Registration(
                request.RegistrationId,
                request.CommitmentsApprenticeshipId,
                request.CommitmentsApprovedOn,
                new PersonalInformation(
                    request.FirstName,
                    request.LastName,
                    request.DateOfBirth,
                    new MailAddress(request.Email)),
                new ApprenticeshipDetails(
                    request.EmployerAccountLegalEntityId,
                    request.EmployerName,
                    request.TrainingProviderId,
                    request.TrainingProviderName,
                    new CourseDetails(
                        request.CourseName,
                        request.CourseLevel,
                        request.CourseOption,
                        request.PlannedStartDate,
                        request.PlannedEndDate,
                        request.CourseDuration))));

            return Unit.Value;
        }
    }
}