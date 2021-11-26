using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.ApprenticeAccounts.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Infrastructure
{
    public class ApprenticeAccountsHealthCheck : IHealthCheck
    {
        private const string HealthCheckResultsDescription = "Apprentice Accounts API Health Check";
        private readonly IApprenticeContext _registrationRepository;

        public ApprenticeAccountsHealthCheck(IApprenticeContext registrationRepository)
        {
            _registrationRepository = registrationRepository;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var dbConnectionHealthy = true;
            try
            {
                await _registrationRepository.Entities.Take(1).ToListAsync();
            }
            catch
            {
                dbConnectionHealthy = false;
            }

            return dbConnectionHealthy ? HealthCheckResult.Healthy(HealthCheckResultsDescription) : HealthCheckResult.Unhealthy(HealthCheckResultsDescription);
        }
    }
}