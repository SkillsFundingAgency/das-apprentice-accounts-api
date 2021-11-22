using NServiceBus.Testing;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.Infrastructure;
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

        public ApprenticeCommitmentsApi Api { get; set; }
        public ApprenticeCommitmentsDbContext DbContext { get; set; }

        public SpecifiedTimeProvider Time { get; set; }
            = new SpecifiedTimeProvider(DateTime.UtcNow);

        public TestableMessageSession Messages { get; set; }
            = new TestableMessageSession();

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