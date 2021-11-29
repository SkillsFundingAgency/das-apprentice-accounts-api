using SFA.DAS.ApprenticeAccounts.Infrastructure;
using SFA.DAS.NServiceBus.Testing.Services;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeAccounts.Api.AcceptanceTests.Bindings
{
    [Binding]
    [Scope(Tag = "api")]
    public class Api
    {
        public static ApprenticeAccountsApi Client { get; set; }
        public static LocalWebApplicationFactory<Startup> Factory { get; set; }
        private static readonly Func<SpecifiedTimeProvider> _time = () => _timeProvider;
        private static readonly Func<TestableEventPublisher> _messages = () => _eventProvider;
        private static SpecifiedTimeProvider _timeProvider;
        private static TestableEventPublisher _eventProvider;

        private readonly TestContext _context;

        public Api(TestContext context)
        {
            _context = context;
            _timeProvider = context.Time;
            _eventProvider = context.Events;
        }

        [BeforeScenario()]
        public void Initialise()
        {
            if (Client == null)
            {
                Factory = CreateApiFactory();
                Client = new ApprenticeAccountsApi(Factory.CreateClient());
            }
            _context.Api = Client;
        }

        public static LocalWebApplicationFactory<Startup> CreateApiFactory()
        {
            var config = new Dictionary<string, string>
                {
                    { "EnvironmentName", "ACCEPTANCE_TESTS" },
                    { "ApplicationSettings:DbConnectionString", TestsDbConnectionFactory.ConnectionString }
                };

            return new LocalWebApplicationFactory<Startup>(config, _time, _messages);
        }

        [AfterFeature()]
        public static void CleanUpFeature()
        {
            Client?.Dispose();
            Client = null;
            Factory?.Dispose();
            _timeProvider = null;
            _eventProvider = null;
        }
    }
}