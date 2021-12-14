using Microsoft.EntityFrameworkCore;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System;

namespace SFA.DAS.ApprenticeAccounts.Api.AcceptanceTests
{
    public class UntrackedApprenticeAccountsDbContext : ApprenticeAccountsDbContext
    {
        public UntrackedApprenticeAccountsDbContext(DbContextOptions<ApprenticeAccountsDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine);
            base.OnConfiguring(optionsBuilder);
        }
    }
}