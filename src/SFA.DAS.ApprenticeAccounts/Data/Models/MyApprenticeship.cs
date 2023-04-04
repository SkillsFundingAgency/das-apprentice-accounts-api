﻿using System;

namespace SFA.DAS.ApprenticeAccounts.Data.Models;

public class MyApprenticeship : Entity
{
    private MyApprenticeship()
    {
        // for Entity Framework
    }

    public MyApprenticeship(Guid Id, Guid apprenticeId, long? uln, long? apprenticeshipId,string? employerName, DateTime? startDate, DateTime? endDate,long? trainingProviderId, string? trainingProviderName, string? trainingCode, string? standardUid )
    {
        this.Id = Id;
        ApprenticeId = apprenticeId;
        Uln = uln;
        ApprenticeshipId = apprenticeshipId;
        EmployerName = employerName;
        StartDate = startDate;
        EndDate = endDate;
        TrainingProviderId = trainingProviderId;
        TrainingProviderName = trainingProviderName;
        TrainingCode = trainingCode;
        StandardUid = standardUid;

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
