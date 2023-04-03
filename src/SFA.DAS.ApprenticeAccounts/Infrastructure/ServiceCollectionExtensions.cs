using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NServiceBus;
using NServiceBus.ObjectBuilder.MSDependencyInjection;
using SFA.DAS.ApprenticeAccounts.Configuration;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.Extensions;
using SFA.DAS.ApprenticeAccounts.Infrastructure.Mediator;
using SFA.DAS.NServiceBus.Configuration.AzureServiceBus;
using SFA.DAS.NServiceBus.Configuration.MicrosoftDependencyInjection;
using SFA.DAS.NServiceBus.Extensions;
using SFA.DAS.NServiceBus.Hosting;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServicesForApprenticeAccounts(this IServiceCollection services)
        {
            services.AddMediatR(typeof(UnitOfWorkPipelineBehavior<,>).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
            services.AddFluentValidation(new[] { typeof(UnitOfWorkPipelineBehavior<,>).Assembly });
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkPipelineBehavior<,>));

            services.AddTransient<ITimeProvider, UtcTimeProvider>();
            services.AddSingleton<IManagedIdentityTokenProvider, ManagedIdentityTokenProvider>();
            services.AddTransient<IConnectionFactory, SqlServerConnectionFactory>();
            services.AddScoped<IApprenticeContext>(s => s.GetRequiredService<ApprenticeAccountsDbContext>());
            services.AddScoped<IPreferencesContext>(s => s.GetRequiredService<ApprenticeAccountsDbContext>());
            services.AddScoped<IApprenticePreferencesContext>(s => s.GetRequiredService<ApprenticeAccountsDbContext>());
            services.AddScoped<IMyApprenticeshipContext>(s => s.GetRequiredService<ApprenticeAccountsDbContext>());
            services.AddScoped<EventDispatcher>();

            return services;
        }

        public static IServiceCollection AddEntityFrameworkForApprenticeAccounts(this IServiceCollection services, IConfiguration config)
        {
            return services.AddScoped(p =>
            {
                var connectionFactory = p.GetRequiredService<IConnectionFactory>();
                var loggerFactory = p.GetRequiredService<ILoggerFactory>();

                ApprenticeAccountsDbContext dbContext;

                var settings = p.GetRequiredService<IOptions<ApplicationSettings>>().Value;
                var optionsBuilder = new DbContextOptionsBuilder<ApprenticeAccountsDbContext>()
                    .UseDataStorage(connectionFactory, settings.DbConnectionString)
                    .UseLocalSqlLogger(loggerFactory, config);
                dbContext = new ApprenticeAccountsDbContext(optionsBuilder.Options, p.GetRequiredService<EventDispatcher>());

                return dbContext;
            });
        }

        public static async Task<UpdateableServiceProvider> StartNServiceBus(this UpdateableServiceProvider serviceProvider, IConfiguration configuration)
        {
            var endpointConfiguration = new EndpointConfiguration("SFA.DAS.ApprenticeAccounts.Api")
                .UseMessageConventions()
                .UseNewtonsoftJsonSerializer()
                .UseServicesBuilder(serviceProvider);

            if (UseLearningTransport(configuration))
            {
                endpointConfiguration.UseTransport<LearningTransport>();
            }
            else
            {
                endpointConfiguration.UseAzureServiceBusTransport(configuration["ApplicationSettings:NServiceBusConnectionString"]);
            }

            if (!string.IsNullOrEmpty(configuration["ApplicationSettings:NServiceBusLicense"]))
            {
                endpointConfiguration.License(configuration["ApplicationSettings:NServiceBusLicense"]);
            }

            var endpoint = await Endpoint.Start(endpointConfiguration);

            serviceProvider.AddSingleton(p => endpoint)
                .AddSingleton<IMessageSession>(p => p.GetRequiredService<IEndpointInstance>())
                .AddHostedService<NServiceBusHostedService>();

            return serviceProvider;
        }

        private static bool UseLearningTransport(IConfiguration configuration) =>
            string.IsNullOrEmpty(configuration["ApplicationSettings:NServiceBusConnectionString"]) ||
            configuration["ApplicationSettings:NServiceBusConnectionString"].Equals("UseLearningEndpoint=true",
                StringComparison.CurrentCultureIgnoreCase);
    }
}