using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System;
using System.Net.Mail;

namespace SFA.DAS.ApprenticeAccounts.DTOs
{
    public sealed class ApprenticePatchDto
    {
        private readonly Apprentice _apprentice;
        private readonly ILogger _logger;

        public ApprenticePatchDto(Apprentice apprentice, ILogger logger)
        {
            _apprentice = apprentice;
            _logger = logger;
        }

        public Guid ApprenticeId => _apprentice.Id;

        public string FirstName
        {
            get => _apprentice.FirstName;
            set
            {
                _logger.LogInformation("Patching FirstName for Apprentice {id}", _apprentice.Id);
                _apprentice.FirstName = value;
            }
        }

        public string LastName
        {
            get => _apprentice.LastName;
            set
            {
                _logger.LogInformation("Patching LastName for Apprentice {id}", _apprentice.Id);
                _apprentice.LastName = value;
            }
        }

        public string Email
        {
            get => _apprentice.Email.ToString();
            set
            {
                _logger.LogInformation("Patching Email for Apprentice {id}", _apprentice.Id);
                _apprentice.UpdateEmail(new MailAddress(value));
            }
        }

        public DateTime DateOfBirth
        {
            get => _apprentice.DateOfBirth;
            set
            {
                _logger.LogInformation("Patching DoB for Apprentice {id}", _apprentice.Id);
                _apprentice.DateOfBirth = value;
            }
        }

        public bool TermsOfUseAccepted
        {
            get => _apprentice.TermsOfUseAccepted;
            set
            {
                _logger.LogInformation("Patching TermsOfUse for Apprentice {id}", _apprentice.Id);
                _apprentice.TermsOfUseAccepted = value;
            }
        }
    }
}
