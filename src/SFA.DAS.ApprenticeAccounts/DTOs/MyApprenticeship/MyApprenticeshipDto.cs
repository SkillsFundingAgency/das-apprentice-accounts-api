﻿using System;

namespace SFA.DAS.ApprenticeAccounts.DTOs.MyApprenticeship;
public class MyApprenticeshipDto
{

    public static implicit operator MyApprenticeshipDto?(Data.Models.MyApprenticeship? source)
    {
        if (source == null)
            return null;

        return new MyApprenticeshipDto
        {
            Uln = source.Uln,
            ApprenticeshipId = source.ApprenticeshipId,
            EmployerName = source.EmployerName,
            StartDate = source.StartDate,
            EndDate = source.EndDate,
            TrainingProviderId = source.TrainingProviderId,
            TrainingProviderName = source.TrainingProviderName,
            TrainingCode = source.TrainingCode,
            StandardUId = source.StandardUId
        };
    }

    public long? Uln { get; set; }
    public long? ApprenticeshipId { get; set; }
    public string? EmployerName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public long? TrainingProviderId { get; set; }
    public string? TrainingProviderName { get; set; }
    public string? TrainingCode { get; set; }
    public string? StandardUId { get; set; }

    public bool IsEmpty()
    {
        return Uln is null &&
               ApprenticeshipId is null &&
               EmployerName is null &&
               StartDate is null &&
               EndDate is null &&
               TrainingProviderId is null &&
               TrainingProviderName is null &&
               TrainingCode is null &&
               StandardUId is null;
    }
}