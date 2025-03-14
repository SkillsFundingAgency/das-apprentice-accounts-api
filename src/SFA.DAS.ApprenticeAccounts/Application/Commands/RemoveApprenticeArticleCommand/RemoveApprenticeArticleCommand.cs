﻿using MediatR;
using SFA.DAS.ApprenticeAccounts.Application.Commands.AddUpdateApprenticeArticleCommand;
using SFA.DAS.ApprenticeAccounts.Infrastructure.Mediator;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.AddUpdateApprenticeArticle
{
    public class RemoveApprenticeArticleCommand : IRequest<Unit>, IUnitOfWorkCommand
    {
        public static implicit operator RemoveApprenticeArticleCommand(AddOrUpdateApprenticeArticleRequest request)
        {
            return new RemoveApprenticeArticleCommand
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