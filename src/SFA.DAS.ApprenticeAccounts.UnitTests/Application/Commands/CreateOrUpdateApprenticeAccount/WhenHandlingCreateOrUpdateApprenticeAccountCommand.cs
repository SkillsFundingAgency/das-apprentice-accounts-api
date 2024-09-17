using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Commands.CreateOrUpdateApprenticeAccount;
using SFA.DAS.ApprenticeAccounts.Configuration;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Data;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Application.Commands.CreateOrUpdateApprenticeAccount;

public class WhenHandlingCreateOrUpdateApprenticeAccountCommand
{
    [Test, MoqAutoData]
    public async Task Then_If_The_Apprentice_Exists_By_Gov_Identifier_Then_It_Is_Returned(
        CreateOrUpdateApprenticeAccountCommand command,
        [Frozen] Mock<ApplicationSettings> settings,
        [Frozen] Mock<IApprenticeContext> apprenticeContext,
        CreateOrUpdateApprenticeAccountCommandHandler handler)
    {
        var email = "test@example.com";
        command.Email = email;
        var apprentice = new Apprentice(Guid.NewGuid(), null!, null!, new MailAddress(email),new DateTime(), command.GovUkIdentifier );
        var termsOfServiceDate = DateTime.UtcNow;
        settings.Setup(x => x.TermsOfServiceUpdatedOn).Returns(termsOfServiceDate);
        apprenticeContext.Setup(x => x.FindByGovIdentifier(command.GovUkIdentifier)).ReturnsAsync(apprentice);

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.Should().BeEquivalentTo(ApprenticeDto.Create(apprentice, termsOfServiceDate));
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_The_Apprentice_Exists_By_Gov_Identifier_And_The_Email_Is_Different_Then_It_Is_Updated_And_Returned(
        CreateOrUpdateApprenticeAccountCommand command,
        [Frozen] Mock<ApplicationSettings> settings,
        [Frozen] Mock<IApprenticeContext> apprenticeContext,
        CreateOrUpdateApprenticeAccountCommandHandler handler)
    {
        var email = "test@example.com";
        command.Email = email;
        var apprentice = new Apprentice(Guid.NewGuid(), null!, null!, new MailAddress("differenttest@example.com"),new DateTime(), command.GovUkIdentifier );
        var termsOfServiceDate = DateTime.UtcNow;
        settings.Setup(x => x.TermsOfServiceUpdatedOn).Returns(termsOfServiceDate);
        apprenticeContext.Setup(x => x.FindByGovIdentifier(command.GovUkIdentifier)).ReturnsAsync(apprentice);
        apprenticeContext.Setup(x => x.FindByEmail(It.Is<MailAddress>(c=>c.Address == command.Email))).ReturnsAsync((Apprentice)null);

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.Email.Should().Be(email);
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_The_Apprentice_Exists_By_Email_But_Not_By_Gov_Identifier_Then_It_Is_Updated_And_Returned(
        CreateOrUpdateApprenticeAccountCommand command,
        [Frozen] Mock<ApplicationSettings> settings,
        [Frozen] Mock<IApprenticeContext> apprenticeContext,
        CreateOrUpdateApprenticeAccountCommandHandler handler)
    {
        var email = "test@example.com";
        command.Email = email;
        var apprentice = new Apprentice(Guid.NewGuid(), null!, null!, new MailAddress(email),new DateTime() );
        var termsOfServiceDate = DateTime.UtcNow;
        settings.Setup(x => x.TermsOfServiceUpdatedOn).Returns(termsOfServiceDate);
        apprenticeContext.Setup(x => x.FindByGovIdentifier(command.GovUkIdentifier)).ReturnsAsync((Apprentice)null);
        apprenticeContext.Setup(x => x.FindByEmail(It.Is<MailAddress>(c=>c.Address == command.Email))).ReturnsAsync(apprentice);

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.GovUkIdentifier.Should().Be(command.GovUkIdentifier);
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_The_Apprentice_Exists_By_Email_And_By_Gov_Identifier_And_The_Email_Is_Different_Then_Email_Matching_Command_Is_Updated_With_Gov_Login(
        CreateOrUpdateApprenticeAccountCommand command,
        [Frozen] Mock<ApplicationSettings> settings,
        [Frozen] Mock<IApprenticeContext> apprenticeContext,
        CreateOrUpdateApprenticeAccountCommandHandler handler)
    {
        var idEmail = Guid.NewGuid();
        var idGovIdentifier = Guid.NewGuid();
        var email = "test@example.com";
        var email2 = "test2@example.com";
        command.Email = email;
        var apprenticeEmail = new Apprentice(idEmail, null!, null!, new MailAddress(email),new DateTime() );
        var apprenticeGov = new Apprentice(idGovIdentifier, null!, null!, new MailAddress(email2),new DateTime(), command.GovUkIdentifier );
        var termsOfServiceDate = DateTime.UtcNow;
        settings.Setup(x => x.TermsOfServiceUpdatedOn).Returns(termsOfServiceDate);
        apprenticeContext.Setup(x => x.FindByGovIdentifier(command.GovUkIdentifier)).ReturnsAsync(apprenticeGov);
        apprenticeContext.Setup(x => x.FindByEmail(It.Is<MailAddress>(c=>c.Address == command.Email))).ReturnsAsync(apprenticeEmail);

        var actual = await handler.Handle(command, CancellationToken.None);

        actual.GovUkIdentifier.Should().Be(command.GovUkIdentifier);
        actual.Email.Should().Be(email);
        actual.ApprenticeId.Should().Be(idEmail);
    }
    
    
    [Test, MoqAutoData]
    public async Task Then_If_The_Apprentice_Exists_By_Email_And_By_Gov_Identifier_And_The_Email_Is_Different_Then_Email_Matching_Command_But_Has_GovIdentifier_Then_Exception_Thrown(
        CreateOrUpdateApprenticeAccountCommand command,
        [Frozen] Mock<ApplicationSettings> settings,
        [Frozen] Mock<IApprenticeContext> apprenticeContext,
        CreateOrUpdateApprenticeAccountCommandHandler handler)
    {
        var idEmail = Guid.NewGuid();
        var idGovIdentifier = Guid.NewGuid();
        var email = "test@example.com";
        var email2 = "test2@example.com";
        command.Email = email;
        var apprenticeEmail = new Apprentice(idEmail, null!, null!, new MailAddress(email),new DateTime(),  idGovIdentifier.ToString());
        var apprenticeGov = new Apprentice(idGovIdentifier, null!, null!, new MailAddress(email2),new DateTime(), command.GovUkIdentifier );
        var termsOfServiceDate = DateTime.UtcNow;
        settings.Setup(x => x.TermsOfServiceUpdatedOn).Returns(termsOfServiceDate);
        apprenticeContext.Setup(x => x.FindByGovIdentifier(command.GovUkIdentifier)).ReturnsAsync(apprenticeGov);
        apprenticeContext.Setup(x => x.FindByEmail(It.Is<MailAddress>(c=>c.Address == command.Email))).ReturnsAsync(apprenticeEmail);

        Assert.ThrowsAsync<ConstraintException>(() => handler.Handle(command, CancellationToken.None));
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_The_Apprentice_Does_Not_Exist_By_Email_Or_By_Gov_Identifier_Then_It_Is_Created_And_Returned(
        CreateOrUpdateApprenticeAccountCommand command,
        [Frozen] Mock<ApplicationSettings> settings)
    {
        var apprenticeContext = new Mock<IApprenticeContext>();
        var termsOfServiceDate = DateTime.UtcNow;
        command.Email = "test@example.com";
        settings.Setup(x => x.TermsOfServiceUpdatedOn).Returns(termsOfServiceDate);
        apprenticeContext.Setup(x => x.FindByGovIdentifier(It.IsAny<string>())).ReturnsAsync((Apprentice)null);
        apprenticeContext.Setup(x => x.FindByEmail(It.IsAny<MailAddress>())).ReturnsAsync((Apprentice)null);
        var handler = new CreateOrUpdateApprenticeAccountCommandHandler(apprenticeContext.Object, settings.Object);
        
        var actual = await handler.Handle(command, CancellationToken.None);

        actual.GovUkIdentifier.Should().Be(command.GovUkIdentifier);
        actual.Email.Should().Be(command.Email);
        actual.ApprenticeId.Should().NotBe(Guid.Empty);
        apprenticeContext.Verify(
            x => x.AddAsync(
                It.Is<Apprentice>(c =>
                    c.GovUkIdentifier == command.GovUkIdentifier && c.Email.Address == command.Email),
                It.IsAny<CancellationToken>()), Times.Once);
    }
}