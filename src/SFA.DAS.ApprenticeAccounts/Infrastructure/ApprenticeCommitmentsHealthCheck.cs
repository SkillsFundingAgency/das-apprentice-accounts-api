using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.ApprenticeCommitments.Data;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Infrastructure
{
    public class ApprenticeCommitmentsHealthCheck : IHealthCheck
    {
        private const string HealthCheckResultsDescription = "Apprentice Commitments API Health Check";
        private readonly IRegistrationContext _registrationRepository;

        public ApprenticeCommitmentsHealthCheck(IRegistrationContext registrationRepository)
        {
            _registrationRepository = registrationRepository;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var dbConnectionHealthy = true;
            try
            {
                await _registrationRepository.RegistrationsExist();
            }
            catch
            {
                dbConnectionHealthy = false;
            }

            return dbConnectionHealthy ? HealthCheckResult.Healthy(HealthCheckResultsDescription) : HealthCheckResult.Unhealthy(HealthCheckResultsDescription);
        }
    }
}