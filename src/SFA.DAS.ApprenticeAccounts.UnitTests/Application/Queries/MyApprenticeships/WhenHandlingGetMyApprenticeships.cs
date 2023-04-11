using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand;
using SFA.DAS.ApprenticeAccounts.Application.Queries.MyApprenticeshipsQuery;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.MyApprenticeships;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Application.Queries.MyApprenticeships;

public class WhenHandlingGetMyApprenticeships
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task ThenExpectedMyApprenticeshpsAreReturned(
            MyApprenticeshipsQuery query,
            Mock<IApprenticeContext> mockApprenticeContext,
            //ApprenticeWithMyApprenticeshipsDto apprenticeWithMyApprenticeships,
            List<MyApprenticeship> myApprenticeships,
            Guid apprenticeId
        )
        // int mockPreferenceId,
        // int mockPreferenceId2,
        // string mockPreferenceMeaning,
        // string mockPreferenceHint,
        // bool mockStatus,
        // bool mockStatus2,
        // DateTime mockCreatedOn,
        // DateTime mockCreatedOn2,
        // DateTime mockUpdatedOn,
        // DateTime mockUpdatedOn2)
    {
        var apprentice = new Apprentice(apprenticeId, "first name", "last name", new MailAddress("test@test.com"),
            DateTime.Now) { MyApprenticeships = myApprenticeships, TermsOfUseAccepted = true};

        // public Apprentice(Guid Id, string firstName, string lastName, MailAddress email, DateTime dateOfBirth)
        {
            mockApprenticeContext.Setup(x => x.Find(query.ApprenticeId)).ReturnsAsync(apprentice);
         
            var handler =
                new MyApprenticeshipsQueryHandler(mockApprenticeContext.Object);
            var result = await handler.Handle(query, CancellationToken.None);

            result.MyApprenticeships.Count().Should().Be(myApprenticeships.Count);

            // var preference = new Preference(mockPreferenceId, mockPreferenceMeaning, mockPreferenceHint);
            // var response = new List<Data.Models.ApprenticePreferences>(2)
            //     {
            //         new Data.Models.ApprenticePreferences
            //             (query.ApprenticeId, mockPreferenceId, mockStatus, mockCreatedOn, mockUpdatedOn),
            //         new Data.Models.ApprenticePreferences
            //             (query.ApprenticeId, mockPreferenceId2, mockStatus2, mockCreatedOn2, mockUpdatedOn2)
            //     };
            // foreach (var apprenticePreference in response)
            // {
            //     apprenticePreference.Preference = preference;
            // }
            //
            // mockContext.Setup(c => c.GetAllApprenticePreferencesForApprentice(query.ApprenticeId))
            //     .ReturnsAsync(response);
            //
            // var handler = new GetAllApprenticePreferencesForApprenticeQueryHandler(mockContext.Object);
            // var result = await handler.Handle(query, CancellationToken.None);
            //
            // result.ApprenticePreferences.Count.Should().Be(response.Count);
            // result.ApprenticePreferences.Should().BeEquivalentTo(response.Select(s => s),
            //     l => l.Excluding(e => e.DomainEvents)
            //         .Excluding(e => e.ApprenticeId)
            //         .Excluding(e => e.CreatedOn)
            //         .Excluding(e => e.Preference)
            //         .Excluding(e => e.Apprentice)
            //         .Excluding(e => e.UpdatedOn));
        }
    }
}
