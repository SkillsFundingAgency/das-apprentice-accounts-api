using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateAllApprenticePreferencesCommand
{
    public class UpdateApprenticePreferencesCommandHandler : IRequestHandler<UpdateAllApprenticePreferencesCommand>
    {
        private readonly IApprenticePreferencesContext _apprenticePreferencesContext;
        private readonly IApprenticeContext _apprenticeContext;
        private readonly IPreferencesContext _preferencesContext;

        public UpdateApprenticePreferencesCommandHandler(IApprenticePreferencesContext apprenticePreferencesContext,
            IApprenticeContext apprenticeContext, IPreferencesContext preferencesContext)
        {
            _apprenticePreferencesContext = apprenticePreferencesContext;
            _apprenticeContext = apprenticeContext;
            _preferencesContext = preferencesContext;
        }

        public async Task<Unit> Handle(UpdateAllApprenticePreferencesCommand request, CancellationToken cancellationToken)
        {
            foreach (var apprenticePreference in request.ApprenticePreferences)
            {
                var apprentice = await _apprenticeContext.Entities.FindAsync(apprenticePreference.ApprenticeId);
                var preference = await _preferencesContext.Entities.FindAsync(apprenticePreference.PreferenceId);

                if (apprentice == null || preference == null)
                {
                    throw new InvalidOperationException();
                }

                var record =
                    await _apprenticePreferencesContext.GetApprenticePreferenceForApprenticeAndPreference(
                        apprenticePreference.ApprenticeId, apprenticePreference.PreferenceId);

                if (record == null)
                {
                    await _apprenticePreferencesContext.AddAsync(new ApprenticePreferences(
                        apprenticePreference.ApprenticeId, apprenticePreference.PreferenceId,
                        apprenticePreference.Status, DateTime.Now, DateTime.Now));
                }
                else
                {
                    record.Status = apprenticePreference.Status;
                    record.UpdatedOn = DateTime.Now;
                    
                    _apprenticePreferencesContext.Update(record);
                }
            }

            return await Unit.Task;
        }
    }
}