#nullable enable
using MediatR;
using SFA.DAS.ApprenticeAccounts.Application.Queries.GetApprenticeAccountByName;
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
    public class GetApprenticeAccountByNameQueryHandler : IRequestHandler<GetApprenticeAccountByNameQuery, List<Apprentice>?>
    {
        private readonly IApprenticeContext _apprentices;
        private readonly ApplicationSettings _settings;

        public GetApprenticeAccountByNameQueryHandler(IApprenticeContext apprenticeshipRepository, ApplicationSettings settings)
        {
            _apprentices = apprenticeshipRepository;
            _settings = settings;
        }

        public async Task<List<Apprentice?>> Handle(
            GetApprenticeAccountByNameQuery request,
            CancellationToken cancellationToken)
        {
            var apprentice = await _apprentices.FindByName(request.FirstName, request.LastName);

            return apprentice;
        }
    }
}