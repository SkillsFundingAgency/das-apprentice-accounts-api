using AutoFixture.NUnit3;
using FluentAssertions;
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
            [Frozen] Mock<IApprenticeContext> mockApprenticeContext,
            GetApprenticesHandler handler,
            Apprentice apprentice
        )
    {
        GetApprenticesQuery query = new GetApprenticesQuery(null, new Guid[] { apprentice.Id });

        mockApprenticeContext
            .Setup(x => x.GetForSync(query.ApprenticeIds, query.UpdatedSince))
            .ReturnsAsync(new Apprentice[] { apprentice });

        var result = await handler.Handle(query, CancellationToken.None);

        result.Apprentices.Should().ContainSingle();

        var apprenticeResult = result.Apprentices.First();

        apprenticeResult.ApprenticeID.Should().Be(apprentice.Id);
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task AndEmptyApprenticeIdArray_ThenReturnsEmptyArray(
            [Frozen] Mock<IApprenticeContext> mockApprenticeContext
        )
    {
        var handler = new GetApprenticesHandler(mockApprenticeContext.Object);

        GetApprenticesQuery query = new GetApprenticesQuery(null, Array.Empty<Guid>());

        var result = await handler.Handle(query, CancellationToken.None);

        result.Apprentices.Should().BeEmpty();
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task AndFutureDate_ThenExpectedArrayOfApprenticeSyncDtoReturned(
            [Frozen] Mock<IApprenticeContext> mockApprenticeContext,
            GetApprenticesHandler handler,
            Apprentice apprentice
        )
    {
        GetApprenticesQuery query = new GetApprenticesQuery(apprentice.UpdatedOn, new Guid[] { apprentice.Id });

        apprentice.UpdatedOn = apprentice.UpdatedOn.Date.AddDays(1);

        mockApprenticeContext
            .Setup(x => x.GetForSync(query.ApprenticeIds, query.UpdatedSince))
            .ReturnsAsync(new Apprentice[] { apprentice });

        var result = await handler.Handle(query, CancellationToken.None);

        var expectedResult =  new ApprenticeSyncDto()
        {
            ApprenticeID = apprentice.Id,
            FirstName = apprentice.FirstName,
            LastName = apprentice.LastName,
            Email = apprentice.Email.Address,
            LastUpdatedDate = apprentice.UpdatedOn,
            DateOfBirth = apprentice.DateOfBirth
        };

        result.Apprentices.Should().ContainSingle();

        result.Apprentices.First().Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task AndFutureDate_ThenEmptyArrayReturned(
            [Frozen] Mock<IApprenticeContext> mockApprenticeContext,
            GetApprenticesHandler handler,
            Apprentice apprentice
        )
    {
        GetApprenticesQuery query = new GetApprenticesQuery(apprentice.UpdatedOn, new Guid[] { apprentice.Id });

        apprentice.UpdatedOn = apprentice.UpdatedOn.Date.AddDays(-1);

        mockApprenticeContext
            .Setup(x => x.GetForSync(query.ApprenticeIds, query.UpdatedSince))
            .ReturnsAsync(Array.Empty<Apprentice>());

        var result = await handler.Handle(query, CancellationToken.None);

        result.Apprentices.Should().BeEmpty();
    }

    [Test]
    [RecursiveMoqAutoData]
    public void AndNullRequest_ThenThrowsArgumentNullException(
            Mock<IApprenticeContext> mockApprenticeContext
        )
    {
        var handler = new GetApprenticesHandler(mockApprenticeContext.Object);

        var exception = Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(null, CancellationToken.None));
    }
}