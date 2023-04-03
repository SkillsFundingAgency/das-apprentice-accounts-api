using SFA.DAS.ApprenticeAccounts.Data.Models;

#nullable enable

namespace SFA.DAS.ApprenticeAccounts.Data
{
    public interface IMyApprenticeshipContext : IEntityContext<MyApprenticeship>
    {
        // internal async Task<MyApprenticeship> Find(Guid apprenticeId)
        //     => await Entities.SingleOrDefaultAsync(a => a.Id == apprenticeId);
    }
}