using System;
using System.Net.Mail;

namespace SFA.DAS.ApprenticeCommitments.Data.Models
{
    public struct PersonalInformation
    {
        public PersonalInformation(
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            MailAddress email)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Email = email;
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public MailAddress Email { get; }
    }
}