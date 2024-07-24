using MediatR;
using SFA.DAS.ApprenticeAccounts.Application.Commands.AddUpdateApprenticeArticleCommand;
using SFA.DAS.ApprenticeAccounts.Infrastructure.Mediator;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.AddUpdateApprenticeArticle
{
    public class AddOrUpdateApprenticeArticleCommand : IRequest<Unit>, IUnitOfWorkCommand
    {
        public static implicit operator AddOrUpdateApprenticeArticleCommand(AddOrUpdateApprenticeArticleRequest request)
        {
            return new AddOrUpdateApprenticeArticleCommand
            {
                Id = request.Id,
                EntryId = request.EntryId,
                LikeStatus = request.LikeStatus,
                IsSaved = request.IsSaved,
                SaveTime = request.SaveTime
            };
        }

        public Guid Id { get; set; }
        public string? EntryId { get; set; }
        public bool? IsSaved { get; set; }
        public bool? LikeStatus { get; set; }
        public DateTime? SaveTime { get; set; }
    }
}