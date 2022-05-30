using MediatR;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateApprenticePreferencesCommand
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

        public Task<Unit> Handle(UpdateApprenticePreferenceCommand request, CancellationToken cancellationToken)
        {
            var apprentice = _apprenticeContext.Entities.Find(request.ApprenticeId);

            var preference = _preferencesContext.Entities.Find(request.PreferenceId);

            if (apprentice == null || preference == null)
            {
                throw new InvalidOperationException();
            }

            var record =
                _apprenticePreferencesContext.GetSinglePreferenceValueAsync(request.ApprenticeId, request.PreferenceId);

            if (record.Result == null)
            {
                _apprenticePreferencesContext.Add(new ApprenticePreferences(request.ApprenticeId, request.PreferenceId,
                    request.Status, DateTime.Now, DateTime.Now));
            }
            else
            {
                record.Result.Status = request.Status;
                record.Result.UpdatedOn = DateTime.Now;

                _apprenticePreferencesContext.Update(record.Result);
            }

            return Unit.Task;
        }
    }
}
