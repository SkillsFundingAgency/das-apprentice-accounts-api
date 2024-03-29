﻿using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Api.Controllers;
using SFA.DAS.ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Api.UnitTests.Controllers.MyApprenticeship;

public class WhenPostingMyApprenticeship
{
    [Test, MoqAutoData]
    public async Task CreateMyApprenticeship_InvokesRequest(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MyApprenticeshipController sut,
        CreateMyApprenticeshipRequest request,
        Guid apprenticeId)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<CreateMyApprenticeshipCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Unit.Task);

        var result = await sut.PostMyApprenticeship(apprenticeId, request) as CreatedResult;

        result.Should().NotBeNull(); result.Location.Should().Be($"apprentices/{apprenticeId}/MyApprenticeship");

        mediatorMock.Verify(m => m.Send(It.Is<CreateMyApprenticeshipCommand>(c =>
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
    public async Task CreateMyApprenticeshipApprenticeIdNotPresent_ReturnsNotFound(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MyApprenticeshipController sut,
        CreateMyApprenticeshipRequest request,
        Guid apprenticeId)
    {
        var exception = new ValidationException(CreateMyApprenticeshipCommandValidator.ApprenticeIdNotPresent);
        mediatorMock.Setup(m => m.Send(It.IsAny<CreateMyApprenticeshipCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Unit.Task);

        mediatorMock.Setup(m =>
                m.Send(It.IsAny<CreateMyApprenticeshipCommand>(), It.IsAny<CancellationToken>()))
            .Throws(exception);

        var result = await sut.PostMyApprenticeship(apprenticeId, request) as ActionResult;

        result.Should().NotBeNull();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test, MoqAutoData]
    public async Task CreateMyApprenticeshipApprenticeIdNotValid_ReturnsNotFound(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MyApprenticeshipController sut,
        CreateMyApprenticeshipRequest request,
        Guid apprenticeId)
    {
        var exception = new ValidationException(CreateMyApprenticeshipCommandValidator.ApprenticeIdNotValid);
        mediatorMock.Setup(m => m.Send(It.IsAny<CreateMyApprenticeshipCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Unit.Task);

        mediatorMock.Setup(m =>
                m.Send(It.IsAny<CreateMyApprenticeshipCommand>(), It.IsAny<CancellationToken>()))
            .Throws(exception);

        var result = await sut.PostMyApprenticeship(apprenticeId, request) as ActionResult;

        result.Should().NotBeNull();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test, MoqAutoData]
    public async Task CreateMyApprenticeshipMyApprenticeshipIdAlreadyExists_ReturnsBadRequest(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MyApprenticeshipController sut,
        CreateMyApprenticeshipRequest request,
        Guid apprenticeId)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<CreateMyApprenticeshipCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Unit.Task);

        mediatorMock.Setup(m =>
                m.Send(It.IsAny<CreateMyApprenticeshipCommand>(), It.IsAny<CancellationToken>()))
            .Throws(new ValidationException(CreateMyApprenticeshipCommandValidator.ApprenticeshipIdAlreadyPresent));

        var result = await sut.PostMyApprenticeship(apprenticeId, request) as ActionResult;

        result.Should().NotBeNull();
        result.Should().BeOfType<BadRequestObjectResult>();
    }
}
