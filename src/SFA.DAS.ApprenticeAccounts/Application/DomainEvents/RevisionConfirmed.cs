using MediatR;
using SFA.DAS.ApprenticeCommitments.Data.Models;

namespace SFA.DAS.ApprenticeCommitments.Application.DomainEvents
{
    internal class RevisionConfirmed : INotification
    {
        public Revision Revision { get; }

        public RevisionConfirmed(Revision revision)
            => Revision = revision;
    }
}