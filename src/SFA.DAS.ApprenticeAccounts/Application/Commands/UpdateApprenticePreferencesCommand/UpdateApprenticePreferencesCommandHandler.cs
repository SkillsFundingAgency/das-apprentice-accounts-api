using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateApprenticePreferencesCommand
{
    public class UpdateApprenticePreferencesCommandHandler : IRequestHandler<UpdateApprenticePreferencesCommand>
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

        public Task<Unit> Handle(UpdateApprenticePreferencesCommand request, CancellationToken cancellationToken)
        {
            foreach (var apprenticePreferences in request.ApprenticePreferences)
            {
                var apprentice = _apprenticeContext.Entities.Find(apprenticePreferences.ApprenticeId);
                var preference = _preferencesContext.Entities.Find(apprenticePreferences.PreferenceId);

                if (apprentice == null || preference == null)
                {
                    throw new InvalidOperationException();
                }

                var record =
                    _apprenticePreferencesContext.GetSinglePreferenceValueAsync(apprenticePreferences.ApprenticeId, apprenticePreferences.PreferenceId);

                if (record.Result == null)
                {
                    _apprenticePreferencesContext.Add(new ApprenticePreferences(apprenticePreferences.ApprenticeId, apprenticePreferences.PreferenceId,
                        apprenticePreferences.Status, DateTime.Now, DateTime.Now));
                }
                else
                {
                    record.Result.Status = apprenticePreferences.Status;
                    record.Result.UpdatedOn = DateTime.Now;

                    _apprenticePreferencesContext.Update(record.Result);
                }
            }
            return Unit.Task;
        }
    }
}