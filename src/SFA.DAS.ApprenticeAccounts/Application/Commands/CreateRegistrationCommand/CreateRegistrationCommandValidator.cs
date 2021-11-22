using FluentValidation;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.CreateRegistrationCommand
{
    public class CreateRegistrationCommandValidator : AbstractValidator<CreateRegistrationCommand>
    {
        public CreateRegistrationCommandValidator()
        {
            RuleFor(model => model.RegistrationId).Must(id => id != default).WithMessage("The Apprentice Id must be valid");
            RuleFor(model => model.CommitmentsApprenticeshipId).Must(id => id > 0).WithMessage("The ApprenticeshipId must be positive");
            RuleFor(model => model.EmployerAccountLegalEntityId).Must(id => id > 0).WithMessage("The EmployerAccountLegalEntityId must be positive");
            RuleFor(model => model.FirstName).NotNull().NotEmpty().WithMessage("FirstName is required");
            RuleFor(model => model.LastName).NotNull().NotEmpty().WithMessage("LastName is required");
            RuleFor(model => model.DateOfBirth).Must(dob => dob != default).WithMessage("The DateOfBirth must be valid");
            RuleFor(model => model.Email).NotNull().EmailAddress().WithMessage("Email must be a valid email address");
            RuleFor(model => model.EmployerName).NotEmpty().WithMessage("The Employer Name is required");
            RuleFor(model => model.TrainingProviderId).Must(id => id > 0).WithMessage("The TrainingProviderId must be positive");
            RuleFor(model => model.TrainingProviderName).NotEmpty().WithMessage("The Training Provider Name is required");
        }
    }
}
