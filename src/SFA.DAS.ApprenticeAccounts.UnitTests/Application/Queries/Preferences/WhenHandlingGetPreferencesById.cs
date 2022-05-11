using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Queries.ApprenticePreferencesQuery;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Application.Queries.Preferences
{
    public class WhenHandlingGetPreferencesById
    {
        [Test, RecursiveMoqAutoData]
        public async Task ThenCorrectPreferencesAreReturned(
            GetApprenticePreferencesByIdQuery query,
            Mock<IApprenticePreferencesContext> mockContext,
            List<ApprenticePreferences> response)
        {
            mockContext.Setup(c => c.GetApprenticePreferencesByIdAsync(query.ApprenticeId)).Returns(response);

            var handler = new GetApprenticePreferencesByIdQueryHandler(mockContext.Object);
            var result = await handler.Handle(query, CancellationToken.None);

            result.ApprenticePreferences.Count.Should().Be(response.Count);
            result.ApprenticePreferences.Should().BeEquivalentTo(response.Select(s => s),
                l => l.Excluding(e => e.DomainEvents)
                .Excluding(e => e.ApprenticeId)
                .Excluding(e => e.CreatedOn)
                .Excluding(e => e.preference));

        }
    }
}
