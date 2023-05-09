using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateMyApprenticeshipCommand;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Application.Commands.MyApprenticeship;
[TestFixture]
public class UpdateMyApprenticeshipCommandValidatorTests
{
    private Mock<IApprenticeContext> _mockApprenticeContext = new();
    private Mock<IMyApprenticeshipContext> _mockMyApprenticeshipContext = new();

    [Test]
    public async Task ApprenticeId_NotPresent()
    {
        _mockApprenticeContext = new Mock<IApprenticeContext>();
        _mockMyApprenticeshipContext = new Mock<IMyApprenticeshipContext>();
        _mockApprenticeContext.Setup(x => x.Find(It.IsAny<Guid>())).ReturnsAsync((Apprentice)null);
       
        var validator = new UpdateMyApprenticeshipCommandValidator(_mockApprenticeContext.Object, _mockMyApprenticeshipContext.Object);
        var command = new UpdateMyApprenticeshipCommand { ApprenticeId = Guid.NewGuid() };
        var result = await validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.ApprenticeId)
            .WithErrorMessage(UpdateMyApprenticeshipCommandValidator.ApprenticeIdNotPresent);
    }

    [Test]
    public async Task ApprenticeshipId_MyApprenticeshipNotPresent()
    {
        var apprenticeId = Guid.NewGuid();
        const int apprenticeshipId = 1234;
        _mockApprenticeContext = new Mock<IApprenticeContext>();
        _mockMyApprenticeshipContext = new Mock<IMyApprenticeshipContext>();
        _mockApprenticeContext.Setup(x => x.Find(It.IsAny<Guid>())).ReturnsAsync(new Apprentice(apprenticeId, "first name", "last name", new MailAddress("test@test.com"), DateTime.Now));

        _mockMyApprenticeshipContext.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync((Data.Models.MyApprenticeship)null);

        var validator = new UpdateMyApprenticeshipCommandValidator(_mockApprenticeContext.Object, _mockMyApprenticeshipContext.Object);
        var command = new UpdateMyApprenticeshipCommand { ApprenticeId = apprenticeId, ApprenticeshipId = apprenticeshipId };
        var result = await validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.ApprenticeId)
            .WithErrorMessage(UpdateMyApprenticeshipCommandValidator.MyApprenticeshipNotPresentForApprenticeId);
    }

    [Test]
    public async Task ApprenticeshipId_MyApprenticeshipNotPresentButApprenticeIdNotSetHasPrecedence()
    {
        var apprenticeId = Guid.NewGuid();
        const int apprenticeshipId = 1234;
        _mockApprenticeContext = new Mock<IApprenticeContext>();
        _mockMyApprenticeshipContext = new Mock<IMyApprenticeshipContext>();
        _mockApprenticeContext.Setup(x => x.Find(It.IsAny<Guid>())).ReturnsAsync((Apprentice)null);

        _mockMyApprenticeshipContext.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync((Data.Models.MyApprenticeship)null);

        var validator = new UpdateMyApprenticeshipCommandValidator(_mockApprenticeContext.Object, _mockMyApprenticeshipContext.Object);
        var command = new UpdateMyApprenticeshipCommand { ApprenticeId = apprenticeId, ApprenticeshipId = apprenticeshipId };
        var result = await validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.ApprenticeId)
            .WithErrorMessage(UpdateMyApprenticeshipCommandValidator.ApprenticeIdNotPresent);
    }

    [Test]
    public async Task ApprenticeshipId_Validation_AlreadyExists()
    {
        var apprenticeId = Guid.NewGuid();
        var otherApprenticeId = Guid.NewGuid();
        var apprenticeshipId = 1234;
        _mockApprenticeContext = new Mock<IApprenticeContext>();
        _mockMyApprenticeshipContext = new Mock<IMyApprenticeshipContext>();
        _mockApprenticeContext.Setup(x => x.Find(apprenticeId)).ReturnsAsync(new Apprentice(apprenticeId, "first name", "last name", new MailAddress("test@test.com"), DateTime.Now));
        _mockApprenticeContext.Setup(x => x.Find(otherApprenticeId)).ReturnsAsync(new Apprentice(otherApprenticeId, "first name", "last name", new MailAddress("test@test.com"), DateTime.Now));

        _mockMyApprenticeshipContext.Setup(x => x.FindByApprenticeId(apprenticeId))
            .ReturnsAsync(new Data.Models.MyApprenticeship { ApprenticeshipId = apprenticeshipId, ApprenticeId = apprenticeId });
        _mockMyApprenticeshipContext.Setup(x => x.FindByApprenticeshipId(apprenticeshipId))
            .ReturnsAsync(new Data.Models.MyApprenticeship { ApprenticeshipId = apprenticeshipId, ApprenticeId = otherApprenticeId });

        var validator = new UpdateMyApprenticeshipCommandValidator(_mockApprenticeContext.Object, _mockMyApprenticeshipContext.Object);
        var command = new UpdateMyApprenticeshipCommand { ApprenticeId = apprenticeId, ApprenticeshipId = apprenticeshipId };
        var result = await validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.ApprenticeshipId)
            .WithErrorMessage(UpdateMyApprenticeshipCommandValidator.ApprenticeshipIdAlreadyExists);
    }

    [Test]
    public async Task ApprenticeshipId_Validation_AlreadyExists_ButApprenticeIdNotSetHasPrecedence()
    {
        var apprenticeId = Guid.NewGuid();
        var otherApprenticeId = Guid.NewGuid();
        var apprenticeshipId = 1234;
        _mockApprenticeContext = new Mock<IApprenticeContext>();
        _mockMyApprenticeshipContext = new Mock<IMyApprenticeshipContext>();
        _mockApprenticeContext.Setup(x => x.Find(apprenticeId)).ReturnsAsync((Apprentice)null);
        _mockApprenticeContext.Setup(x => x.Find(otherApprenticeId)).ReturnsAsync(new Apprentice(otherApprenticeId, "first name", "last name", new MailAddress("test@test.com"), DateTime.Now));

        _mockMyApprenticeshipContext.Setup(x => x.FindByApprenticeId(apprenticeId))
            .ReturnsAsync(new Data.Models.MyApprenticeship { ApprenticeshipId = apprenticeshipId, ApprenticeId = apprenticeId });
        _mockMyApprenticeshipContext.Setup(x => x.FindByApprenticeshipId(apprenticeshipId))
            .ReturnsAsync(new Data.Models.MyApprenticeship { ApprenticeshipId = apprenticeshipId, ApprenticeId = otherApprenticeId });

        var validator = new UpdateMyApprenticeshipCommandValidator(_mockApprenticeContext.Object, _mockMyApprenticeshipContext.Object);
        var command = new UpdateMyApprenticeshipCommand { ApprenticeId = apprenticeId, ApprenticeshipId = apprenticeshipId };
        var result = await validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.ApprenticeId)
            .WithErrorMessage(UpdateMyApprenticeshipCommandValidator.ApprenticeIdNotPresent);

        result.Errors.Count.Should().Be(1);
    }

    [Test]
    public async Task ApprenticeshipIdNull_ApprenticeshipIdNullAlreadyPresent_ValidationPasses()
    {
        var apprenticeId = Guid.NewGuid();
        _mockApprenticeContext = new Mock<IApprenticeContext>();
        _mockMyApprenticeshipContext = new Mock<IMyApprenticeshipContext>();
        _mockApprenticeContext.Setup(x => x.Find(It.IsAny<Guid>())).ReturnsAsync(new Apprentice(apprenticeId, "first name", "last name", new MailAddress("test@test.com"), DateTime.Now));
    
        var validator = new UpdateMyApprenticeshipCommandValidator(_mockApprenticeContext.Object, _mockMyApprenticeshipContext.Object);
        var command = new UpdateMyApprenticeshipCommand { ApprenticeId = apprenticeId, ApprenticeshipId = null };
        var result = await validator.TestValidateAsync(command);
    
        result.ShouldNotHaveValidationErrorFor(c => c.ApprenticeshipId);
    }
}
