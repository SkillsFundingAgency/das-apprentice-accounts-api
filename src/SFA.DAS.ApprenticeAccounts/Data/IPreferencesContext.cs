using Microsoft.EntityFrameworkCore;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Data
{
    public interface IPreferencesContext : IEntityContext<Preference>
    {
        public async Task<IEnumerable<Preference>> GetAllPreferences() => await Entities.ToListAsync();
    }
}