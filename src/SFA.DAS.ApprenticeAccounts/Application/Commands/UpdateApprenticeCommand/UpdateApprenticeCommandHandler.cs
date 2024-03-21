using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateApprenticeCommand
{
    public class UpdateApprenticeCommandHandler : IRequestHandler<UpdateApprenticeCommand, bool>
    {

        private readonly IApprenticeContext _apprentices;

        private readonly ILogger<UpdateApprenticeCommandHandler> _logger;

        public UpdateApprenticeCommandHandler(IApprenticeContext apprenticeships, ILogger<UpdateApprenticeCommandHandler> logger)
        {

            _apprentices = apprenticeships;

            _logger = logger;

        }

        /// <summary>
        /// Handles the update apprentice command.
        /// </summary>
        /// <param name="request">The update apprentice command request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation. True if the apprentice is updated successfully, otherwise false.</returns>
        /// <exception cref="FluentValidation.ValidationException">Thrown when validation fails.</exception>
        public async Task<bool> Handle(UpdateApprenticeCommand request, CancellationToken cancellationToken)
        {

            var apprentice = await _apprentices.Find(request.ApprenticeId);

            if (apprentice != null)
            {

                string prepatchEmail = apprentice.Email.Address;

                string prepatchFirstName = apprentice.FirstName;

                string prepatchLastName = apprentice.LastName;

                ApprenticePatchDto patch = new ApprenticePatchDto(apprentice, _logger);

                request.Updates.ApplyTo(patch);

                var validation = await new UpdateApprenticeValidator().ValidateAsync(apprentice, cancellationToken);

                if (!validation.IsValid)
                {

                    throw new FluentValidation.ValidationException(validation.Errors);

                }

                if
                (

                    !string.Equals(prepatchEmail, new MailAddress(patch.Email).Address, StringComparison.Ordinal) ||

                    !string.Equals(prepatchFirstName, patch.FirstName, StringComparison.Ordinal) ||

                    !string.Equals(prepatchLastName, patch.LastName, StringComparison.Ordinal)
                
                )
                {

                    apprentice.UpdatedOn = DateTime.UtcNow;

                }

                _logger.LogInformation("{HandlerName} Apprentice Id {ApprenticeId}", 
                    
                    nameof(UpdateApprenticeCommandHandler), 
                    
                    request.ApprenticeId
                    
                );

                return true;

            }
            else
            {

                _logger.LogInformation("{HandlerName} Apprentice Id does not exist {ApprenticeId}",
                    
                    nameof(UpdateApprenticeCommandHandler),
                    
                    request.ApprenticeId
                    
                );
                
                return false;

            }

        }

    }

}