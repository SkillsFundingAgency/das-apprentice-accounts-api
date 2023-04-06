﻿using SFA.DAS.ApprenticeAccounts.Data.Models;
using System;

namespace SFA.DAS.ApprenticeAccounts.DTOs.MyApprenticeships;

public class MyApprenticeshipsDto
{
    public Guid Id { get; set; }
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

    public static implicit operator MyApprenticeshipsDto(MyApprenticeship? source)
    {
        if (source == null) return null!;

        var myApprenticeshipsDto = new MyApprenticeshipsDto
        {
            Id = source.Id,
            Uln = source.Uln,
            ApprenticeshipId = source.ApprenticeshipId,
            EmployerName = source.EmployerName,
            StartDate = source.StartDate,
            EndDate = source.EndDate,
            TrainingProviderId = source.TrainingProviderId,
            TrainingProviderName = source.TrainingProviderName,
            TrainingCode = source.TrainingCode,
            StandardUId = source.StandardUId,
            CreatedOn = source.CreatedOn
        };

        return myApprenticeshipsDto;
    }
}