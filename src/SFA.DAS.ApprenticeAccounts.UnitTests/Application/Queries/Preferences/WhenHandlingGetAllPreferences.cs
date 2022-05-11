﻿using FluentAssertions;
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
        [Test, RecursiveMoqAutoData]
        public async Task ThenPreferencesAreReturned(
            GetAllPreferencesQuery query,
            Mock<IPreferencesContext> mockContext,
            List<Preference> response)
        {
            mockContext.Setup(c => c.GetAllPreferencesAsync()).Returns(response);

            var handler = new GetAllPreferencesQueryHandler(mockContext.Object);
            var result = await handler.Handle(query, CancellationToken.None);

            result.preferencesDto.Count.Should().Be(response.Count);
            result.preferencesDto.Should().BeEquivalentTo(response.Select(s => s), l => l.Excluding(e => e.DomainEvents));
        }
    }
}
