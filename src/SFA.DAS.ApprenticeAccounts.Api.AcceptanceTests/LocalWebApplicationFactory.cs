using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Testing;
using SFA.DAS.ApprenticeCommitments.Infrastructure;
using SFA.DAS.UnitOfWork.Managers;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests
{
    public class LocalWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        private readonly Dictionary<string, string> _config;
        private readonly Func<ITimeProvider> _timeProvider;
        private readonly Func<TestableMessageSession> _messages;

        public LocalWebApplicationFactory(Dictionary<string, string> config, Func<ITimeProvider> timeProvider, Func<TestableMessageSession> messages)
        {
            _config = config;
            _timeProvider = timeProvider;
            _messages = messages;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(s =>
            {
                s.AddTransient<IUnitOfWorkManager, FakeUnitOfWorkManager>();
                s.AddTransient<IConnectionFactory, TestsDbConnectionFactory>();
                s.AddTransient((_) => _timeProvider());
                s.AddTransient<IMessageSession>((_) => _messages());
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
