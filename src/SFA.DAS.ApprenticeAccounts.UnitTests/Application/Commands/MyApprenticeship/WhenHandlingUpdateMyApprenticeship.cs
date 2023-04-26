using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateMyApprenticeCommand;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Application.Commands.MyApprenticeship;

public class WhenHandlingUpdateMyApprenticeship
{
    private Mock<IMyApprenticeshipContext> _mockMyApprenticeshipContext;
    private UpdateMyApprenticeshipCommand _mockCommand;

    [SetUp]
    public void SetUp()
    {
        _mockMyApprenticeshipContext = new Mock<IMyApprenticeshipContext>();
        _mockCommand = new UpdateMyApprenticeshipCommand();
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task AndRecordIsUpdated()
    {
        _mockMyApprenticeshipContext.Setup(x => x.Entities.Update(It.IsAny<Data.Models.MyApprenticeship>())); 
        _mockMyApprenticeshipContext
           .Setup(x => x.FindByApprenticeIdMyApprenticeshipId(It.IsAny<Guid>(), It.IsAny<Guid>()))
           .ReturnsAsync(new Data.Models.MyApprenticeship());

        var handler = new UpdateMyApprenticeshipCommandHandler(_mockMyApprenticeshipContext.Object);
        var result = await handler.Handle(_mockCommand, CancellationToken.None);
        result.GetType().Should().Be(typeof(Unit));
        _mockMyApprenticeshipContext.Verify(x => x.Update(It.IsAny<Data.Models.MyApprenticeship>()), Times.Once);
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task AndNoResultSoNoRecordIsUpdated()
    {
        var handler = new UpdateMyApprenticeshipCommandHandler(_mockMyApprenticeshipContext.Object);
        var result = await handler.Handle(_mockCommand, CancellationToken.None);
        result.GetType().Should().Be(typeof(Unit));
        _mockMyApprenticeshipContext.Verify(x=>x.Update(It.IsAny<Data.Models.MyApprenticeship>()),Times.Never);
    }
}
