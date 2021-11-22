using FluentValidation;
using SFA.DAS.ApprenticeAccounts.Data.Models;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.CreateApprenticeAccountCommand
{
    public class CreateApprenticeAccountCommandValidator : AbstractValidator<CreateApprenticeAccountCommand>
    {
        public CreateApprenticeAccountCommandValidator()
        {
            RuleFor(model => model.ApprenticeId).Must(id => id != default).WithMessage("'Apprentice ID' is not a valid identifier.");
            RuleFor(model => model.FirstName).NotEmpty();
            RuleFor(model => model.LastName).NotEmpty();
            RuleFor(model => model.DateOfBirth).Must(dob => dob != default).WithMessage("'Date Of Birth' is not a valid date.");
            RuleFor(model => model.Email).EmailAddress();
        }
    }

    public class ApprenticeValidator : AbstractValidator<Apprentice>
    {
        public ApprenticeValidator()
        {
            RuleFor(model => model.FirstName).NotEmpty();
            RuleFor(model => model.LastName).NotEmpty();
            RuleFor(model => model.DateOfBirth).Must(dob => dob != default).WithMessage("'Date Of Birth' is not a valid date.");
            RuleFor(model => model.Email).Transform(e => e.ToString()).EmailAddress();
        }
    }
}