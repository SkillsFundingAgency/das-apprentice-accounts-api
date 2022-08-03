using FluentValidation;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateAllApprenticePreferencesCommand
{
    public class UpdateAllApprenticePreferencesCommandValidator : AbstractValidator<List<ApprenticePreferences>>
    {
        public UpdateAllApprenticePreferencesCommandValidator()
        {
            RuleFor(model => model.Select(m => m.ApprenticeId)).NotNull().NotEmpty();
            RuleFor(model => model.Select(m => m.PreferenceId)).NotNull().NotEmpty();
            RuleFor(model => model.Select(m => m.Status)).NotNull().NotEmpty();
        }
    }
}
