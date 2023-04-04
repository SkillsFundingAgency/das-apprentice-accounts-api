using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Application.Commands.MyApprenticeship;
public class WhenHandlingCreateMyApprenticeship
{
    private Mock<IMyApprenticeshipContext> _mockMyApprenticeshipContext;
    private CreateMyApprenticeshipCommand _mockCommand;

    [SetUp]
    public void SetUp()
    {
        _mockMyApprenticeshipContext = new Mock<IMyApprenticeshipContext>();
        _mockCommand = new CreateMyApprenticeshipCommand();
    }



    [Test]
    [RecursiveMoqAutoData]
    public async Task AndRecordIsCreated()
    {
        _mockMyApprenticeshipContext.Setup(x => x.Entities.Add(It.IsAny<Data.Models.MyApprenticeship>()));
        var handler = new CreateMyApprenticeshipCommandHandler(_mockMyApprenticeshipContext.Object);
        var result = await handler.Handle(_mockCommand, CancellationToken.None);
        result.GetType().Should().Be(typeof(Unit));
    }
}
