using Microsoft.EntityFrameworkCore;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using SFA.DAS.ApprenticeCommitments.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

#nullable enable

namespace SFA.DAS.ApprenticeCommitments.Data
{
    public interface IRevisionContext : IEntityContext<Revision>
    {

        internal async Task<Revision> GetById(Guid apprenticeId, long apprenticeshipId, long revisionId)
        {
            var revision = await Entities.Include(e => e.Apprenticeship).Where(e => e.Id == revisionId).SingleOrDefaultAsync();

            if (revision == null)
            {
                throw new DomainException($"Revision {revisionId} not found");
            }

            if (revision.ApprenticeshipId != apprenticeshipId || revision.Apprenticeship.ApprenticeId != apprenticeId)
            {
                throw new DomainException($"Revision {revisionId} does not belong to Apprenticeship {apprenticeshipId}");
            }

            return revision;
        }

        internal async Task<Revision?> FindLatestByCommitmentsApprenticeshipId(long apprenticeshipId)
            => await Entities
                .Where(x => x.CommitmentsApprenticeshipId == apprenticeshipId)
                .OrderByDescending(x => x.CommitmentsApprovedOn)
                .FirstOrDefaultAsync();
    }
}