using SFA.DAS.ApprenticeAccounts.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Data
{
    public interface IPreferencesContext : IEntityContext<Preference>
    {
        public async Task<List<Preference>> GetAllPreferencesAsync()
        {
            return Entities.ToList();
        }
    }
}
