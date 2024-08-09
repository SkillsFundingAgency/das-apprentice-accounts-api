using Newtonsoft.Json;
using System;

#nullable enable

namespace SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
public class ApprenticeSyncDto
{
    [JsonProperty("apprenticeId")]
    public Guid ApprenticeID { get; set; }

    [JsonProperty("firstName")]
    public string FirstName { get; set; } = null!;

    [JsonProperty("lastName")]
    public string LastName { get; set; } = null!;

    [JsonProperty("dateOfBirth")]
    public DateTime? DateOfBirth { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; } = null!;

    [JsonProperty("lastUpdatedDate")]
    public DateTime LastUpdatedDate { get; set; }

    public static ApprenticeSyncDto MapToSyncResponse(Data.Models.Apprentice apprentice)
    {
        return new ApprenticeSyncDto()
        {
            ApprenticeID = apprentice.Id,
            FirstName = apprentice.FirstName,
            LastName = apprentice.LastName,
            Email = apprentice.Email.Address,
            LastUpdatedDate = apprentice.UpdatedOn,
            DateOfBirth = apprentice.DateOfBirth
        };
    }
}
