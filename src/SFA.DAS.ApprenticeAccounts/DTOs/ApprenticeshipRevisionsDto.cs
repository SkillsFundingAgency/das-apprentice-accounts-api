using System;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeCommitments.DTOs
{
    public class ApprenticeshipRevisionsDto
    {
        public Guid ApprenticeId { get; set; }
        public long ApprenticeshipId { get; set; }

        public List<ApprenticeshipRevisionDto> Revisions { get; set; }
            = new List<ApprenticeshipRevisionDto>();
    }
}