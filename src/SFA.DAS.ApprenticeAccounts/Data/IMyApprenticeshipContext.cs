using Microsoft.EntityFrameworkCore;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Data
{

    public interface IMyApprenticeshipContext : IEntityContext<MyApprenticeship>
    {
        [ExcludeFromCodeCoverage]
        
        public async Task<MyApprenticeship?> FindByApprenticeId(Guid apprenticeId)
        {
            return await Entities
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ApprenticeId == apprenticeId);
        }

        public async Task<MyApprenticeship?> FindByApprenticeshipId(long apprenticeshipId)
        {
            return await Entities
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ApprenticeshipId == apprenticeshipId);
        }

        public async Task<MyApprenticeship?> FindById(Guid id)
        {
            return await Entities
                .AsNoTracking()
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

    }
}