using MediatR;
using SFA.DAS.ApprenticeAccounts.Data.Models;

namespace SFA.DAS.ApprenticeAccounts.DomainEvents
{
    internal class ApprenticeEmailAddressChanged : INotification
    {
        public ApprenticeEmailAddressChanged(Apprentice apprentice)
            => Apprentice = apprentice;
        
        public Apprentice Apprentice { get; }
    }
}
