﻿using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Api.Controllers;
using SFA.DAS.ApprenticeAccounts.Application.Queries.MyApprenticeshipQuery;
using SFA.DAS.ApprenticeAccounts.DTOs.MyApprenticeship;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Api.UnitTests.Controllers.MyApprenticeship;
public class WhenGettingMyApprenticeship
{
    [Test, MoqAutoData]
      public async Task AndMediatorCommandSuccessful_ThenReturnOk(
          [Frozen] Mock<IMediator> mediatorMock,
          [Greedy] MyApprenticeshipController controller,
          MyApprenticeshipDto response,
          Guid myApprenticeshipId,
          Guid apprenticeId)
      {
          response.Id = myApprenticeshipId;
          mediatorMock.Setup(m => m.Send(It.IsAny<MyApprenticeshipQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(response);
    
          var result = await controller.GetMyApprenticeship(apprenticeId) as OkObjectResult;
    
          result.Should().NotBeNull();
          var model = result.Value;
    
          model.Should().BeEquivalentTo(response, l => l.Excluding(e => e.Id));
    
          mediatorMock.Verify(m => m.Send(It.Is<MyApprenticeshipQuery>(c =>
              c.ApprenticeId == apprenticeId
          ), It.IsAny<CancellationToken>()));
      }
    
    [Test]
    [MoqAutoData]
    public async Task AndMediatorCommandReturnsNull_ThenReturnNotFound(
        [Frozen] Mock<IMediator> mediator,
        [Greedy] MyApprenticeshipController controller)
    {
        mediator.Setup(m => m.Send(It.IsAny<MyApprenticeshipQuery>(),
            CancellationToken.None)).ReturnsAsync((MyApprenticeshipDto)null);
    
        var result = await controller.GetMyApprenticeship(Guid.NewGuid());
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    [MoqAutoData]
    public async Task AndMediatorCommandReturnsEmptyRecord_ThenReturnEmptyResult(
        [Frozen] Mock<IMediator> mediator,
        [Greedy] MyApprenticeshipController controller)
    {
        mediator.Setup(m => m.Send(It.IsAny<MyApprenticeshipQuery>(),
            CancellationToken.None)).ReturnsAsync(new MyApprenticeshipDto());

        var result = await controller.GetMyApprenticeship(Guid.NewGuid()) as OkObjectResult;
        result.Should().BeOfType<OkObjectResult>();
        result.Value.Should().BeOfType<EmptyResult>();

    }
}
