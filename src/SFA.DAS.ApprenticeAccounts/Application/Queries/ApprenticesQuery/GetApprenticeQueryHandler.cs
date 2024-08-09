#nullable enable
using MediatR;
using SFA.DAS.ApprenticeAccounts.Configuration;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.ApprenticesQuery
{
    public class GetApprenticeQueryHandler
        : IRequestHandler<GetApprenticeQuery, ApprenticeDto?>
    {
        private readonly IApprenticeContext _apprentices;
        private readonly ApplicationSettings _settings;

        public GetApprenticeQueryHandler(IApprenticeContext apprenticeshipRepository, ApplicationSettings settings)
        {
            _apprentices = apprenticeshipRepository;
            _settings = settings;
        }

        public async Task<ApprenticeDto?> Handle(
            GetApprenticeQuery request,
            CancellationToken cancellationToken)
        {
            var isApprenticeGuid = Guid.TryParse(request.ApprenticeId, out var apprenticeGuid); 
            var apprentice = isApprenticeGuid 
                ? await _apprentices.Find(apprenticeGuid) 
                : await _apprentices.FindByGovIdentifier(request.ApprenticeId);
            
            return ApprenticeDto.Create(apprentice, _settings.TermsOfServiceUpdatedOn);
        }
    }
}