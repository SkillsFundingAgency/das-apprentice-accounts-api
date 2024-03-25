using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Queries.SyncApprenticeAccountsQuery;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Application.Queries.ApprenticeSync;
public class WhenHandlingSyncApprentices
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task WithNullDateThenExpectedArrayOfApprenticeSyncDtoReturned(
            [Frozen] Mock<ILogger<SyncApprenticeAccountHandler>> logger,
            [Frozen] Mock<IApprenticeContext> mockApprenticeContext,
            [Frozen] Guid apprenticeId,
            [Frozen] string firstName,
            [Frozen] string surname,
            [Frozen] MailAddress email,
            [Frozen] DateTime dateOfBirth,
            [Frozen] DateTime updatedOn
        )
    {
        SyncApprenticeAccountQuery query = new SyncApprenticeAccountQuery(null, new Guid[] { apprenticeId });

        var apprentice = new Apprentice(
            apprenticeId,
            firstName,
            surname,
            email,
            dateOfBirth
        )
        {
            UpdatedOn = updatedOn
        };

        var mockDbSet = MockDbSetSetup.CreateQueryableMockDbSet(new List<Apprentice> { apprentice });

        mockApprenticeContext
            .Setup(x => x.Entities)
            .Returns(mockDbSet.Object);

        var handler = new SyncApprenticeAccountHandler(logger.Object, mockApprenticeContext.Object);

        var result = await handler.Handle(query, CancellationToken.None);

        var expectedResult = new ApprenticeSyncDto() {
            ApprenticeID = apprenticeId,
            FirstName = firstName,
            LastName = surname,
            Email = email.Address,
            LastUpdatedDate = updatedOn,
            DateOfBirth = dateOfBirth
        };

        result.Apprentices.Should().ContainSingle();
        result.Apprentices.First().Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task WithEmptyApprenticeIdArrayEmptyArrayReturned(
            [Frozen] Mock<ILogger<SyncApprenticeAccountHandler>> logger,
            [Frozen] Mock<IApprenticeContext> mockApprenticeContext
        )
    {
        SyncApprenticeAccountQuery query = new SyncApprenticeAccountQuery(null, new Guid[] {  });

        var handler = new SyncApprenticeAccountHandler(logger.Object, mockApprenticeContext.Object);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Apprentices.Should().BeEmpty();
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task WithFutureDateThenExpectedArrayOfApprenticeSyncDtoReturned(
            [Frozen] Mock<ILogger<SyncApprenticeAccountHandler>> logger,
            [Frozen] Mock<IApprenticeContext> mockApprenticeContext,
            [Frozen] Guid apprenticeId,
            [Frozen] string firstName,
            [Frozen] string surname,
            [Frozen] MailAddress email,
            [Frozen] DateTime dateOfBirth,
            [Frozen] DateTime updatedSinceDate

        )
    {
        SyncApprenticeAccountQuery query = new SyncApprenticeAccountQuery(updatedSinceDate, new Guid[] { apprenticeId });

        var apprentice = new Apprentice(
            apprenticeId,
            firstName,
            surname,
            email,
            dateOfBirth
        )
        {
            UpdatedOn = updatedSinceDate.Date.AddDays(1)
        };

        var mockDbSet = MockDbSetSetup.CreateQueryableMockDbSet(new List<Apprentice> { apprentice });

        mockApprenticeContext
            .Setup(x => x.Entities)
            .Returns(mockDbSet.Object);

        var handler = new SyncApprenticeAccountHandler(logger.Object, mockApprenticeContext.Object);

        var result = await handler.Handle(query, CancellationToken.None);

        var expectedResult = new ApprenticeSyncDto()
        {
            ApprenticeID = apprenticeId,
            FirstName = firstName,
            LastName = surname,
            Email = email.Address,
            LastUpdatedDate = updatedSinceDate.Date.AddDays(1),
            DateOfBirth = dateOfBirth
        };

        result.Apprentices.Should().ContainSingle();
        result.Apprentices.First().Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task WithFutureDateThenEmptyArrayReturned(
            [Frozen] Mock<ILogger<SyncApprenticeAccountHandler>> logger,
            [Frozen] Mock<IApprenticeContext> mockApprenticeContext,
            [Frozen] Guid apprenticeId,
            [Frozen] string firstName,
            [Frozen] string surname,
            [Frozen] MailAddress email,
            [Frozen] DateTime dateOfBirth,
            [Frozen] DateTime updatedSinceDate

        )
    {
        SyncApprenticeAccountQuery query = new SyncApprenticeAccountQuery(updatedSinceDate, new Guid[] { apprenticeId });

        var apprentice = new Apprentice(
            apprenticeId,
            firstName,
            surname,
            email,
            dateOfBirth
        )
        {
            UpdatedOn = updatedSinceDate.Date.AddDays(-1)
        };

        var mockDbSet = MockDbSetSetup.CreateQueryableMockDbSet(new List<Apprentice> { apprentice });

        mockApprenticeContext
            .Setup(x => x.Entities)
            .Returns(mockDbSet.Object);

        var handler = new SyncApprenticeAccountHandler(logger.Object, mockApprenticeContext.Object);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Apprentices.Should().BeEmpty();
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task WhenNullRequestEmptyArrayReturned(
            [Frozen] Mock<ILogger<SyncApprenticeAccountHandler>> logger,
            [Frozen] Mock<IApprenticeContext> mockApprenticeContext
        )
    {
        var handler = new SyncApprenticeAccountHandler(logger.Object, mockApprenticeContext.Object);

        var result = await handler.Handle(null, CancellationToken.None);

        result.Apprentices.Should().BeEmpty();
    }
}