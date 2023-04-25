using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateMyApprenticeCommand;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.Testing.AutoFixture;
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
        var handler = new UpdateMyApprenticeshipCommandHandler(_mockMyApprenticeshipContext.Object);
        var result = await handler.Handle(_mockCommand, CancellationToken.None);
        result.GetType().Should().Be(typeof(Unit));
    }
}
