using AutoFixture.NUnit3;
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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Api.UnitTests.Controllers.MyApprenticeship;
public class WhenGettingMyApprenticeship
{
    [Test, MoqAutoData]
     public async Task AndMediatorCommandSuccessful_ThenReturnOk(
         [Frozen] Mock<IMediator> mediatorMock,
         [Greedy] MyApprenticeshipController controller,
         [Greedy]ApprenticeDto response,
         Guid apprenticeId)
     {
         response.ApprenticeId = apprenticeId;
         mediatorMock.Setup(m => m.Send(It.IsAny<MyApprenticeshipQuery>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(response);
    
         var result = await controller.GetMyApprenticeship(apprenticeId,null) as OkObjectResult;
    
         result.Should().NotBeNull();
    
         var model = result.Value;
    
         model.Should().BeEquivalentTo(response);
    
         mediatorMock.Verify(m => m.Send(It.Is<MyApprenticeshipQuery>(c =>
             c.ApprenticeId == apprenticeId
         ), It.IsAny<CancellationToken>()));
     }

     [Test, MoqAutoData]
     public async Task AndMediatorCommandSuccessfulWithApprenticeshipId_ThenReturnOk(
         [Frozen] Mock<IMediator> mediatorMock,
         [Greedy] MyApprenticeshipController controller,
         [Greedy] ApprenticeDto response,
         Guid apprenticeId,
         int apprenticeshipId)
     {
         response.ApprenticeId = apprenticeId;
         response.MyApprenticeships = new List<MyApprenticeshipsDto>
         {
             new MyApprenticeshipsDto {ApprenticeshipId = apprenticeshipId}

         };
         mediatorMock.Setup(m => m.Send(It.IsAny<MyApprenticeshipQuery>(), It.IsAny<CancellationToken>()))
             .ReturnsAsync(response);

         var result = await controller.GetMyApprenticeship(apprenticeId, apprenticeshipId) as OkObjectResult;

         result.Should().NotBeNull();

         var model = result.Value;

         model.Should().BeEquivalentTo(response);

         mediatorMock.Verify(m => m.Send(It.Is<MyApprenticeshipQuery>(c =>
             c.ApprenticeId == apprenticeId && c.ApprenticeshipId == apprenticeshipId
         ), It.IsAny<CancellationToken>()));
     }

    [Test]
     [MoqAutoData]
     public async Task AndMediatorCommandReturnsNull_ThenReturnNotFound(
         [Frozen] Mock<IMediator> mediator,
         [Greedy] MyApprenticeshipController controller)
     {
         mediator.Setup(m => m.Send(It.IsAny<MyApprenticeshipQuery>(),
             CancellationToken.None)).ReturnsAsync((ApprenticeDto)null);

         var result = await controller.GetMyApprenticeship(Guid.NewGuid(),null);
         result.Should().BeOfType<NotFoundResult>();
     }
}
