using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ApprenticeCommitments.DTOs;

namespace SFA.DAS.ApprenticeCommitments.Application.Queries.RegistrationRemindersQuery
{
    public class RegistrationRemindersResponse
    {
        public RegistrationRemindersResponse(IEnumerable<RegistrationDto> registrations)
        {
            Registrations = registrations.ToList();
        }
        public List<RegistrationDto> Registrations { get; set; }
    }
}
