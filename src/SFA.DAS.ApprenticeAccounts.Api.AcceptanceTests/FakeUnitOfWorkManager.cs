using System;
using System.Threading.Tasks;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.UnitOfWork.Managers;

namespace SFA.DAS.ApprenticeAccounts.Api.AcceptanceTests
{
    public class FakeUnitOfWorkManager : IUnitOfWorkManager
    {
        private readonly Lazy<ApprenticeAccountsDbContext> _dbContext;

        public FakeUnitOfWorkManager(Lazy<ApprenticeAccountsDbContext> dbContext)
        {
            _dbContext = dbContext;
        }
        public Task BeginAsync()
        {
            return Task.CompletedTask;
        }

        public async Task EndAsync(Exception ex = null)
        {
            if (ex == null) await _dbContext.Value.SaveChangesAsync();
        }
    }
}