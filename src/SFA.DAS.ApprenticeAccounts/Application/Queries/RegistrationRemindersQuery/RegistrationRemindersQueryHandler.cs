using MediatR;
using SFA.DAS.ApprenticeCommitments.Data;
using SFA.DAS.ApprenticeCommitments.DTOs;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Queries.RegistrationRemindersQuery
{
    public class RegistrationRemindersQueryHandler : IRequestHandler<RegistrationRemindersQuery, RegistrationRemindersResponse>
    {
        private readonly IRegistrationContext _registrations;

        public RegistrationRemindersQueryHandler(IRegistrationContext registrations)
            => _registrations = registrations;

        public async Task<RegistrationRemindersResponse> Handle(RegistrationRemindersQuery request, CancellationToken cancellationToken)
        {
            var reminders = await _registrations.RegistrationsNeedingSignUpReminders(request.CutOffDateTime);
            return new RegistrationRemindersResponse(reminders.Select(x => x.MapToRegistrationDto()));
        }
    }
}