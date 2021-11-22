using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SFA.DAS.ApprenticeCommitments.Infrastructure;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Data.Models
{
    public class ApprenticeCommitmentsDbContext
        : DbContext, IRegistrationContext, IApprenticeContext, IApprenticeshipContext, IRevisionContext
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

        public virtual DbSet<Registration> Registrations { get; set; } = null!;
        public virtual DbSet<Apprentice> Apprentices { get; set; } = null!;
        public virtual DbSet<Apprenticeship> Apprenticeships { get; set; } = null!;
        public virtual DbSet<Revision> Revisions { get; set; } = null!;

        DbSet<Registration> IEntityContext<Registration>.Entities => Registrations;
        DbSet<Apprentice> IEntityContext<Apprentice>.Entities => Apprentices;
        DbSet<Apprenticeship> IEntityContext<Apprenticeship>.Entities => Apprenticeships;
        DbSet<Revision> IEntityContext<Revision>.Entities => Revisions;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Revision>().ToTable("Revision");
            modelBuilder.Entity<Apprenticeship>().ToTable("Apprenticeship");
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
                a.HasMany(e => e.Apprenticeships).WithOne();
                a.Property(e => e.CreatedOn).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
                a.Ignore(e => e.TermsOfUseAccepted)
                    .Property("_termsOfUseAcceptedOn")
                    .HasColumnName("TermsOfUseAcceptedOn");
            });

            modelBuilder.Entity<Revision>(a =>
            {
                a.HasKey("Id");
            });

            modelBuilder.Entity<Revision>()
                .OwnsOne(e => e.Details, details =>
                {
                    details.Property(p => p.EmployerAccountLegalEntityId).HasColumnName("EmployerAccountLegalEntityId");
                    details.Property(p => p.EmployerName).HasColumnName("EmployerName");
                    details.Property(p => p.TrainingProviderId).HasColumnName("TrainingProviderId");
                    details.Property(p => p.TrainingProviderName).HasColumnName("TrainingProviderName");
                    details.OwnsOne(e => e.Course, course =>
                    {
                        course.Property(p => p.Name).HasColumnName("CourseName");
                        course.Property(p => p.Level).HasColumnName("CourseLevel");
                        course.Property(p => p.Option).HasColumnName("CourseOption");
                        course.Property(p => p.PlannedStartDate).HasColumnName("PlannedStartDate");
                        course.Property(p => p.PlannedEndDate).HasColumnName("PlannedEndDate");
                        course.Property(p => p.CourseDuration).HasColumnName("CourseDuration");
                    });
                });

            modelBuilder.Entity<Registration>(entity =>
            {
                entity.HasKey(e => e.RegistrationId);

                entity.Property(e => e.CreatedOn).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
                entity.Property(e => e.Email)
                    .HasConversion(
                        v => v.ToString(),
                        v => new MailAddress(v));
            });

            modelBuilder.Entity<Registration>(entity =>
            {
                entity.Property(e => e.CreatedOn).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
                entity.OwnsOne(e => e.Apprenticeship, apprenticeship =>
                {
                    apprenticeship.Property(p => p.EmployerAccountLegalEntityId)
                        .HasColumnName("EmployerAccountLegalEntityId");
                    apprenticeship.Property(p => p.EmployerName).HasColumnName("EmployerName");
                    apprenticeship.Property(p => p.TrainingProviderId).HasColumnName("TrainingProviderId");
                    apprenticeship.Property(p => p.TrainingProviderName).HasColumnName("TrainingProviderName");
                    apprenticeship.OwnsOne(e => e.Course, course =>
                    {
                        course.Property(p => p.Name).HasColumnName("CourseName");
                        course.Property(p => p.Level).HasColumnName("CourseLevel");
                        course.Property(p => p.Option).HasColumnName("CourseOption");
                        course.Property(p => p.PlannedStartDate).HasColumnName("PlannedStartDate");
                        course.Property(p => p.PlannedEndDate).HasColumnName("PlannedEndDate");
                        course.Property(p => p.CourseDuration).HasColumnName("CourseDuration");
                    });
                });
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