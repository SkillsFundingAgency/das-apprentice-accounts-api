using System;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using SFA.DAS.ApprenticeCommitments.Infrastructure.Mediator;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands.UpdateRevisionCommand
{
    public class UpdateRevisionCommand : IUnitOfWorkCommand
    {

        public UpdateRevisionCommand(Guid apprenticeId, long apprenticeshipId, long revisionId, JsonPatchDocument<Revision> updates)
        {
            ApprenticeId = apprenticeId;
            ApprenticeshipId = apprenticeshipId;
            RevisionId = revisionId;
            Updates = updates;
        }

        public Guid ApprenticeId { get; }
        public long ApprenticeshipId { get; }
        public long RevisionId { get; }
        public JsonPatchDocument<Revision> Updates { get; }
    }
}