using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.Exceptions;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

#nullable enable

namespace SFA.DAS.ApprenticeAccounts.Data
{
    public interface IApprenticeContext : IEntityContext<Apprentice>
    {
        internal async Task<Apprentice> GetById(Guid apprenticeId)
            => await Find(apprenticeId)
                     ?? throw new DomainException(
                        $"Apprentice {apprenticeId} not found");

        public async Task<Apprentice?> Find(Guid apprenticeId)
            => await Entities.SingleOrDefaultAsync(a => a.Id == apprenticeId);

        internal async Task<Apprentice[]> GetByEmail(MailAddress email)
            => await Entities
                .Where(x => x.Email == email)
                .ToArrayAsync();

        public async Task<Apprentice[]> GetForSync(Guid[] ids, DateTime? UpdatedSince)
            => await Entities
                .Where(x => ids.Contains(x.Id) && (!UpdatedSince.HasValue || x.UpdatedOn.Date > UpdatedSince.Value.Date))
                .ToArrayAsync();

        public async Task<Apprentice?> FindByGovIdentifier(string govUkIdentifier) 
            => await Entities.SingleOrDefaultAsync(c=>c.GovUkIdentifier == govUkIdentifier);
    }
}