using MediatR;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeAccounts.Application.Queries.GetApprenticeAccountByName
{
    public class GetApprenticeAccountByNameQuery : IRequest<List<Apprentice>?>
    {
        public GetApprenticeAccountByNameQuery(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}