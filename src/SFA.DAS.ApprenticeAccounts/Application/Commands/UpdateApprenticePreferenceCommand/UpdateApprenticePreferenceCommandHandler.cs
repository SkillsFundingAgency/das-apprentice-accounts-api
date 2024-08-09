using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateApprenticePreferenceCommand
{
    public class UpdateApprenticePreferenceCommandHandler : IRequestHandler<UpdateApprenticePreferenceCommand, Unit>
    {
        private readonly IApprenticePreferencesContext _apprenticePreferencesContext;
        private readonly IApprenticeContext _apprenticeContext;
        private readonly IPreferencesContext _preferencesContext;
        private readonly ILogger<UpdateApprenticePreferenceCommandHandler> _logger;

        public UpdateApprenticePreferenceCommandHandler(IApprenticePreferencesContext apprenticePreferencesContext,
            IApprenticeContext apprenticeContext, IPreferencesContext preferencesContext,
            ILogger<UpdateApprenticePreferenceCommandHandler> logger)
        {
            _apprenticePreferencesContext = apprenticePreferencesContext;
            _apprenticeContext = apprenticeContext;
            _preferencesContext = preferencesContext;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateApprenticePreferenceCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Fetch Apprentice record by Id: {request.ApprenticeId}");
            var apprentice = await _apprenticeContext.Entities.FindAsync(request.ApprenticeId);

            _logger.LogInformation($"Fetch Preference record by Id: {request.PreferenceId}");
            var preference = await _preferencesContext.Entities.FindAsync(request.PreferenceId);

            if (apprentice == null)
            {
                _logger.LogError(
                    $"No Apprentice record found. Apprentice Id used: {request.ApprenticeId}");
                throw new InvalidInputException(InvalidInputException.InvalidInputApprentice);
            }

            if (preference == null)
            {
                _logger.LogError(
                    $"No Preference record found. Preference Id used: {request.PreferenceId}");
                throw new InvalidInputException(InvalidInputException.InvalidInputPreference);
            }

            _logger.LogInformation(
                $"Fetch ApprenticePreferences record by Apprentice Id and Preference Id. Apprentice Id used: {request.ApprenticeId}, Preference Id used: {request.PreferenceId}");
            var record =
                await _apprenticePreferencesContext.GetApprenticePreferenceForApprenticeAndPreference(
                    request.ApprenticeId, request.PreferenceId);

            if (record == null)
            {
                await _apprenticePreferencesContext.AddAsync(new ApprenticePreferences(request.ApprenticeId,
                    request.PreferenceId,
                    request.Status, DateTime.Now, DateTime.Now), CancellationToken.None);
                _logger.LogDebug(
                    $"Successfully created a new ApprenticePreferences record with Apprentice Id: {request.ApprenticeId}, and Preference Id: {request.PreferenceId}");
            }
            else
            {
                record.Status = request.Status;
                record.UpdatedOn = DateTime.Now;

                _logger.LogDebug(
                    $"Successfully updated an ApprenticePreferences record with Apprentice Id: {request.ApprenticeId}, and Preference Id: {request.PreferenceId}");
            }

            return await Unit.Task;
        }
    }
}