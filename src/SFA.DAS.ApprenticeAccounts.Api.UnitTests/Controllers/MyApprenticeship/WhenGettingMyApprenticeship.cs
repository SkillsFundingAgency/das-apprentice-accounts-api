using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Api.Controllers;
using SFA.DAS.ApprenticeAccounts.Application.Queries.MyApprenticeshipQuery;
using SFA.DAS.ApprenticeAccounts.Data.Models;
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
          ApprenticeWithMyApprenticeshipDto response,
          Guid myApprenticeshipId,
          Guid apprenticeId)
      {
          mediatorMock.Setup(m => m.Send(It.IsAny<MyApprenticeshipQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(response);
    
          var result = await controller.GetMyApprenticeship(apprenticeId) as OkObjectResult;
    
          result.Should().NotBeNull();
          var model = result.Value;
    
          model.Should().BeEquivalentTo(response.MyApprenticeship);
    
          mediatorMock.Verify(m => m.Send(It.Is<MyApprenticeshipQuery>(c =>
              c.ApprenticeId == apprenticeId
          ), It.IsAny<CancellationToken>()));
      }
    
    [Test]
    [MoqAutoData]
    public async Task AndMediatorCommandReturnsNullApprentice_ThenReturnBadRequest(
        [Frozen] Mock<IMediator> mediator,
        [Greedy] MyApprenticeshipController controller)
    {
        mediator.Setup(m => m.Send(It.IsAny<MyApprenticeshipQuery>(),
            CancellationToken.None)).ReturnsAsync(new ApprenticeWithMyApprenticeshipDto());
    
        var result = await controller.GetMyApprenticeship(Guid.NewGuid());
        result.Should().BeOfType<BadRequestResult>();
    }
    
    [Test]
    [MoqAutoData]
    public async Task AndMediatorCommandReturnsNullMyApprenticeship_ThenReturnNotFound(
        [Frozen] Mock<IMediator> mediator,
        [Greedy] MyApprenticeshipController controller,
        [Frozen] Apprentice  apprentice)
    {
        mediator.Setup(m => m.Send(It.IsAny<MyApprenticeshipQuery>(),
            CancellationToken.None)).ReturnsAsync(new ApprenticeWithMyApprenticeshipDto {Apprentice = apprentice});
    
        var result = await controller.GetMyApprenticeship(Guid.NewGuid());
        result.Should().BeOfType<NotFoundResult>();
        
    
    }
}
