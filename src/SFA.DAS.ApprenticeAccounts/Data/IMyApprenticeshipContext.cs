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
        public async Task<IEnumerable<MyApprenticeship>> FindAll(Guid apprenticeId)
        {
            return await Entities
                .AsNoTracking()
                .Where(x => x.ApprenticeId == apprenticeId).ToListAsync();
        }


        public async Task<MyApprenticeship?> FindById(Guid id)
        {
            return Entities
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);
        }

        public async Task<MyApprenticeship?> FindByApprenticeIdMyApprenticeshipId(Guid apprenticeId, Guid myApprenticeshipId)
        {
            return await Entities
                .AsNoTracking()
                .Where(x => x.ApprenticeId == apprenticeId && x.Id == myApprenticeshipId).FirstOrDefaultAsync();
        }
    }
}