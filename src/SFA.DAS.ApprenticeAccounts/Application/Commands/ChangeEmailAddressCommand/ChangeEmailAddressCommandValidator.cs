using FluentValidation;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.CreateRegistrationCommand
{
    public class ChangeEmailAddressCommandValidator : AbstractValidator<ChangeEmailAddressCommand>
    {
        public ChangeEmailAddressCommandValidator()
        {
            RuleFor(model => model.ApprenticeId).Must(id => id != default).WithMessage("The ApprenticeId must be valid");
            RuleFor(model => model.Email).NotNull().EmailAddress().WithMessage("Email must be a valid email address");
        }
    }
}
