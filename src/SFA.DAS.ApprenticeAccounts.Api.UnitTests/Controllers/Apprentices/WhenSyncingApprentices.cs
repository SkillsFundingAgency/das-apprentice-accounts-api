using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Api.Controllers;
using SFA.DAS.ApprenticeAccounts.Application.Queries.ApprenticesQuery;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Api.UnitTests.Controllers.Apprentices;
public class WhenSyncingApprentices
{
    [Test]
    [MoqAutoData]
    public async Task AndMediatorCommandSuccessful_ThenReturnOk(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ApprenticesController controller,
        GetApprenticesQuery query
    )
    {
        mediatorMock.Setup(m =>
            m.Send(
                It.IsAny<GetApprenticesQuery>(),
                It.IsAny<CancellationToken>())
        ).ReturnsAsync(new ApprenticeSyncResponseDto());

        var result = await controller.SyncApprentices(query.ApprenticeIds, query.UpdatedSince) as OkObjectResult;

        result.Should().NotBeNull();
        result.Value.Should().BeOfType<ApprenticeSyncResponseDto>();
    }

    [Test]
    [MoqAutoData]
    public async Task AndMediatorCommandReturnsNullApprenticeArray_ThenReturnOkResult(
        [Frozen] Mock<IMediator> mediator,
        [Greedy] ApprenticesController controller)
    {
        mediator.Setup(m => m.Send(It.IsAny<GetApprenticesQuery>(),
            CancellationToken.None)).ReturnsAsync(new ApprenticeSyncResponseDto());

        var result = await controller.SyncApprentices(Array.Empty<Guid>(), DateTime.Now);
        result.Should().BeOfType<OkObjectResult>();
    }
}
