using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Api.Controllers;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateMyApprenticeshipCommand;
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
        var result = await sut.UpdateMyApprenticeship(apprenticeId, request) as NoContentResult;

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
        ), It.IsAny<CancellationToken>()));
    }

    [Test, MoqAutoData]
    public async Task UpdateMyApprenticeshipApprenticeIdNotPresent_ReturnsNotFound(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MyApprenticeshipController sut,
        UpdateMyApprenticeshipRequest request,
        Guid apprenticeId)
    {
        var exception = new ValidationException(UpdateMyApprenticeshipCommandValidator.ApprenticeIdNotPresent);
        mediatorMock.Setup(m => m.Send(It.IsAny<UpdateMyApprenticeshipCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Unit.Task);

        mediatorMock.Setup(m =>
                m.Send(It.IsAny<UpdateMyApprenticeshipCommand>(), It.IsAny<CancellationToken>()))
            .Throws(exception);

        var result = await sut.UpdateMyApprenticeship(apprenticeId, request) as ActionResult;

        result.Should().NotBeNull();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test, MoqAutoData]
    public async Task UpdateMyApprenticeshipMyApprenticeshipIdAlreadyExists_ReturnsBadRequest(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MyApprenticeshipController sut,
        UpdateMyApprenticeshipRequest request,
        Guid apprenticeId)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<UpdateMyApprenticeshipCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Unit.Task);

        mediatorMock.Setup(m =>
                m.Send(It.IsAny<UpdateMyApprenticeshipCommand>(), It.IsAny<CancellationToken>()))
            .Throws(new ValidationException(UpdateMyApprenticeshipCommandValidator.ApprenticeshipIdAlreadyExists));

        var result = await sut.UpdateMyApprenticeship(apprenticeId, request) as ActionResult;

        result.Should().NotBeNull();
        result.Should().BeOfType<BadRequestObjectResult>();
    }
}
