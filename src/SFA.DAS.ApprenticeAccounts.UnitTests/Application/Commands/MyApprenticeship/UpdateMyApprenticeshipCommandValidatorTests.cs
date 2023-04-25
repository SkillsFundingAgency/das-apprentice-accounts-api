using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateMyApprenticeCommand;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System;
using System.Collections.Generic;
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
        _mockMyApprenticeshipContext.Setup(x => x.FindAll(It.IsAny<Guid>())).ReturnsAsync(new List<Data.Models.MyApprenticeship>());
    
        var validator= new UpdateMyApprenticeshipCommandValidator(_mockApprenticeContext.Object, _mockMyApprenticeshipContext.Object);
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
    
        result.ShouldHaveValidationErrorFor(c => c.MyApprenticeshipId)
            .WithErrorMessage(UpdateMyApprenticeshipCommandValidator.MyApprenticeshipIdNotPresentForApprenticeId);
    }

    [Test]
    public async Task MyApprenticeshipId_AgainstDifferentApprenticeId()
    {
        var apprenticeId = Guid.NewGuid();
        const int apprenticeshipId = 1234;
        var otherApprenticeId = Guid.NewGuid();
        _mockApprenticeContext = new Mock<IApprenticeContext>();
        _mockMyApprenticeshipContext = new Mock<IMyApprenticeshipContext>();
        _mockApprenticeContext.Setup(x => x.Find(It.IsAny<Guid>())).ReturnsAsync(new Apprentice(apprenticeId, "first name", "last name", new MailAddress("test@test.com"), DateTime.Now));

        _mockMyApprenticeshipContext.Setup(x => x.FindById(It.IsAny<Guid>())).ReturnsAsync(new Data.Models.MyApprenticeship {ApprenticeshipId = apprenticeshipId,ApprenticeId = otherApprenticeId});

        var validator = new UpdateMyApprenticeshipCommandValidator(_mockApprenticeContext.Object, _mockMyApprenticeshipContext.Object);
        var command = new UpdateMyApprenticeshipCommand { ApprenticeId = apprenticeId, ApprenticeshipId = apprenticeshipId };
        var result = await validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.MyApprenticeshipId)
            .WithErrorMessage(UpdateMyApprenticeshipCommandValidator.MyApprenticeshipIdNotPresentForApprenticeId);
    }
    
    [Test]
    public async Task ApprenticeshipIdAlreadyPresent()
    {
        var apprenticeId = Guid.NewGuid();
        const int apprenticeshipId = 1234;
        var myApprenticeshipId = Guid.NewGuid();
        _mockApprenticeContext = new Mock<IApprenticeContext>();
        _mockMyApprenticeshipContext = new Mock<IMyApprenticeshipContext>();
     
        _mockMyApprenticeshipContext.Setup(x => x.FindAll(apprenticeId)).ReturnsAsync(
            new List<Data.Models.MyApprenticeship>
            {
                new(apprenticeId: apprenticeId, apprenticeshipId: apprenticeshipId, id: myApprenticeshipId)
            });
    
        var validator = new UpdateMyApprenticeshipCommandValidator(_mockApprenticeContext.Object, _mockMyApprenticeshipContext.Object);
        var command = new UpdateMyApprenticeshipCommand { ApprenticeId = apprenticeId, ApprenticeshipId = apprenticeshipId };
        var result = await validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.ApprenticeshipId)
            .WithErrorMessage(UpdateMyApprenticeshipCommandValidator.ApprenticeshipIdAlreadyExists);
    }
}
