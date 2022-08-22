using Microsoft.EntityFrameworkCore;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Data
{
    public interface IApprenticePreferencesContext : IEntityContext<ApprenticePreferences>
    {
        public async Task<IEnumerable<ApprenticePreferences>?> GetAllApprenticePreferencesForApprentice(
            Guid apprenticeId)
            => await Entities.Where(a => a.ApprenticeId == apprenticeId).Include(s => s.Preference).ToListAsync();

        public async Task<ApprenticePreferences?> GetApprenticePreferenceForApprenticeAndPreference(Guid apprenticeId,
            int preferenceId)
            => await Entities.Where(a => a.ApprenticeId == apprenticeId)
                .Where(a => a.PreferenceId == preferenceId)
                .Include(p => p.Preference)
                .SingleOrDefaultAsync();
    }
}