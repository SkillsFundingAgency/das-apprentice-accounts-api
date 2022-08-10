using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Queries.PreferencesQuery;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Application.Queries.Preferences
{
    public class WhenHandlingGetAllPreferences
    {
        [Test]
        [RecursiveMoqAutoData]
        public async Task ThenPreferencesAreReturned(
            GetAllPreferencesQuery query,
            Mock<IPreferencesContext> mockContext,
            int mockPreferenceId,
            int mockPreferenceId2,
            string mockPreferenceMeaning,
            string mockPreferenceMeaning2,
            string mockPreferenceHint,
            string mockPreferenceHint2)
        {
            var response = new List<Preference>(2)
            {
                new Preference(mockPreferenceId, mockPreferenceMeaning, mockPreferenceHint),
                new Preference(mockPreferenceId2, mockPreferenceMeaning2, mockPreferenceHint2)
            };
            mockContext.Setup(c => c.GetAllPreferences()).ReturnsAsync(response);

            var handler = new GetAllPreferencesQueryHandler(mockContext.Object);
            var result = await handler.Handle(query, CancellationToken.None);

            result.Preferences.Count.Should().Be(response.Count);
            result.Preferences.Should().BeEquivalentTo(response.Select(p => p),
                l => l.Excluding(e => e.DomainEvents)
                    .Excluding(e => e.ApprenticePreferences));
        }
    }
}