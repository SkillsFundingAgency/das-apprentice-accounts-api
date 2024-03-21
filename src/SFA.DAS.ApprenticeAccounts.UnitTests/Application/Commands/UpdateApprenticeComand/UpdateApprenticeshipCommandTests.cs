using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateApprenticeCommand;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Application.Commands.UpdateApprenticeComand;
public class UpdateApprenticeshipCommandTests
{
    private Mock<IApprenticeContext> _mockApprenticeContext;
    private UpdateApprenticeCommand _mockCommand;
    private Mock<ILogger<UpdateApprenticeCommandHandler>> _logger;

    [SetUp]
    public void SetUp()
    {
        _mockApprenticeContext = new Mock<IApprenticeContext>();
        _mockCommand = new UpdateApprenticeCommand(new Guid(), new JsonPatchDocument<ApprenticePatchDto>());
        _logger = new Mock<ILogger<UpdateApprenticeCommandHandler>>();
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task AndRecordIsUpdated()
    {
        _mockApprenticeContext
            .Setup(x => x.Find(It.IsAny<Guid>()))
            .ReturnsAsync(
                new Apprentice(
                    new Guid(),
                    "firstName",
                    "surname",
                    new MailAddress("test@email.com"),
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
                )
            );

        var handler = new UpdateApprenticeCommandHandler(_mockApprenticeContext.Object, _logger.Object);
        var result = await handler.Handle(_mockCommand, CancellationToken.None);
        result.GetType().Should().Be(typeof(bool));
        result.Should().BeTrue();
    }

    [Test]
    public async Task And_ApprenticeIsNull_ReturnFalse()
    {
        _mockApprenticeContext
            .Setup(x => x.Find(It.IsAny<Guid>()))
            .ReturnsAsync((Apprentice)null);

        var handler = new UpdateApprenticeCommandHandler(_mockApprenticeContext.Object, _logger.Object);

        var result = await handler.Handle(_mockCommand, CancellationToken.None);

        result.Should().BeFalse();
    }
}
