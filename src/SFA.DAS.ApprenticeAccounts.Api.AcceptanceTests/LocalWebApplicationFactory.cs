using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using SFA.DAS.ApprenticeAccounts.Infrastructure;
using SFA.DAS.NServiceBus.Services;
using SFA.DAS.NServiceBus.Testing.Services;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeAccounts.Api.AcceptanceTests
{
    public class LocalWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        private readonly Dictionary<string, string> _config;
        private readonly Func<ITimeProvider> _timeProvider;
        private readonly Func<TestableEventPublisher> _events;

        public LocalWebApplicationFactory(Dictionary<string, string> config, Func<ITimeProvider> timeProvider, Func<TestableEventPublisher> events)
        {
            _config = config;
            _timeProvider = timeProvider;
            _events = events;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(s =>
            {
                s.AddTransient<IConnectionFactory, TestsDbConnectionFactory>();
                s.AddTransient((_) => _timeProvider());
                s.AddTransient<IEventPublisher>((_) => _events());
                s.AddTransient<IMessageSession>((_) => new TestEventPublisher(_events.Invoke()));
            });

            builder.ConfigureAppConfiguration(a =>
            {
                a.Sources.Clear();
                a.AddInMemoryCollection(_config);
            });
            builder.UseEnvironment("Development");
        }
    }
}
