using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Application.Commands.MyApprenticeship.CreateMyApprenticeshipCommandValidatorTests;

[TestFixture]
public class CreateMyApprenticeshipCommandValidatorTests
{
    private Mock<IApprenticeContext> _mockApprenticeContext = new();
    private Mock<IMyApprenticeshipContext> _mockMyApprenticeshipContext = new();

    [TestCase(0, true)]
    [TestCase(1, true)]
    [TestCase(200, true)]
    [TestCase(201, false)]
    public async Task EmployerName_Validation(int lengthOfString, bool isValid)
    {
        var employerName = new string('a', lengthOfString);
        var command = new ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand.CreateMyApprenticeshipCommand { EmployerName = employerName};

        var sut = GetValidator();

        var result = await sut.TestValidateAsync(command);
        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.EmployerName);
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.EmployerName)
                .WithErrorMessage(CreateMyApprenticeshipCommandValidator.EmployerNameTooLong);
        }
    }

    [TestCase(0, true)]
    [TestCase(1, true)]
    [TestCase(200, true)]
    [TestCase(201, false)]
    public async Task TrainingProviderName_Validation(int lengthOfString, bool isValid)
    {
        var trainingProviderName = new string('a', lengthOfString);
        var command = new ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand.CreateMyApprenticeshipCommand { EmployerName = "employer Name", TrainingProviderName = trainingProviderName};

        var sut = GetValidator();

        var result = await sut.TestValidateAsync(command);
        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.TrainingProviderName);
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.TrainingProviderName)
                .WithErrorMessage(CreateMyApprenticeshipCommandValidator.TrainingProviderNameTooLong);
        }
    }

    [TestCase(0, true)]
    [TestCase(1, true)]
    [TestCase(15, true)]
    [TestCase(16, false)]
    public async Task TrainingCode_Validation(int lengthOfString, bool isValid)
    {
        var trainingCode = new string('a', lengthOfString);
        var command = new ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand.CreateMyApprenticeshipCommand { TrainingCode = trainingCode };

        var sut = GetValidator();

        var result = await sut.TestValidateAsync(command);
        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.TrainingCode);
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.TrainingCode)
                .WithErrorMessage(CreateMyApprenticeshipCommandValidator.TrainingCodeTooLong);
        }
    }

    [TestCase(0, true)]
    [TestCase(1, true)]
    [TestCase(20, true)]
    [TestCase(21, false)]
    public async Task StandardUId_Validation(int lengthOfString, bool isValid)
    {
        var standardUId = new string('a', lengthOfString);
        var command = new ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand.CreateMyApprenticeshipCommand { StandardUId = standardUId };

        var sut = GetValidator();

        var result = await sut.TestValidateAsync(command);
        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.StandardUId);
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.StandardUId)
                .WithErrorMessage(CreateMyApprenticeshipCommandValidator.StandardUIdTooLong);
        }
    }

    [TestCase("00000000-0000-0000-0000-000000000000",false)]
    [TestCase("a06595aa-0681-4bd2-aa8a-62116bf535c4", true)]
    public async Task ApprenticeId_Validation(Guid apprenticeIdGuid, bool isValid)
    {
        var command = new ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand.CreateMyApprenticeshipCommand { ApprenticeId = apprenticeIdGuid};

        var sut = GetValidator();

        var result = await sut.TestValidateAsync(command);
        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.ApprenticeId);
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.ApprenticeId)
                .WithErrorMessage(CreateMyApprenticeshipCommandValidator.ApprenticeIdNotValid);
        }
    }

    [Test]
    public async Task ApprenticeId_Validation_NotPresent()
    {
        _mockApprenticeContext = new Mock<IApprenticeContext>();
        _mockMyApprenticeshipContext = new Mock<IMyApprenticeshipContext>();
        _mockApprenticeContext.Setup(x => x.Find(It.IsAny<Guid>())).ReturnsAsync((Apprentice)null);
        _mockMyApprenticeshipContext.Setup(x => x.FindAll(It.IsAny<Guid>())).ReturnsAsync(new List<Data.Models.MyApprenticeship>());

        var validator= new CreateMyApprenticeshipCommandValidator(_mockApprenticeContext.Object, _mockMyApprenticeshipContext.Object);
        var command = new ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand.CreateMyApprenticeshipCommand { ApprenticeId = Guid.NewGuid() };
        var result = await validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.ApprenticeId)
            .WithErrorMessage(CreateMyApprenticeshipCommandValidator.ApprenticeIdNotPresent);
    }

    [Test]
    public async Task ApprenticeshipId_Validation_AlreadyPresent()
    {
        var apprenticeId = Guid.NewGuid();
        var apprenticeshipId = 1234;
        _mockApprenticeContext = new Mock<IApprenticeContext>();
        _mockMyApprenticeshipContext = new Mock<IMyApprenticeshipContext>();
        _mockApprenticeContext.Setup(x => x.Find(It.IsAny<Guid>())).ReturnsAsync(new Apprentice(apprenticeId, "first name", "last name", new MailAddress("test@test.com"), DateTime.Now));

        _mockMyApprenticeshipContext.Setup(x => x.FindAll(It.IsAny<Guid>())).ReturnsAsync(new List<Data.Models.MyApprenticeship>
        {
            new ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand.CreateMyApprenticeshipCommand {ApprenticeshipId = apprenticeshipId, ApprenticeId = apprenticeId}
        });

        var validator = new CreateMyApprenticeshipCommandValidator(_mockApprenticeContext.Object, _mockMyApprenticeshipContext.Object);
        var command = new ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand.CreateMyApprenticeshipCommand { ApprenticeId = apprenticeId, ApprenticeshipId = apprenticeshipId};
        var result = await validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.ApprenticeshipId)
            .WithErrorMessage(CreateMyApprenticeshipCommandValidator.ApprenticeshipIdAlreadyExists);
    }

    [Test]
    public async Task ApprenticeshipIdNull_ApprenticeshipIdNullAlreadyPresent_Validation()
    {
        var apprenticeId = Guid.NewGuid();
        _mockApprenticeContext = new Mock<IApprenticeContext>();
        _mockMyApprenticeshipContext = new Mock<IMyApprenticeshipContext>();
        _mockApprenticeContext.Setup(x => x.Find(It.IsAny<Guid>())).ReturnsAsync(new Apprentice(apprenticeId, "first name", "last name", new MailAddress("test@test.com"), DateTime.Now));

        _mockMyApprenticeshipContext.Setup(x => x.FindAll(It.IsAny<Guid>())).ReturnsAsync(
            new List<Data.Models.MyApprenticeship>
            {
                new ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand.CreateMyApprenticeshipCommand { ApprenticeId = apprenticeId }
            });

        var validator = new CreateMyApprenticeshipCommandValidator(_mockApprenticeContext.Object, _mockMyApprenticeshipContext.Object);
        var command = new ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand.CreateMyApprenticeshipCommand { ApprenticeId = apprenticeId, ApprenticeshipId = null };
        var result = await validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.ApprenticeshipId);
    }

    private CreateMyApprenticeshipCommandValidator GetValidator()
    {
        _mockApprenticeContext = new Mock<IApprenticeContext>();
        _mockMyApprenticeshipContext = new Mock<IMyApprenticeshipContext>();
        _mockApprenticeContext.Setup(x => x.Find(It.IsAny<Guid>())).ReturnsAsync(new Apprentice(Guid.NewGuid(),"first name","last name",new MailAddress("test@test.com"),DateTime.Now));
        _mockMyApprenticeshipContext.Setup(x => x.FindAll(It.IsAny<Guid>())).ReturnsAsync(new List<Data.Models.MyApprenticeship>());

        return new CreateMyApprenticeshipCommandValidator(_mockApprenticeContext.Object,_mockMyApprenticeshipContext.Object);
    }
}
