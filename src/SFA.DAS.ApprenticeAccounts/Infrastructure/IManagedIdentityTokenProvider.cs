using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Infrastructure
{
    public interface IManagedIdentityTokenProvider
    {
        Task<string> GetSqlAccessTokenAsync();
    }
}