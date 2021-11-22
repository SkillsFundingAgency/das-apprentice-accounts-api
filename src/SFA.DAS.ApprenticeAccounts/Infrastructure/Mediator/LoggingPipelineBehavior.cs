using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Infrastructure.Mediator
{
    public class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly ILogger<TRequest> _logger;

        public LoggingPipelineBehavior(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                _logger.LogInformation($"Start handling '{typeof(TRequest)}'");
                var response = await next();
                _logger.LogInformation($"End handling '{typeof(TRequest)}'");
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error handling '{typeof(TRequest)}'");
                throw;
            }
        }
    }
}