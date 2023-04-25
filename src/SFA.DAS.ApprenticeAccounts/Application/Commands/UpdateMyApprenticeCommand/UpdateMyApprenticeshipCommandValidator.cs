using FluentValidation;
using SFA.DAS.ApprenticeAccounts.Data;
using System.Linq;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateMyApprenticeCommand;
public class UpdateMyApprenticeshipCommandValidator: AbstractValidator<UpdateMyApprenticeshipCommand>
{
    public const string ApprenticeIdNotPresent = "Apprentice Id is not present in the Apprentice table";
    public const string ApprenticeshipIdAlreadyExists = "This Apprenticeship Id is already recorded against the Apprentice Id";
    public const string MyApprenticeshipIdNotPresentForApprenticeId = "MyApprenticeship Id not present for Apprentice Id";

    public UpdateMyApprenticeshipCommandValidator(IApprenticeContext apprenticeContext, IMyApprenticeshipContext myApprenticeshipContext)
    {
        RuleFor(model => model.ApprenticeId)
            .Must((model, apprenticeId, cancellation) =>
            {
                var result = apprenticeContext.Find(apprenticeId).Result;
                return result != null;
            }).WithMessage(ApprenticeIdNotPresent);

        RuleFor(model => model.MyApprenticeshipId)
            .Must((model, apprenticeshipId, cancellation) =>
            {
                var myApprenticeship = myApprenticeshipContext.FindById(model.MyApprenticeshipId).Result;
                return myApprenticeship != null && myApprenticeship.ApprenticeId == model.ApprenticeId;
            }).WithMessage(MyApprenticeshipIdNotPresentForApprenticeId);

        RuleFor(model => model.ApprenticeshipId)
            .Must((model, apprenticeshipId, cancellation) =>
            {
                if (model.ApprenticeshipId == null) return true;

                var myApprenticeships = myApprenticeshipContext.FindAll(model.ApprenticeId).Result;
                var res= !myApprenticeships.Any(x => (x.ApprenticeId != model.ApprenticeId || x.Id != model.MyApprenticeshipId) && x.ApprenticeshipId!=null  && model.ApprenticeshipId!=null  && x.ApprenticeshipId==model.ApprenticeshipId);
                return res;

            }).WithMessage(ApprenticeshipIdAlreadyExists);
    }
}
