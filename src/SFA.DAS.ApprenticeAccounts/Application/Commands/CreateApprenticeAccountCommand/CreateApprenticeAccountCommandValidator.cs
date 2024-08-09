using FluentValidation;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.CreateApprenticeAccountCommand
{
    public class CreateApprenticeAccountCommandValidator : AbstractValidator<CreateApprenticeAccountCommand>
    {
        public CreateApprenticeAccountCommandValidator()
        {
            RuleFor(model => model.ApprenticeId).Must(id => id != default).WithMessage("'Apprentice ID' is not a valid identifier.");
            RuleFor(model => model.FirstName).NotEmpty().WithMessage("Enter your first name");
            RuleFor(model => model.LastName).NotEmpty().WithMessage("Enter your last name");
            RuleFor(model => model.DateOfBirth).Must(_ => true).WithMessage("Enter your date of birth");
            RuleFor(model => model.DateOfBirth).Must(dob => dob != DateTime.MinValue).WithMessage("Enter your date of birth");
            RuleFor(model => model.Email).EmailAddress();
        }
    }
}