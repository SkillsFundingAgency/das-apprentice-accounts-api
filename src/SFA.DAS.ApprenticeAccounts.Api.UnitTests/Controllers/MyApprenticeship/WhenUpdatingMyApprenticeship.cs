using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Api.Controllers;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateMyApprenticeCommand;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Api.UnitTests.Controllers.MyApprenticeship;
public class WhenUpdatingMyApprenticeship
{
    [Test, MoqAutoData]
    public async Task UpdateMyApprenticeship_InvokesRequest(
        Mock<IMediator> mediatorMock,
        UpdateMyApprenticeshipRequest request,
        Guid apprenticeId,
        Guid myApprenticeshipId)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<UpdateMyApprenticeshipCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Unit.Task);

        var sut = new MyApprenticeshipController(mediatorMock.Object);
        var result = await sut.UpdateMyApprenticeship (apprenticeId, myApprenticeshipId, request) as NoContentResult;

        result!.StatusCode.Should().Be(StatusCodes.Status204NoContent);
  
        mediatorMock.Verify(m => m.Send(It.Is<UpdateMyApprenticeshipCommand>(c =>
            c.ApprenticeshipId == request.ApprenticeshipId
            && c.EmployerName == request.EmployerName
            && c.StartDate == request.StartDate
            && c.EndDate == request.EndDate
            && c.StandardUId == request.StandardUId
            && c.TrainingCode == request.TrainingCode
            && c.TrainingProviderId == request.TrainingProviderId
            && c.TrainingProviderName == request.TrainingProviderName
            && c.Uln == request.Uln
            && c.ApprenticeId == apprenticeId
            && c.MyApprenticeshipId == myApprenticeshipId
        ), It.IsAny<CancellationToken>()));
    }
}
