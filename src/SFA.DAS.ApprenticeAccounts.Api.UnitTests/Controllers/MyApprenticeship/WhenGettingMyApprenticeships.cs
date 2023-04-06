using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Transaction;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Api.Controllers;
using SFA.DAS.ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand;
using SFA.DAS.ApprenticeAccounts.Application.Queries.GetApprenticePreferenceForApprenticeAndPreferenceQuery;
using SFA.DAS.ApprenticeAccounts.Application.Queries.MyApprenticeshipsQuery;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences.GetApprenticePreferenceForApprenticeAndPreference;
using SFA.DAS.ApprenticeAccounts.DTOs.MyApprenticeships;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Api.UnitTests.Controllers.MyApprenticeship;
public class WhenGettingMyApprenticeships
{

    [Test, MoqAutoData]
    public async Task AndMediatorCommandSuccessful_ThenReturnOk(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MyApprenticeshipsController controller,
        [Greedy]ApprenticeWithMyApprenticeshipsDto response,
        Guid apprenticeId)
    {
        response.ApprenticeId = apprenticeId;
        mediatorMock.Setup(m => m.Send(It.IsAny<MyApprenticeshipsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await controller.GetMyApprenticeship(apprenticeId) as OkObjectResult;

        result.Should().NotBeNull();

        var model = result.Value;

        model.Should().BeEquivalentTo(response);

        mediatorMock.Verify(m => m.Send(It.Is<MyApprenticeshipsQuery>(c =>
            c.ApprenticeId == apprenticeId
        ), It.IsAny<CancellationToken>()));
    }

    [Test]
    [MoqAutoData]
    public async Task AndMediatorCommandReturnsNull_ThenReturnNotFound(
        [Frozen] Mock<IMediator> mediator,
        [Greedy] MyApprenticeshipsController controller)
    {
        mediator.Setup(m => m.Send(It.IsAny<MyApprenticeshipsQuery>(),
            CancellationToken.None)).ReturnsAsync((ApprenticeWithMyApprenticeshipsDto)null);

        var result = await controller.GetMyApprenticeship(Guid.NewGuid());
        result.Should().BeOfType<NotFoundResult>();
    }
}
