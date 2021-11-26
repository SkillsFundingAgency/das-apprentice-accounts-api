using Microsoft.EntityFrameworkCore;
using SFA.DAS.ApprenticeAccounts.Data.Models;

namespace SFA.DAS.ApprenticeAccounts.Migrations
{
    // From das-apprentice-accounts-api\src\SFA.DAS.ApprenticeAccounts.Migrations
    // > dotnet ef dbcontext script

    public class DbContext : ApprenticeAccountsDbContext
    {
        public DbContext() : base(
            new DbContextOptionsBuilder<ApprenticeAccountsDbContext>()
                .UseSqlServer("Data Source=.;Initial Catalog=SFA.DAS.ApprenticeAccounts.Migrated;Integrated Security=True;Pooling=False;Connect Timeout=30")
                .Options)
        {
        }
    }
}