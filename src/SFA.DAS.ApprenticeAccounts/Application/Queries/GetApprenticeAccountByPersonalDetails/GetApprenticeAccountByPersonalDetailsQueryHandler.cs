#nullable enable
using MediatR;
using SFA.DAS.ApprenticeAccounts.Application.Queries.GetApprenticeAccountByPersonalDetails;
using SFA.DAS.ApprenticeAccounts.Configuration;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.ApprenticesQuery
{
    public class GetApprenticeAccountByPersonalDetailsQueryHandler : IRequestHandler<GetApprenticeAccountByPersonalDetailsQuery, List<Apprentice>?>
    {
        private readonly IApprenticeContext _apprentices;        

        public GetApprenticeAccountByPersonalDetailsQueryHandler(IApprenticeContext apprenticeshipRepository, ApplicationSettings settings)
        {
            _apprentices = apprenticeshipRepository;            
        }

        public async Task<List<Apprentice>?> Handle(
            GetApprenticeAccountByPersonalDetailsQuery request,
            CancellationToken cancellationToken)
        {
            var apprentice = await _apprentices.FindByPersonalDetails(request.FirstName, request.LastName, request.DateOfBirth);

            return apprentice;
        }
    }
}