using SFA.DAS.ApprenticeAccounts.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApprenticeAccounts.Data
{
    public interface IPreferencesContext : IEntityContext<Preference>
    {
        public List<Preference> GetAllPreferencesAsync()
        {
            return Entities.ToList();
        }

        //public Preference GetPreferencesById(int id)
        //{
        //    return Entities.Find(id);
        //}
    }
}
