using SFA.DAS.ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand;
using System;

namespace SFA.DAS.ApprenticeAccounts.Data.Models;

public class MyApprenticeship : Entity
{
    private MyApprenticeship()
    {
        // for Entity Framework
    }

    public MyApprenticeship(Guid Id, CreateMyApprenticeshipCommand command )
    {
        this.Id = Id;
        ApprenticeId = command.ApprenticeId;
        Uln = command.Uln;
        ApprenticeshipId = command.ApprenticeshipId;
        EmployerName = command.EmployerName;
        StartDate = command.StartDate;
        EndDate = command.EndDate;
        TrainingProviderId = command.TrainingProviderId;
        TrainingProviderName = command.TrainingProviderName;
        TrainingCode = command.TrainingCode;
        StandardUid = StandardUid;

    }

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

    public string? StandardUid { get; set; }
}
