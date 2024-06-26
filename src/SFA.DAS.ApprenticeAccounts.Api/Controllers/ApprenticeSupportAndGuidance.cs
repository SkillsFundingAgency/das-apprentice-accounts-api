using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAccounts.Application.Commands.AddUpdateApprenticeArticle;
using SFA.DAS.ApprenticeAccounts.Application.Queries.GetAllApprenticeArticlesByIdentifier;
using SFA.DAS.ApprenticeAccounts.Application.Queries.GetAllApprenticeArticlesForApprenticeQuery;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Api.Controllers
{
    [ApiController]
    public class ApprenticeSupportAndGuidance : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApprenticeSupportAndGuidance(IMediator mediator) => _mediator = mediator;

        [HttpGet("apprentices/{id}/articles")]
        public async Task<IActionResult> GetAllApprenticeArticles(Guid id)
        {
            var result = await _mediator.Send(new GetAllApprenticeArticlesForApprenticeQuery
            {
                Id = id
            });
            return Ok(result);
        }

        [HttpGet("apprentices/{id}/articles/{articleIdentifier}")]
        public async Task<IActionResult> GetAllApprenticeArticlesByIdentifier(Guid id, string articleIdentifier)
        {
            var result = await _mediator.Send(new GetAllApprenticeArticlesByIdentifierQuery
            {
                Id = id,
                EntryId = articleIdentifier
            });
            return Ok(result);
        }

        [HttpPost("apprentices/{id}/articles/{articleIdentifier}")]
        public async Task<IActionResult> AddOrUpdateApprenticeArticle(Guid id, string articleIdentifier, [FromBody] ApprenticeArticleRequest request)
        {
            var result = await _mediator.Send(new AddOrUpdateApprenticeArticleCommand
            {
                Id = id,
                EntryId = articleIdentifier,
                IsSaved = request.IsSaved,
                LikeStatus = request.LikeStatus
            });
            return Ok(result);
        }

        public class ApprenticeArticleRequest
        {
            public bool? IsSaved { get; set; }
            public bool? LikeStatus { get; set; }
        }
    }
}