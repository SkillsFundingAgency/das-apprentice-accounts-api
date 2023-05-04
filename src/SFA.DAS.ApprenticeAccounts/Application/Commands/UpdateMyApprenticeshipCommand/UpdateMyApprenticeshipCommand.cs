using MediatR;
using SFA.DAS.ApprenticeAccounts.Infrastructure.Mediator;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateMyApprenticeshipCommand;

public class UpdateMyApprenticeshipCommand : IRequest<Unit>, IUnitOfWorkCommand
{
    public static implicit operator UpdateMyApprenticeshipCommand(UpdateMyApprenticeshipRequest request)
    {
        return new UpdateMyApprenticeshipCommand
        {
            Uln = request.Uln,
            ApprenticeshipId = request.ApprenticeshipId,
            EmployerName = request.EmployerName,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            StandardUId = request.StandardUId,
            TrainingCode = request.TrainingCode,
            TrainingProviderId = request.TrainingProviderId,
            TrainingProviderName = request.TrainingProviderName
        };
    }

    public Guid ApprenticeId { get; set; }
    public long? Uln { get; set; }
    public long? ApprenticeshipId { get; set; }
    public string? EmployerName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public long? TrainingProviderId { get; set; }
    public string? TrainingProviderName { get; set; }
    public string? TrainingCode { get; set; }
    public string? StandardUId { get; set; }
}