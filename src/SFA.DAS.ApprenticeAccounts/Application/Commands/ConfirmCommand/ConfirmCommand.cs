using SFA.DAS.ApprenticeCommitments.Data.Models;
using SFA.DAS.ApprenticeCommitments.Infrastructure.Mediator;
using System;

#nullable enable

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.ConfirmCommand
{
    public class ConfirmCommand : IUnitOfWorkCommand
    {
        public ConfirmCommand(
            Guid apprenticeId, long apprenticeshipId, long revisionId,
            Confirmations confirmations)
        {
            ApprenticeId = apprenticeId;
            ApprenticeshipId = apprenticeshipId;
            RevisionId = revisionId;
            Confirmations = confirmations;
        }

        public Guid ApprenticeId { get; }
        public long ApprenticeshipId { get; }
        public long RevisionId { get; }
        public Confirmations Confirmations { get; }
    }
}