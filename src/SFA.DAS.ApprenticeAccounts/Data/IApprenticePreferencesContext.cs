using Microsoft.EntityFrameworkCore;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApprenticeAccounts.Data
{
    public interface IApprenticePreferencesContext : IEntityContext<ApprenticePreferences>
    {
        public List<ApprenticePreferences> GetApprenticePreferencesByIdAsync(Guid apprenticeId) =>
            Entities.Where(a => a.ApprenticeId == apprenticeId).Include(s => s.Preference).ToList();

        public ApprenticePreferences GetSinglePreferenceValueAsync(Guid apprenticeId, int preferenceId) =>
            Entities.Where(a => a.ApprenticeId == apprenticeId)
                .Where(a => a.PreferenceId == preferenceId)
                .Include(p => p.Preference)
                .SingleOrDefault();
    }
}