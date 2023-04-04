using MediatR;
using SFA.DAS.ApprenticeAccounts.Infrastructure.Mediator;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand;

public class CreateMyApprenticeshipCommand : IMyApprenticeshipCommand, IRequest<Unit>, IUnitOfWorkCommand
{
    public Guid ApprenticeId { get; set; }
    public long? Uln { get; set; }
    public long? ApprenticeshipId { get; set; }
    public string? EmployerName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? TrainingProviderId { get; set; }
    public string? TrainingProviderName { get; set; }
    public string? TrainingCode { get; set; }
    public string? StandardUId { get; set; }

    public static implicit operator CreateMyApprenticeshipCommand(CreateMyApprenticeshipRequest command)
    {
        return new CreateMyApprenticeshipCommand
        {
            Uln = command.Uln,
            ApprenticeshipId = command.ApprenticeshipId,
            EmployerName = command.EmployerName,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            StandardUId = command.StandardUId,
            TrainingCode = command.TrainingCode,
            TrainingProviderId = command.TrainingProviderId,
            TrainingProviderName = command.TrainingProviderName
        };
    }
}