using MediatR;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences.GetAllApprenticePreferencesForApprentice;
using System;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.GetAllApprenticePreferencesForApprenticeQuery
{
    public class GetAllApprenticePreferencesForApprenticeQuery : IRequest<GetAllApprenticePreferencesForApprenticeDto>
    {
        public Guid ApprenticeId { get; set; }
    }
}
