using FluentValidation;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.ChangeRegistrationCommand
{
    public class ChangeRegistrationCommandValidator : AbstractValidator<ChangeRegistrationCommand>
    {
        public ChangeRegistrationCommandValidator()
        {
            RuleFor(model => model.CommitmentsApprenticeshipId).Must(id => id > 0).WithMessage("The ApprenticeshipId must be positive");
            RuleFor(model => model.EmployerAccountLegalEntityId).Must(id => id > 0).WithMessage("The EmployerAccountLegalEntityId must be positive");
            RuleFor(model => model.FirstName).NotEmpty().WithMessage("The First Name is required");
            RuleFor(model => model.LastName).NotEmpty().WithMessage("The Last Name is required");
            RuleFor(model => model.DateOfBirth).Must(dob => dob != default).WithMessage("The Date of Birth is required");
            RuleFor(model => model.CommitmentsApprenticeshipId).Must(id => id > 0).WithMessage("The ApprenticeshipId must be positive");
            RuleFor(model => model.TrainingProviderId).Must(id => id > 0).WithMessage("The TrainingProviderId must be positive");
            RuleFor(model => model.TrainingProviderName).NotEmpty().WithMessage("The Training Provider Name is required");
        }
    }
}