using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Infrastructure.Mediator
{
    public class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public LoggingPipelineBehavior(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Start handling '{Request}'", typeof(TRequest));
                var response = await next();
                _logger.LogInformation("End handling '{Request}'", typeof(TRequest));
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error handling '{Request}'", typeof(TRequest));
                throw;
            }
        }
    }
}