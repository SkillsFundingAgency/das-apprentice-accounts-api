﻿using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Api.Controllers;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateAllApprenticePreferencesCommand;
using SFA.DAS.ApprenticeAccounts.Exceptions;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Api.UnitTests.Controllers
{
    public class WhenPostingAllApprenticePreferences
    {
        [Test]
        [MoqAutoData]
        public async Task AndInvalidInputExceptionIsReturned_ThenReturnNotFound(
            [Frozen] Mock<IMediator> mediator,
            UpdateAllApprenticePreferencesCommand command,
            Guid apprenticeId)
        {
            mediator.Setup(m =>
                    m.Send(It.IsAny<UpdateAllApprenticePreferencesCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new InvalidInputException(It.IsAny<string>()));

            var controller = new ApprenticePreferencesController(mediator.Object);

            var result =
                await controller.UpdateAllApprenticePreferences(command, apprenticeId) as ActionResult;

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        [MoqAutoData]
        public async Task AndAnyOtherExceptionIsReturned_ThenReturnBadRequest(
            [Frozen] Mock<IMediator> mediator,
            UpdateAllApprenticePreferencesCommand command,
            Guid apprenticeId)
        {
            mediator.Setup(m =>
                    m.Send(It.IsAny<UpdateAllApprenticePreferencesCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            var controller = new ApprenticePreferencesController(mediator.Object);
            var result = await controller.UpdateAllApprenticePreferences(command, apprenticeId) as ActionResult;

            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        [MoqAutoData]
        public async Task AndMediatorCommandIsSuccessful_ThenReturnOk(
            [Greedy] ApprenticePreferencesController controller,
            [Frozen] Mock<IMediator> mediator,
            UpdateAllApprenticePreferencesCommand command,
            Guid apprenticeId)
        {
            mediator.Setup(m =>
                    m.Send(It.IsAny<UpdateAllApprenticePreferencesCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Unit.Task);

            var result = await controller.UpdateAllApprenticePreferences(command, apprenticeId);

            result.Should().BeOfType(typeof(OkResult));
        }
    }
}