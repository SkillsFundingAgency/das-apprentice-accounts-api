using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeAccounts.Data;
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

        public async Task<bool> Handle(UpdateApprenticeCommand request, CancellationToken cancellationToken)
        {
            var apprentice = await _apprentices.Find(request.ApprenticeId);

            if (apprentice != null)
            {
                string prePatchEmail = apprentice.Email.Address;
                string prePatchFirstName = apprentice.FirstName;
                string prePatchLastName = apprentice.LastName;

                ApprenticePatchDto patch = new ApprenticePatchDto(apprentice, _logger);

                request.Updates.ApplyTo(patch);

                var validation = await new UpdateApprenticeValidator().ValidateAsync(apprentice, cancellationToken);

                if (!validation.IsValid)
                {
                    throw new FluentValidation.ValidationException(validation.Errors);
                }

                if(!string.Equals(prePatchEmail, new MailAddress(patch.Email).Address, StringComparison.Ordinal) ||
                   !string.Equals(prePatchFirstName, patch.FirstName, StringComparison.Ordinal) ||
                   !string.Equals(prePatchLastName, patch.LastName, StringComparison.Ordinal))
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