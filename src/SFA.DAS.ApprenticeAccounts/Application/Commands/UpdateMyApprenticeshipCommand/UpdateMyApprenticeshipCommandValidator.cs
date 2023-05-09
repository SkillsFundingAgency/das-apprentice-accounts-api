using FluentValidation;
using SFA.DAS.ApprenticeAccounts.Data;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateMyApprenticeshipCommand;
public class UpdateMyApprenticeshipCommandValidator : AbstractValidator<UpdateMyApprenticeshipCommand>
{
    public const string ApprenticeIdNotPresent = "ApprenticeId is not present in the Apprentice table";
    public const string ApprenticeshipIdAlreadyExists = "This ApprenticeshipId is already recorded against another Apprentice Id";
    public const string MyApprenticeshipNotPresentForApprenticeId = "MyApprenticeship not present for ApprenticeId";

    public UpdateMyApprenticeshipCommandValidator(IApprenticeContext apprenticeContext, IMyApprenticeshipContext myApprenticeshipContext)
    {
        RuleFor(model => model.ApprenticeId)
            .Cascade(CascadeMode.Stop)
            .Must((model, apprenticeId, cancellation) =>
            {
                var result = apprenticeContext.Find(apprenticeId).Result;
                return result != null;
            }).WithMessage(ApprenticeIdNotPresent)
            .Must((model,  cancellation) =>
            {
                var result = apprenticeContext.Find(model.ApprenticeId).Result;
                var myApprenticeship = myApprenticeshipContext.FindByApprenticeId(result.Id).Result;
                return myApprenticeship != null;
            }).WithMessage(MyApprenticeshipNotPresentForApprenticeId);

        RuleFor(model => model.ApprenticeshipId)
            .Must((model, apprenticeshipId, cancellation) =>
            {
                if (model.ApprenticeshipId == null) return true;
                var result = apprenticeContext.Find(model.ApprenticeId).Result;
                if (result == null) return true;
                var myApprenticeship =
                    myApprenticeshipContext.FindByApprenticeshipId((long)model.ApprenticeshipId).Result;
                return myApprenticeship == null || myApprenticeship.ApprenticeId == model.ApprenticeId;
            }).WithMessage(ApprenticeshipIdAlreadyExists);
    }
}
