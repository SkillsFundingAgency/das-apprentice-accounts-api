using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Infrastructure
{
    public interface IManagedIdentityTokenProvider
    {
        Task<string> GetSqlAccessTokenAsync();
    }
}