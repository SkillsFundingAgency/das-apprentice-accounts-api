using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NServiceBus;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

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
                request.Updates.ApplyTo(new ApprenticePatchDto(apprentice, _logger));

                var validation = await new UpdateApprenticeValidator().ValidateAsync(apprentice, cancellationToken);
                if (!validation.IsValid) throw new FluentValidation.ValidationException(validation.Errors);

                _logger.LogInformation("UpdateApprenticeCommandHandler Apprentice Id {ApprenticeId}", request.ApprenticeId);

                return true;
            }
            else
            {
                _logger.LogInformation("UpdateApprenticeCommandHandler Apprentice Id does not exist {ApprenticeId}", request.ApprenticeId);
                
                return false;
            }
        }

    }
}