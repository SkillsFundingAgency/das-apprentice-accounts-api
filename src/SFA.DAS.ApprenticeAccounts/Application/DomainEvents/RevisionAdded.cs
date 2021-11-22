using MediatR;
using SFA.DAS.ApprenticeCommitments.Data.Models;

namespace SFA.DAS.ApprenticeCommitments.Application.DomainEvents
{
    internal class RevisionAdded : INotification
    {
        public Revision Revision { get; }

        public RevisionAdded(Revision revision)
            => Revision = revision;
    }
}