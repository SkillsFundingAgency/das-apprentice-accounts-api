﻿using Microsoft.EntityFrameworkCore;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

#nullable enable

namespace SFA.DAS.ApprenticeAccounts.Data
{

    public interface IMyApprenticeshipContext : IEntityContext<MyApprenticeship>
    {
        [ExcludeFromCodeCoverage]
        public async Task<IEnumerable<MyApprenticeship>> FindAll(Guid apprenticeId)
        {
            return await Entities.Where(x => x.ApprenticeId == apprenticeId).ToListAsync();
        }

    }
}