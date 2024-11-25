using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.AddUpdateApprenticeArticle
{
    public class RemoveApprenticeArticleCommandHandler : IRequestHandler<RemoveApprenticeArticleCommand, Unit>
    {
        private readonly IApprenticeArticleContext _apprenticeArticleContext;
        private readonly ILogger<RemoveApprenticeArticleCommandHandler> _logger;

        public RemoveApprenticeArticleCommandHandler(
            IApprenticeArticleContext apprenticeArticleContext,
            ILogger<RemoveApprenticeArticleCommandHandler> logger)
        {
            _apprenticeArticleContext = apprenticeArticleContext;
            _logger = logger;
        }

        public async Task<Unit> Handle(RemoveApprenticeArticleCommand request, CancellationToken cancellationToken)
        {
            var apprenticeArticle = await _apprenticeArticleContext.FindByIdAndEntryId(request.Id, request.EntryId);

            if (apprenticeArticle != null)
            {
                _apprenticeArticleContext.Remove(apprenticeArticle);
            }

            _logger.LogInformation("{HandlerName} Apprentice Id {ApprenticeId}",
                nameof(AddOrUpdateApprenticeArticleCommandHandler),
                request.Id
            );

            return await Unit.Task;
        }
    }
}