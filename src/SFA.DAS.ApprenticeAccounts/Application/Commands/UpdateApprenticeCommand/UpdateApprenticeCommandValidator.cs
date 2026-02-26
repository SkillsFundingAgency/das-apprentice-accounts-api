using FluentValidation;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateApprenticeCommand
{
    public class UpdateApprenticeValidator : AbstractValidator<Apprentice>
    {
        public UpdateApprenticeValidator()
        {
            RuleFor(model => model.FirstName)
                .NotEmpty()
                .WithMessage("Enter your first name")
                .When(x => x.FirstName != null);

            RuleFor(model => model.LastName)
                .NotEmpty()
                .WithMessage("Enter your last name")
                .When(x => x.LastName != null);

            RuleFor(model => model.DateOfBirth)
                .Must(dob => dob != default)
                .WithMessage("Enter your date of birth")
                .When(x => x.DateOfBirth.HasValue);

            RuleFor(model => model.DateOfBirth).Must(dob => dob != DateTime.MinValue).WithMessage("Enter your date of birth");
            RuleFor(model => model.Email.Address).EmailAddress();
        }
    }
}