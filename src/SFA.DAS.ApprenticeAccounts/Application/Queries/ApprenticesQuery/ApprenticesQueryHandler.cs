﻿using MediatR;
using SFA.DAS.ApprenticeAccounts.Configuration;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.DTOs;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.ApprenticesQuery
{
    public class ApprenticesQueryHandler
        : IRequestHandler<ApprenticesQuery, ApprenticeDto?>
    {
        private readonly IApprenticeContext _apprentices;
        private readonly ApplicationSettings _settings;

        public ApprenticesQueryHandler(IApprenticeContext apprenticeshipRepository, ApplicationSettings settings)
        {
            _apprentices = apprenticeshipRepository;
            _settings = settings;
        }

        public async Task<ApprenticeDto?> Handle(
            ApprenticesQuery request,
            CancellationToken cancellationToken)
        {
            var apprentice = await _apprentices.Find(request.ApprenticeId);
            
            return ApprenticeDto.Create(apprentice, _settings.TermsOfServiceUpdatedOn);
        }
    }
}