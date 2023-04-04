using AutoFixture.NUnit3;
using FluentAssertions;
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
        [Greedy] ApprenticesController sut,
        CreateMyApprenticeshipRequest model,
        Guid apprenticeId)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<CreateMyApprenticeshipCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Unit.Task);

        var result = await sut.PostMyApprenticeship(apprenticeId, model);

        (result as NoContentResult).Should().NotBeNull();

        mediatorMock.Verify(m => m.Send(It.Is<CreateMyApprenticeshipCommand>(c =>
                c.ApprenticeshipId == model.ApprenticeshipId
                && c.EmployerName == model.EmployerName
                && c.StartDate == model.StartDate
                && c.EndDate == model.EndDate
                && c.StandardUId == model.StandardUId
                && c.TrainingCode == model.TrainingCode
                && c.TrainingProviderId == model.TrainingProviderId
                && c.TrainingProviderName == model.TrainingProviderName
                && c.Uln == model.Uln
                && c.ApprenticeId == apprenticeId
            ), It.IsAny<CancellationToken>()));
    }
}
