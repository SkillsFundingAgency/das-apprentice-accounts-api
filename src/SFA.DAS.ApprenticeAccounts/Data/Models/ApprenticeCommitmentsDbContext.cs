using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SFA.DAS.ApprenticeAccounts.Infrastructure;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Data.Models
{
    public class ApprenticeCommitmentsDbContext
        : DbContext, IApprenticeContext
    {
        protected IEventDispatcher _dispatcher;

        public ApprenticeCommitmentsDbContext(DbContextOptions<ApprenticeCommitmentsDbContext> options)
            : this(options, new NullEventDispatcher())
        {
        }

        public ApprenticeCommitmentsDbContext(
            DbContextOptions<ApprenticeCommitmentsDbContext> options,
            IEventDispatcher dispatcher) : base(options)
        {
            _dispatcher = dispatcher;
        }

        public virtual DbSet<Apprentice> Apprentices { get; set; } = null!;

        DbSet<Apprentice> IEntityContext<Apprentice>.Entities => Apprentices;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Apprentice>(a =>
            {
                a.ToTable("Apprentice");
                a.HasKey(e => e.Id);
                a.Property(e => e.Email)
                 .HasConversion(
                     v => v.ToString(),
                     v => new MailAddress(v));
                a.OwnsMany(
                    e => e.PreviousEmailAddresses,
                    c =>
                    {
                        c.HasKey("Id");
                        c.Property(typeof(long), "Id");
                        c.HasIndex("ApprenticeId");
                        c.Property(e => e.EmailAddress)
                            .HasConversion(
                                v => v.ToString(),
                                v => new MailAddress(v));
                    });
                a.Property(e => e.CreatedOn).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
                a.Ignore(e => e.TermsOfUseAccepted)
                    .Property("_termsOfUseAcceptedOn")
                    .HasColumnName("TermsOfUseAcceptedOn");
            });
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var numEntriesWritten = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await DispatchDomainEvents();
            return numEntriesWritten;
        }

        private async Task DispatchDomainEvents()
        {
            var events = ChangeTracker.Entries<Entity>()
                .SelectMany(x => x.Entity.DomainEvents)
                .ToArray();

            foreach (var domainEvent in events)
                await _dispatcher.Dispatch(domainEvent);
        }
    }
}