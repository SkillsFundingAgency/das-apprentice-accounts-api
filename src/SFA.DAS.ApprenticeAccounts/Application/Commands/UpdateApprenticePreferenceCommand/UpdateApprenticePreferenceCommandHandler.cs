using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateApprenticePreferenceCommand
{
    public class UpdateApprenticePreferenceCommandHandler : IRequestHandler<UpdateApprenticePreferenceCommand>
    {
        private readonly IApprenticePreferencesContext _apprenticePreferencesContext;
        private readonly IApprenticeContext _apprenticeContext;
        private readonly IPreferencesContext _preferencesContext;

        public UpdateApprenticePreferenceCommandHandler(IApprenticePreferencesContext apprenticePreferencesContext,
            IApprenticeContext apprenticeContext, IPreferencesContext preferencesContext)
        {
            _apprenticePreferencesContext = apprenticePreferencesContext;
            _apprenticeContext = apprenticeContext;
            _preferencesContext = preferencesContext;
        }

        public async Task<Unit> Handle(UpdateApprenticePreferenceCommand request, CancellationToken cancellationToken)
        {
            var apprentice = await _apprenticeContext.Entities.FindAsync(request.ApprenticeId);

            var preference = await _preferencesContext.Entities.FindAsync(request.PreferenceId);

            if (apprentice == null || preference == null)
            {
                throw new InvalidOperationException();
            }

            var record =
                await _apprenticePreferencesContext.GetApprenticePreferenceForApprenticeAndPreference(
                    request.ApprenticeId, request.PreferenceId);

            if (record == null)
            {
                await _apprenticePreferencesContext.AddAsync(new ApprenticePreferences(request.ApprenticeId,
                    request.PreferenceId,
                    request.Status, DateTime.Now, DateTime.Now));
            }
            else
            {
                record.Status = request.Status;
                record.UpdatedOn = DateTime.Now;

                _apprenticePreferencesContext.Update(record);
            }

            return await Unit.Task;
        }
    }
}