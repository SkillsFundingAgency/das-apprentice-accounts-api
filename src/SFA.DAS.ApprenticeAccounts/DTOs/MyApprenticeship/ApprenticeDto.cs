using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApprenticeAccounts.DTOs.MyApprenticeship;
public class ApprenticeDto
{
    public Guid ApprenticeId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public IEnumerable<MyApprenticeshipsDto>? MyApprenticeships { get; set; }
    
    public static ApprenticeDto? Create(Data.Models.Apprentice? source, IEnumerable<Data.Models.MyApprenticeship> myApprenticeships)
    {
        if (source == null) return null!;

        var apprentice = new ApprenticeDto
        {
            ApprenticeId = source.Id,
            FirstName = source.FirstName,
            LastName = source.LastName,
            Email = source.Email.ToString(),
            DateOfBirth = source.DateOfBirth,
            MyApprenticeships = myApprenticeships.OrderByDescending(c => c.CreatedOn).Select(myApprenticeship => (MyApprenticeshipsDto)myApprenticeship)
        };

        return apprentice;
    }
}