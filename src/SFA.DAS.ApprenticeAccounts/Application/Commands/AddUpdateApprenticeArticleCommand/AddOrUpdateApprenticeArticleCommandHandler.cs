using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.AddUpdateApprenticeArticle
{
    public class AddOrUpdateApprenticeArticleCommandHandler : IRequestHandler<AddOrUpdateApprenticeArticleCommand>
    {
        private readonly IApprenticeArticleContext _apprenticeArticleContext;
        private readonly ILogger<AddOrUpdateApprenticeArticleCommandHandler> _logger;

        public AddOrUpdateApprenticeArticleCommandHandler(
            IApprenticeArticleContext apprenticeArticleContext,
            ILogger<AddOrUpdateApprenticeArticleCommandHandler> logger)
        {
            _apprenticeArticleContext = apprenticeArticleContext;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddOrUpdateApprenticeArticleCommand request, CancellationToken cancellationToken)
        {
            var apprenticeArticle = await _apprenticeArticleContext.FindByIdAndEntryId(request.Id, request.EntryId);

            if (apprenticeArticle != null)
            {
                if (request.IsSaved != null) {
                    apprenticeArticle.IsSaved = request.IsSaved;
                }

                if (request.LikeStatus != null) {
                    apprenticeArticle.LikeStatus = request.LikeStatus;
                }
                
                _apprenticeArticleContext.Update(apprenticeArticle);
            }
            else
            {
               _apprenticeArticleContext.Add(new ApprenticeArticle(
                    request.Id,
                    request.EntryId,
                   request.IsSaved,
                   request.LikeStatus
                ));
            }

            _logger.LogInformation("{HandlerName} Apprentice Id {ApprenticeId}",
                nameof(AddOrUpdateApprenticeArticleCommandHandler),
                request.Id
            );

            return await Unit.Task;
        }
    }
}