using FluentValidation;
using SFA.DAS.ApprenticeAccounts.DomainEvents;
using SFA.DAS.ApprenticeAccounts.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace SFA.DAS.ApprenticeAccounts.Data.Models
{
    public class Apprentice : Entity
    {
        private Apprentice()
        {
            // for Entity Framework
        }

        public Apprentice(Guid Id, string firstName, string lastName, MailAddress email, DateTime dateOfBirth)
        {
            this.Id = Id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            DateOfBirth = dateOfBirth;
            PreviousEmailAddresses = new[] { new ApprenticeEmailAddressHistory(email) };
        }

        public Guid Id { get; private set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public MailAddress Email { get; private set; } = null!;
        public ICollection<ApprenticeEmailAddressHistory> PreviousEmailAddresses { get; private set; } = null!;

        private DateTime _dateOfBirth;

        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                // domain validation to go here
                if (false) throw new DomainException("validation error");
                _dateOfBirth = value;
            }
        }

        private DateTime? _termsOfUseAcceptedOn;

        public bool TermsOfUseAccepted
        {
            get => _termsOfUseAcceptedOn != null;
            set
            {
                if (!value) throw new ValidationException("Cannot decline the Terms of Use");
                _termsOfUseAcceptedOn = DateTime.Now;
            }
        }

        public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;

        internal void UpdateEmail(MailAddress newEmail)
        {
            if (newEmail.Address == Email.Address) return;
            Email = newEmail;
            PreviousEmailAddresses.Add(new ApprenticeEmailAddressHistory(Email));
            DomainEvents.Add(new ApprenticeEmailAddressChanged(this));
        }
    }

    public class ApprenticeEmailAddressHistory
    {
        private ApprenticeEmailAddressHistory()
        {
        }

        public ApprenticeEmailAddressHistory(MailAddress emailAddress)
            => EmailAddress = emailAddress;

        public MailAddress EmailAddress { get; private set; } = null!;
        public DateTime ChangedOn { get; private set; } = DateTime.UtcNow;
    }
}