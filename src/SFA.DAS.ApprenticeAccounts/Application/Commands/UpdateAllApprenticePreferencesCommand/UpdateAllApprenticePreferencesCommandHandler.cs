using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateAllApprenticePreferencesCommand
{
    public class UpdateAllApprenticePreferencesCommandHandler : IRequestHandler<UpdateAllApprenticePreferencesCommand>
    {
        private readonly IApprenticePreferencesContext _apprenticePreferencesContext;
        private readonly IApprenticeContext _apprenticeContext;
        private readonly IPreferencesContext _preferencesContext;
        private readonly ILogger<UpdateAllApprenticePreferencesCommandHandler> _logger;

        public UpdateAllApprenticePreferencesCommandHandler(IApprenticePreferencesContext apprenticePreferencesContext,
            IApprenticeContext apprenticeContext, IPreferencesContext preferencesContext,
            ILogger<UpdateAllApprenticePreferencesCommandHandler> logger)
        {
            _apprenticePreferencesContext = apprenticePreferencesContext;
            _apprenticeContext = apprenticeContext;
            _preferencesContext = preferencesContext;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateAllApprenticePreferencesCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Fetch Apprentice by Id. Id used: {request.ApprenticePreferences[0].ApprenticeId}");
            var apprenticeId = request.ApprenticePreferences[0].ApprenticeId;
            var apprentice = await _apprenticeContext.Entities.FindAsync(apprenticeId);

            if (!request.ApprenticePreferences.TrueForAll(ap => ap.ApprenticeId == apprenticeId))
            {
                throw new InvalidInputException();
            }


            foreach (var apprenticePreference in request.ApprenticePreferences)
            {
                _logger.LogInformation($"Fetch Preference by Id. Id used: {apprenticePreference.PreferenceId}");
                var preference = await _preferencesContext.Entities.FindAsync(apprenticePreference.PreferenceId);

                if (apprentice == null || preference == null)
                {
                    _logger.LogError(
                        $"No Apprentice record found, or no Preference record found, or neither record found. Apprentice Id used; {apprenticeId}, Preference Id used: {apprenticePreference.PreferenceId}");
                    throw new InvalidInputException(apprenticeId,
                        apprenticePreference.PreferenceId);
                }

                _logger.LogInformation(
                    $"Fetch ApprenticePreferences record by Apprentice Id and Preference Id. Apprentice Id used: {apprenticeId}, Preference Id used: {apprenticePreference.PreferenceId}");
                var record =
                    await _apprenticePreferencesContext.GetApprenticePreferenceForApprenticeAndPreference(
                        apprenticeId, apprenticePreference.PreferenceId);

                if (record == null)
                {
                    await _apprenticePreferencesContext.AddAsync(new ApprenticePreferences(
                        apprenticePreference.ApprenticeId, apprenticePreference.PreferenceId,
                        apprenticePreference.Status, DateTime.Now, DateTime.Now), CancellationToken.None);
                    _logger.LogDebug(
                        $"Successfully created a new ApprenticePreferences record with Apprentice Id: {apprenticeId} and Preference Id: {apprenticePreference.PreferenceId}");
                }
                else
                {
                    record.Status = apprenticePreference.Status;
                    record.UpdatedOn = DateTime.Now;

                    _logger.LogDebug(
                        $"Successfully updated an ApprenticePreferences record with Apprentice Id: {apprenticeId} and Preference Id: {apprenticePreference.PreferenceId}");
                }
            }

            return await Unit.Task;
        }
    }
}