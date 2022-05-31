﻿using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Api.Controllers;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateApprenticePreferencesCommand;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Api.UnitTests.Controllers
{
    public class WhenPostingApprenticePreferences
    {
        [Test, MoqAutoData]
        public async Task AndInvalidOperationExceptionIsReturned_ThenReturnNotFound(
            [Frozen] Mock<IMediator> mediator,
            UpdateApprenticePreferencesCommand command)
        {
            mediator.Setup(m =>
                m.Send(It.IsAny<UpdateApprenticePreferencesCommand>(), It.IsAny<CancellationToken>())).Throws(new InvalidOperationException());

            var controller = new ApprenticePreferencesController(mediator.Object);

            var result =
                await controller.UpdateApprenticePreferences(command) as ActionResult;

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test, MoqAutoData]
        public async Task AndAnyOtherExceptionIsReturned_ThenReturnBadRequest(
            [Frozen] Mock<IMediator> mediator,
            UpdateApprenticePreferencesCommand command)
        {
            mediator.Setup(m =>
                    m.Send(It.IsAny<UpdateApprenticePreferencesCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            var controller = new ApprenticePreferencesController(mediator.Object);
            var result = await controller.UpdateApprenticePreferences(command) as ActionResult;

            result.Should().BeOfType<BadRequestResult>();
        }

        [Test, MoqAutoData]
        public async Task AndMediatorCommandIsSuccessful_ThenReturnOk(
            [Greedy] ApprenticePreferencesController controller,
            [Frozen] Mock<IMediator> mediator,
            UpdateApprenticePreferencesCommand command)
        {
            mediator.Setup(m =>
                    m.Send(It.IsAny<UpdateApprenticePreferencesCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Unit.Task);

            var result = await controller.UpdateApprenticePreferences(command);

            result.Should().BeOfType(typeof(OkResult));
        }
    }
}
