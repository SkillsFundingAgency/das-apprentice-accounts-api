using FluentValidation;

namespace SFA.DAS.ApprenticeCommitments.Application.Queries.RegistrationQuery
{
    public class RegistrationQueryValidator : AbstractValidator<RegistrationQuery>
    {
        public RegistrationQueryValidator()
        {
            RuleFor(model => model.ApprenticeId).Must(id => id != default).WithMessage("The Apprentice Id must be valid");
        }
    }
}
