using SFA.DAS.ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateMyApprenticeshipCommand;
using System;

namespace SFA.DAS.ApprenticeAccounts.Data.Models;

public class MyApprenticeship : Entity
{
    public MyApprenticeship()
    {
    }

    // public MyApprenticeship(Guid apprenticeId, long? apprenticeshipId, Guid id)
    // {
    //     ApprenticeId = apprenticeId;
    //     ApprenticeshipId = apprenticeshipId;
    //     Id = id;
    // }

    public Guid Id { get; set; }
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
    public DateTime CreatedOn { get; set; }

    public static implicit operator MyApprenticeship(CreateMyApprenticeshipCommand command)
    {
        return new MyApprenticeship
        {
            Id = command.Id,
            ApprenticeId = command.ApprenticeId,
            Uln = command.Uln,
            ApprenticeshipId = command.ApprenticeshipId,
            EmployerName = command.EmployerName,
            StartDate = command.StartDate,
            EndDate = command.EndDate,
            StandardUId = command.StandardUId,
            TrainingCode = command.TrainingCode,
            TrainingProviderId = command.TrainingProviderId,
            TrainingProviderName = command.TrainingProviderName,
            CreatedOn = command.CreatedOn
        };
    }

    public void UpdateMyApprenticeship(UpdateMyApprenticeshipCommand command)
    {
        Uln = command.Uln;
        ApprenticeshipId = command.ApprenticeshipId;
        EmployerName = command.EmployerName;
        StartDate = command.StartDate;
        EndDate = command.EndDate;
        StandardUId = command.StandardUId;
        TrainingCode = command.TrainingCode;
        TrainingProviderId = command.TrainingProviderId;
        TrainingProviderName = command.TrainingProviderName;
    }
}
