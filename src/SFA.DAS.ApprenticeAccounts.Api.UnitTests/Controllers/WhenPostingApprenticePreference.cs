using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Api.Controllers;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateApprenticePreferenceCommand;
using SFA.DAS.ApprenticeAccounts.Exceptions;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Api.UnitTests.Controllers
{
    public class WhenPostingApprenticePreference
    {
        [Test]
        [MoqAutoData]
        public async Task AndInvalidInputExceptionIsReturned_ThenReturnNotFound(
            [Frozen] Mock<IMediator> mediator,
            UpdateApprenticePreferenceCommand command)
        {
            mediator.Setup(m =>
                    m.Send(It.IsAny<UpdateApprenticePreferenceCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new InvalidInputException());

            var controller = new ApprenticePreferencesController(mediator.Object);

            var result =
                await controller.UpdateApprenticePreference(command.ApprenticeId, command.PreferenceId,
                    command.Status) as ActionResult;

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        [MoqAutoData]
        public async Task AndAnyOtherExceptionIsReturned_ThenReturnBadRequest(
            [Frozen] Mock<IMediator> mediator,
            UpdateApprenticePreferenceCommand command)
        {
            mediator.Setup(m =>
                    m.Send(It.IsAny<UpdateApprenticePreferenceCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            var controller = new ApprenticePreferencesController(mediator.Object);
            var result = await controller.UpdateApprenticePreference(command.ApprenticeId, command.PreferenceId,
                command.Status) as ActionResult;

            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        [MoqAutoData]
        public async Task AndMediatorCommandIsSuccessful_ThenReturnOk(
            [Greedy] ApprenticePreferencesController controller,
            [Frozen] Mock<IMediator> mediator,
            UpdateApprenticePreferenceCommand command)
        {
            mediator.Setup(m =>
                    m.Send(It.IsAny<UpdateApprenticePreferenceCommand>(), It.IsAny<CancellationToken>()))
                .Returns(Unit.Task);

            var result = await controller.UpdateApprenticePreference(command.ApprenticeId, command.PreferenceId,
                command.Status);

            result.Should().BeOfType(typeof(OkResult));
        }
    }
}