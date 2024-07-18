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
    public interface IApprenticeArticleContext : IEntityContext<ApprenticeArticle>
    {
        internal async Task<ApprenticeArticle> GetById(Guid id)
            => await Find(id)
                     ?? throw new DomainException(
                        $"Apprentice {id} not found");

        public async Task<ApprenticeArticle?> Find(Guid id)
            => await Entities.SingleOrDefaultAsync(a => a.Id == id);

        public async Task<ApprenticeArticle?> FindByIdAndEntryId(Guid id, string entryId)
            => await Entities.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id && a.EntryId == entryId);

        public async Task<ApprenticeArticle[]> GetAllArticlesByUserId(Guid id)
            => await Entities
                .Where(x => x.Id == id)
                .ToArrayAsync();

        public async Task<ApprenticeArticle[]> GetArticlesByUserIdAndEntryId(Guid id, string entryId)
            => await Entities
                .Where(x => x.Id == id && x.EntryId == entryId)
                .ToArrayAsync();

    }
}