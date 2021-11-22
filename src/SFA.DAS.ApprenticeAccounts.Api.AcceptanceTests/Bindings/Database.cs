using Microsoft.EntityFrameworkCore;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeAccounts.Api.AcceptanceTests.Bindings
{
    [Binding]
    [Scope(Tag = "database")]
    public class Database
    {
        private readonly TestContext _context;
        private static TestsDbConnectionFactory dbFactory = new TestsDbConnectionFactory();

        public Database(TestContext context)
        {
            _context = context;
        }

        [BeforeScenario()]
        public void Initialise()
        {
            _context.DbContext = CreateDbContext();
        }

        public static ApprenticeCommitmentsDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApprenticeCommitmentsDbContext>();
            var options = dbFactory
                .AddConnection(optionsBuilder)
                .EnableSensitiveDataLogging()
                .Options;
            var context = new UntrackedApprenticeCommitmentsDbContext(options);

            dbFactory.EnsureCreated(context);

            return context;
        }

        [AfterScenario()]
        public void Cleanup()
        {
            dbFactory.EnsureDeleted(_context.DbContext);
            _context?.DbContext?.Dispose();
        }
    }
}