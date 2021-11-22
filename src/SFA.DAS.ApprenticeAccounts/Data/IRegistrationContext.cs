using Microsoft.EntityFrameworkCore;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using SFA.DAS.ApprenticeCommitments.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace SFA.DAS.ApprenticeCommitments.Data
{
    public interface IRegistrationContext : IEntityContext<Registration>
    {
        internal async Task<Registration> GetById(Guid apprenticeId)
            => (await Find(apprenticeId))
                ?? throw new DomainException($"Registration for Apprentice {apprenticeId} not found");

        internal async Task<Registration?> Find(Guid apprenticeId)
            => await Entities.FirstOrDefaultAsync(x => x.RegistrationId == apprenticeId);

        internal async Task<Registration?> FindByCommitmentsApprenticeshipId(long commitmentsApprenticeshipId)
            => await Entities.FirstOrDefaultAsync(x => x.CommitmentsApprenticeshipId == commitmentsApprenticeshipId);

        internal Task<List<Registration>> RegistrationsNeedingSignUpReminders(DateTime cutOffDateTime)
            => Entities.Where(r =>
                    r.FirstViewedOn == null && r.SignUpReminderSentOn == null && r.ApprenticeId == null &&
                    r.CreatedOn < cutOffDateTime)
                .ToListAsync(CancellationToken.None);

        public Task<bool> RegistrationsExist()
            => Entities.AnyAsync();
    }
}