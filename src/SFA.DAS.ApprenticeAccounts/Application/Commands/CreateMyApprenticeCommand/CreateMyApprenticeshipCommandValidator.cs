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
    public const string ApprenticeshipIdAlreadyExists = "This Apprenticeship is already recorded for this apprentice";

    public CreateMyApprenticeshipCommandValidator(IApprenticeContext apprenticeContext, IMyApprenticeshipContext myApprenticeshipContext)
    {
        
        RuleFor(model => model.EmployerName).MaximumLength(200).WithMessage(EmployerNameTooLong);
        RuleFor(model => model.TrainingProviderName).MaximumLength(200).WithMessage(TrainingProviderNameTooLong);
        RuleFor(model => model.TrainingCode).MaximumLength(15).WithMessage(TrainingCodeTooLong);
        RuleFor(model => model.StandardUId).MaximumLength(20).WithMessage(StandardUIdTooLong);
        RuleFor(model => model.ApprenticeId).Must(id => id != Guid.Empty).WithMessage(ApprenticeIdNotValid);
        
        RuleFor(model => model.ApprenticeId)
            .MustAsync(async (model, apprenticeId,cancellation) =>
            {
                var result =  await apprenticeContext.Find(apprenticeId);
                return result != null;
            }).WithMessage(ApprenticeIdNotPresent);

        RuleFor(model => model.ApprenticeshipId)
            .MustAsync(async (model,apprenticeshipId, cancellation) =>
            { 
                if (model.ApprenticeshipId == null) return true;
        
                var myApprenticeships =  await myApprenticeshipContext.FindAll(model.ApprenticeId);
                return myApprenticeships.All(x => x.ApprenticeshipId != apprenticeshipId);
        
            }).WithMessage(ApprenticeshipIdAlreadyExists);
    }
}