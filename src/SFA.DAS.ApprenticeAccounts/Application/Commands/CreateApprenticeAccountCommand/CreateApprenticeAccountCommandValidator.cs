using FluentValidation;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.CreateApprenticeAccountCommand
{
    public class CreateApprenticeAccountCommandValidator : AbstractValidator<CreateApprenticeAccountCommand>
    {
        public CreateApprenticeAccountCommandValidator()
        {
            RuleFor(model => model.ApprenticeId).Must(id => id != default).WithMessage("'Apprentice ID' is not a valid identifier.");
            RuleFor(model => model.FirstName).NotEmpty().WithMessage("Enter your first name");
            RuleFor(model => model.LastName).NotEmpty().WithMessage("Enter your last name");
            RuleFor(model => model.DateOfBirth).Must(dob => dob != default).WithMessage("Enter your date of birth");
            RuleFor(model => model.Email).Transform(e => e.ToString()).EmailAddress();
        }
    }
}