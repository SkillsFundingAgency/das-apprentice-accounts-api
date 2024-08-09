using MediatR;
using SFA.DAS.ApprenticeAccounts.Configuration;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.CreateOrUpdateApprenticeAccount;

public class CreateOrUpdateApprenticeAccountCommandHandler(IApprenticeContext apprenticeContext, ApplicationSettings applicationSettings) : IRequestHandler<CreateOrUpdateApprenticeAccountCommand, ApprenticeDto>
{
    public async Task<ApprenticeDto> Handle(CreateOrUpdateApprenticeAccountCommand request,
        CancellationToken cancellationToken)
    {
        var apprentice = await apprenticeContext.FindByGovIdentifier(request.GovUkIdentifier) 
                         ?? await apprenticeContext.FindByEmail(new MailAddress(request.Email));

        if (apprentice != null)
        {
            if (string.IsNullOrEmpty(apprentice.GovUkIdentifier))
            {
                apprentice.GovUkIdentifier = request.GovUkIdentifier;
                apprentice.UpdatedOn = DateTime.UtcNow;
            }

            if (!apprentice.Email.Address.Equals(request.Email))
            {
                apprentice.UpdateEmail(new MailAddress(request.Email));
                apprentice.UpdatedOn = DateTime.UtcNow;
            }
            
            return ApprenticeDto.Create(apprentice, applicationSettings.TermsOfServiceUpdatedOn);
        }

        apprentice = new Apprentice(Guid.NewGuid(), null, null, new MailAddress(request.Email), null, request.GovUkIdentifier);
        await apprenticeContext.AddAsync(apprentice, cancellationToken);
        
        return ApprenticeDto.Create(apprentice, applicationSettings.TermsOfServiceUpdatedOn);
    }
}