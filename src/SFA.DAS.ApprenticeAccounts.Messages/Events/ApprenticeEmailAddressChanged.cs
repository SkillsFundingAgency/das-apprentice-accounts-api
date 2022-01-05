using System;

namespace SFA.DAS.ApprenticeAccounts.Messages.Events
{
    public class ApprenticeEmailAddressChanged
    {
        public Guid ApprenticeId { get; set; }
        public DateTime ChangedOn { get; set; }
    }
}