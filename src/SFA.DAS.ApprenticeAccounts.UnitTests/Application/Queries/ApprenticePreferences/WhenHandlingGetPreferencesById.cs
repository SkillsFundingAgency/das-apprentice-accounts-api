using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Queries.GetAllApprenticePreferencesForApprenticeQuery;
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
        [Test]
        [RecursiveMoqAutoData]
        public async Task ThenCorrectPreferencesAreReturned(
            GetAllApprenticePreferencesForApprenticeQuery query,
            Mock<IApprenticePreferencesContext> mockContext,
            int mockPreferenceId,
            int mockPreferenceId2,
            string mockPreferenceMeaning,
            bool mockStatus,
            bool mockStatus2,
            DateTime mockCreatedOn,
            DateTime mockCreatedOn2,
            DateTime mockUpdatedOn,
            DateTime mockUpdatedOn2)
        {
            var preference = new Preference(mockPreferenceId, mockPreferenceMeaning);
            var response = new List<Data.Models.ApprenticePreferences>(2)
            {
                new Data.Models.ApprenticePreferences
                    (query.ApprenticeId, mockPreferenceId, mockStatus, mockCreatedOn, mockUpdatedOn),
                new Data.Models.ApprenticePreferences
                    (query.ApprenticeId, mockPreferenceId2, mockStatus2, mockCreatedOn2, mockUpdatedOn2)
            };
            foreach (var apprenticePreference in response)
            {
                apprenticePreference.Preference = preference;
            }

            mockContext.Setup(c => c.GetAllApprenticePreferencesForApprentice(query.ApprenticeId))
                .ReturnsAsync(response);

            var handler = new GetAllApprenticePreferencesForApprenticeQueryHandler(mockContext.Object);
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