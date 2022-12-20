using FluentValidation;
using SFA.DAS.ApprenticeAccounts.Data.Models;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateApprenticeCommand
{
    public class UpdateApprenticeValidator : AbstractValidator<Apprentice>
    {
        public UpdateApprenticeValidator()
        {
            RuleFor(model => model.FirstName).NotEmpty().WithMessage("Enter your first name");
            RuleFor(model => model.LastName).NotEmpty().WithMessage("Enter your last name");
            RuleFor(model => model.DateOfBirth).Must(dob => dob != default).WithMessage("Enter your date of birth");
            RuleFor(model => model.Email.Address).EmailAddress();
        }
    }
}