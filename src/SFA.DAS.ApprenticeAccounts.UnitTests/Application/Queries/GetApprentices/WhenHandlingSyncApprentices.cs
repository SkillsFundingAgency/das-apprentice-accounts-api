using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Queries.ApprenticesQuery;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Application.Queries.GetApprentices;
public class WhenHandlingSyncApprentices
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task AndNullDate_ThenExpectedArrayOfApprenticeSyncDtoReturned(
            [Frozen] Mock<ILogger<GetApprenticesHandler>> logger,
            [Frozen] Mock<IApprenticeContext> mockApprenticeContext,
            [Frozen] Guid apprenticeId,
            [Frozen] string firstName,
            [Frozen] string surname,
            [Frozen] MailAddress email,
            [Frozen] DateTime dateOfBirth,
            [Frozen] DateTime updatedOn
        )
    {
        GetApprenticesQuery query = new GetApprenticesQuery(null, new Guid[] { apprenticeId });

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

        mockApprenticeContext
            .Setup(x => x.GetForSync(query.ApprenticeIds, query.UpdatedSince))
            .ReturnsAsync(new Apprentice[] { apprentice });

        var handler = new GetApprenticesHandler(logger.Object, mockApprenticeContext.Object);

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
    public async Task AndEmptyApprenticeIdArray_ThenReturnsEmptyArray(
            [Frozen] Mock<ILogger<GetApprenticesHandler>> logger,
            [Frozen] Mock<IApprenticeContext> mockApprenticeContext
        )
    {
        var handler = new GetApprenticesHandler(logger.Object, mockApprenticeContext.Object);

        GetApprenticesQuery query = new GetApprenticesQuery(null, Array.Empty<Guid>());

        var result = await handler.Handle(query, CancellationToken.None);

        result.Apprentices.Should().BeEmpty();
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task AndFutureDate_ThenExpectedArrayOfApprenticeSyncDtoReturned(
            [Frozen] Mock<ILogger<GetApprenticesHandler>> logger,
            [Frozen] Mock<IApprenticeContext> mockApprenticeContext,
            [Frozen] Guid apprenticeId,
            [Frozen] string firstName,
            [Frozen] string surname,
            [Frozen] MailAddress email,
            [Frozen] DateTime dateOfBirth,
            [Frozen] DateTime updatedSinceDate
        )
    {
        GetApprenticesQuery query = new GetApprenticesQuery(updatedSinceDate, new Guid[] { apprenticeId });

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

        mockApprenticeContext
            .Setup(x => x.GetForSync(query.ApprenticeIds, query.UpdatedSince))
            .ReturnsAsync(new Apprentice[] { apprentice });

        var handler = new GetApprenticesHandler(logger.Object, mockApprenticeContext.Object);

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
    public async Task AndFutureDate_ThenEmptyArrayReturned(
            [Frozen] Mock<ILogger<GetApprenticesHandler>> logger,
            [Frozen] Mock<IApprenticeContext> mockApprenticeContext,
            [Frozen] Guid apprenticeId,
            [Frozen] string firstName,
            [Frozen] string surname,
            [Frozen] MailAddress email,
            [Frozen] DateTime dateOfBirth,
            [Frozen] DateTime updatedSinceDate
        )
    {
        GetApprenticesQuery query = new GetApprenticesQuery(updatedSinceDate, new Guid[] { apprenticeId });

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

        mockApprenticeContext
            .Setup(x => x.GetForSync(query.ApprenticeIds, query.UpdatedSince))
            .ReturnsAsync(Array.Empty<Apprentice>());

        var handler = new GetApprenticesHandler(logger.Object, mockApprenticeContext.Object);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Apprentices.Should().BeEmpty();
    }

    [Test]
    [RecursiveMoqAutoData]
    public void AndNullRequest_ThenThrowsArgumentNullException(
            [Frozen] Mock<ILogger<GetApprenticesHandler>> logger,
            [Frozen] Mock<IApprenticeContext> mockApprenticeContext
        )
    {
        var handler = new GetApprenticesHandler(logger.Object, mockApprenticeContext.Object);

        var exception = Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
    }
}