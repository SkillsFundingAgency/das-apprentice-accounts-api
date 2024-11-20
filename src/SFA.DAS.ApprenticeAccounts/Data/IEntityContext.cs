using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace SFA.DAS.ApprenticeAccounts.Data
{
    public interface IEntityContext<T> where T : class
    {
        DbSet<T> Entities { get; }

        EntityEntry<T> Add(T entity) => Entities.Add(entity);

        ValueTask<EntityEntry<T>> AddAsync(T entity, CancellationToken cancellationToken = default)
            => Entities.AddAsync(entity, cancellationToken);

        [ExcludeFromCodeCoverage]
        EntityEntry<T> Update(T entity) => Entities.Update(entity);

        [ExcludeFromCodeCoverage]
        EntityEntry<T> Remove(T entity) => Entities.Remove(entity);
    }
}