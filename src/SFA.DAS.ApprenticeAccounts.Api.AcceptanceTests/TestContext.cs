using SFA.DAS.ApprenticeAccounts.Configuration;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.Infrastructure;
using SFA.DAS.NServiceBus.Testing.Services;
using System;

namespace SFA.DAS.ApprenticeAccounts.Api.AcceptanceTests
{
    public class TestContext : IDisposable
    {
        private bool isDisposed;

        public TestContext()
        {
            isDisposed = false;
        }

        public ApprenticeAccountsApi Api { get; set; }
        public ApprenticeAccountsDbContext DbContext { get; set; }

        public SpecifiedTimeProvider Time { get; set; }
            = new SpecifiedTimeProvider(DateTime.UtcNow);

        public TestableEventPublisher Events { get; set; }
            = new TestableEventPublisher();

        public ApplicationSettings ApplicationSettings { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;

            if (disposing)
            {
                Api?.Dispose();
            }

            isDisposed = true;
        }
    }
}