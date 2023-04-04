using MediatR;
using SFA.DAS.ApprenticeAccounts.Infrastructure.Mediator;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand;

public class CreateMyApprenticeshipCommand : MyApprenticeshipCommand, IRequest<Unit>, IUnitOfWorkCommand
{
    public Guid ApprenticeId { get; set; }

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