using MediatR;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.GetApprenticeAccountByPersonalDetails
{
    public class GetApprenticeAccountByPersonalDetailsQuery : IRequest<List<Apprentice>?>
    {
        public GetApprenticeAccountByPersonalDetailsQuery(string firstName, string lastName, DateTime dateOfBirth)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}