using FluentValidation;
using SFA.DAS.ApprenticeAccounts.Data;
using System;
using System.Linq;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand;

public class CreateMyApprenticeshipCommandValidator : AbstractValidator<CreateMyApprenticeshipCommand>
{
    public const string EmployerNameTooLong = "Employer Name must be 200 characters or fewer";
    public const string TrainingProviderNameTooLong = "Training Provider Name must be 200 characters or fewer";
    public const string TrainingCodeTooLong = "Training Code must be 15 characters or fewer";
    public const string StandardUIdTooLong = "StandardUId must be 20 characters or fewer";
    public const string ApprenticeIdNotValid = "Apprentice Id is not valid";
    public const string ApprenticeIdNotPresent = "Apprentice Id is not present in the Apprentice table";
    public const string MyApprenticeshipAlreadyPresent = "This Apprentice already has a MyApprenticeship record";

    public CreateMyApprenticeshipCommandValidator(IApprenticeContext apprenticeContext, IMyApprenticeshipContext myApprenticeshipContext)
    {
        
        RuleFor(model => model.EmployerName).MaximumLength(200).WithMessage(EmployerNameTooLong);
        RuleFor(model => model.TrainingProviderName).MaximumLength(200).WithMessage(TrainingProviderNameTooLong);
        RuleFor(model => model.TrainingCode).MaximumLength(15).WithMessage(TrainingCodeTooLong);
        RuleFor(model => model.StandardUId).MaximumLength(20).WithMessage(StandardUIdTooLong);
        RuleFor(model => model.ApprenticeId).Must(id => id != Guid.Empty).WithMessage(ApprenticeIdNotValid);
        
        RuleFor(model => model.ApprenticeId)
            .Must((model, apprenticeId,cancellation) =>
            {
                var result =  apprenticeContext.Find(apprenticeId).Result;
                return result != null;
            }).WithMessage(ApprenticeIdNotPresent);

        RuleFor(model => model.ApprenticeshipId)
            .Must((model,apprenticeId, cancellation) =>
            {
                var myApprenticeship =  myApprenticeshipContext.FindByApprenticeId(model.ApprenticeId).Result;
                return myApprenticeship == null;
        
            }).WithMessage(MyApprenticeshipAlreadyPresent);
    }
}