using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Queries.GetApprenticePreferencesByApprenticeIdQuery;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Application.Queries.ApprenticePreferences
{
    public class WhenHandlingGetPreferencesById
    {
        [Test, RecursiveMoqAutoData]
        public async Task ThenCorrectPreferencesAreReturned(
            GetApprenticePreferencesByIdQuery query,
            Mock<IApprenticePreferencesContext> mockContext,
            DateTime mockDateTimeOne,
            DateTime mockDateTimeTwo)
        {
            var preference = new Preference(1, "Pref meaning");
            var response = new List<Data.Models.ApprenticePreferences>(2) { new Data.Models.ApprenticePreferences(query.ApprenticeId, 1, true, mockDateTimeOne, mockDateTimeTwo), new Data.Models.ApprenticePreferences(query.ApprenticeId, 2, false, mockDateTimeTwo, mockDateTimeOne) };
            foreach (var item in response)
            {
                item.Preference = preference;
            }
            mockContext.Setup(c => c.GetApprenticePreferencesByIdAsync(query.ApprenticeId)).Returns(response);

            var handler = new GetApprenticePreferencesByIdQueryHandler(mockContext.Object);
            var result = await handler.Handle(query, CancellationToken.None);

            result.ApprenticePreferences.Count.Should().Be(response.Count);
            result.ApprenticePreferences.Should().BeEquivalentTo(response.Select(s => s),
                l => l.Excluding(e => e.DomainEvents)
                .Excluding(e => e.ApprenticeId)
                .Excluding(e => e.CreatedOn)
                .Excluding(e => e.Preference)
                .Excluding(e => e.Apprentice));

        }
    }
}
