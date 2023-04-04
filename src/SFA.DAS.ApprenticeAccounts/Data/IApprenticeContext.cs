using Microsoft.EntityFrameworkCore;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.Exceptions;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

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
    }
}