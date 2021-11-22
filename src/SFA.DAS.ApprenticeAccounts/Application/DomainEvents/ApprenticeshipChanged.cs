using MediatR;
using SFA.DAS.ApprenticeCommitments.Data.Models;

namespace SFA.DAS.ApprenticeCommitments.Application.DomainEvents
{
    internal class ApprenticeshipChanged : INotification
    {
        public Apprenticeship Apprenticeship { get; }

        public ApprenticeshipChanged(Apprenticeship apprenticeship)
            => Apprenticeship = apprenticeship;
    }
}