using MediatR;
using SFA.DAS.ApprenticeAccounts.Configuration;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using System;
using System.Data;
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

            //This should cover the case where we have searched by gov identifier and have a different email returned
            //in this instance, we check it doesnt match an already existing user that has a gov login account
            //If they do exist with not gov identifier, we switch the identifier to that account 
            if (!apprentice.Email.Address.Equals(request.Email, StringComparison.CurrentCultureIgnoreCase))
            {
                var apprenticeByEmail = await apprenticeContext.FindByEmail(new MailAddress(request.Email));
                if (apprenticeByEmail != null)
                {
                    if (!string.IsNullOrEmpty(apprenticeByEmail.GovUkIdentifier))
                    {
                        throw new ConstraintException("Unable to upsert apprentice");
                    }
                    //In this scenario a gov uk user has changed email to one that is already registered
                    apprentice.GovUkIdentifier = null;
                    apprentice.UpdatedOn = DateTime.UtcNow;    
                    apprenticeByEmail.GovUkIdentifier = request.GovUkIdentifier;
                    apprenticeByEmail.UpdatedOn = DateTime.UtcNow;    
                    return ApprenticeDto.Create(apprenticeByEmail, applicationSettings.TermsOfServiceUpdatedOn);
                }
                else
                {
                    apprentice.UpdateEmail(new MailAddress(request.Email));
                    apprentice.UpdatedOn = DateTime.UtcNow;    
                }
            }
            
            return ApprenticeDto.Create(apprentice, applicationSettings.TermsOfServiceUpdatedOn);
        }

        apprentice = new Apprentice(Guid.NewGuid(), null, null, new MailAddress(request.Email), null, request.GovUkIdentifier);
        await apprenticeContext.AddAsync(apprentice, cancellationToken);
        
        return ApprenticeDto.Create(apprentice, applicationSettings.TermsOfServiceUpdatedOn);
    }
}