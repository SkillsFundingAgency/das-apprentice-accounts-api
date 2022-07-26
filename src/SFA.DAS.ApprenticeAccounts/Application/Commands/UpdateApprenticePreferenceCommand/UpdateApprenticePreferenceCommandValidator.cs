using FluentValidation;
using SFA.DAS.ApprenticeAccounts.Data.Models;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateApprenticePreferenceCommand
{
    public class UpdateApprenticePreferenceCommandValidator : AbstractValidator<ApprenticePreferences>
    {
        public UpdateApprenticePreferenceCommandValidator()
        {
            RuleFor(model => model.ApprenticeId).NotEmpty();
            RuleFor(model => model.PreferenceId).NotEmpty();
            RuleFor(model => model.Status).NotNull();
        }
    }
}
