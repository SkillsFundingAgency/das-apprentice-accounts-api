using Microsoft.EntityFrameworkCore;
using SFA.DAS.ApprenticeCommitments.Data.Models;

namespace SFA.DAS.ApprenticeCommitments.Migrations
{
    // From das-apprentice-commitments-api\src\SFA.DAS.ApprenticeCommitments.Migrations
    // > dotnet ef dbcontext script

    public class DbContext : ApprenticeCommitmentsDbContext
    {
        public DbContext() : base(
            new DbContextOptionsBuilder<ApprenticeCommitmentsDbContext>()
                .UseSqlServer("Data Source=.;Initial Catalog=SFA.DAS.ApprenticeCommitments.Migrated;Integrated Security=True;Pooling=False;Connect Timeout=30")
                .Options)
        {
        }
    }
}