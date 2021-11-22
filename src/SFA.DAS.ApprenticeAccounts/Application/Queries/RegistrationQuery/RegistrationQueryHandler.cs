using MediatR;
using SFA.DAS.ApprenticeCommitments.Data;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Queries.RegistrationQuery
{
    public class RegistrationQueryHandler : IRequestHandler<RegistrationQuery, RegistrationResponse?>
    {
        private readonly IRegistrationContext _registrations;

        public RegistrationQueryHandler(IRegistrationContext registrations)
            => _registrations = registrations;

        public async Task<RegistrationResponse?> Handle(RegistrationQuery query, CancellationToken cancellationToken)
        {
            var model = await _registrations.Find(query.ApprenticeId);
            return Map(model);
        }

        private RegistrationResponse? Map(Registration? model)
        {
            if (model == null)
            {
                return null;
            }

            return new RegistrationResponse
            {
                RegistrationId = model.RegistrationId,
                DateOfBirth = model.DateOfBirth,
                Email = model.Email.ToString(),
                FirstName = model.FirstName,
                HasViewedVerification = model.FirstViewedOn.HasValue,
                HasCompletedVerification = model.HasBeenCompleted
            };
        }
    }
}