using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SFA.DAS.ApprenticeAccounts.Infrastructure;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Data.Models
{
    public class ApprenticeAccountsDbContext
        : DbContext, IApprenticeContext, IPreferencesContext, IApprenticePreferencesContext
    {
        protected IEventDispatcher _dispatcher;

        public ApprenticeAccountsDbContext(DbContextOptions<ApprenticeAccountsDbContext> options)
            : this(options, new NullEventDispatcher())
        {
        }

        public ApprenticeAccountsDbContext(
            DbContextOptions<ApprenticeAccountsDbContext> options,
            IEventDispatcher dispatcher) : base(options)
        {
            _dispatcher = dispatcher;
        }

        public virtual DbSet<Apprentice> Apprentices { get; set; } = null!;
        public virtual DbSet<Preference> Preference { get; set; }
        public virtual DbSet<ApprenticePreferences> ApprenticePreferences { get; set; }

        DbSet<Apprentice> IEntityContext<Apprentice>.Entities => Apprentices;

        DbSet<Preference> IEntityContext<Preference>.Entities => Preference;
        DbSet<ApprenticePreferences> IEntityContext<ApprenticePreferences>.Entities => ApprenticePreferences;

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

            modelBuilder.Entity<Preference>(p =>
            {
                p.ToTable("Preference")
                 .HasKey(p => p.PreferenceId);                
            });

            modelBuilder.Entity<ApprenticePreferences>( ap =>
            {
                ap.ToTable("ApprenticePreferences")
                 .HasKey(a => new { a.PreferenceId, a.ApprenticeId });

                ap.HasOne(a => a.Apprentice)
                .WithMany(b => b.Preferences)
                .HasForeignKey(c => c.ApprenticeId);

                ap.HasOne(p => p.Preference)
                .WithMany(q => q.ApprenticePreferences)
                .HasForeignKey(r => r.PreferenceId);

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