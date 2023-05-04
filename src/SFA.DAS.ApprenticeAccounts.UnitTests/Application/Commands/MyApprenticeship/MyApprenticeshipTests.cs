using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateMyApprenticeshipCommand;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Application.Commands.MyApprenticeship;

[TestFixture]
public class MyApprenticeshipTests
{
    [Test, AutoData]
    public void Operator_TransformsCommandToEntity(CreateMyApprenticeshipCommand command)
    {
        var entity = (Data.Models.MyApprenticeship) command;
        entity.Should().BeEquivalentTo(command);
    }


    [Test, AutoData]
    public void UpdateMyApprenticeship_UpdatesMyApprenticeship(Data.Models.MyApprenticeship myApprenticeship, UpdateMyApprenticeshipCommand command)
    {
        myApprenticeship.Uln.Should().NotBe(command.Uln);
        myApprenticeship.ApprenticeshipId.Should().NotBe(command.ApprenticeshipId);
        myApprenticeship.EmployerName.Should().NotBe(command.EmployerName);
        myApprenticeship.StartDate.Should().NotBe(command.StartDate);
        myApprenticeship.EndDate.Should().NotBe(command.EndDate);
        myApprenticeship.StandardUId.Should().NotBe(command.StandardUId);
        myApprenticeship.TrainingCode.Should().NotBe(command.TrainingCode);
        myApprenticeship.TrainingProviderId.Should().NotBe(command.TrainingProviderId);
        myApprenticeship.TrainingProviderName.Should().NotBe(command.TrainingProviderName);

        myApprenticeship.UpdateMyApprenticeship(command);
        myApprenticeship.Uln.Should().Be(command.Uln);
        myApprenticeship.ApprenticeshipId.Should().Be(command.ApprenticeshipId);
        myApprenticeship.EmployerName.Should().Be(command.EmployerName);
        myApprenticeship.StartDate.Should().Be(command.StartDate);
        myApprenticeship.EndDate.Should().Be(command.EndDate);
        myApprenticeship.StandardUId.Should().Be(command.StandardUId);
        myApprenticeship.TrainingCode.Should().Be(command.TrainingCode);
        myApprenticeship.TrainingProviderId.Should().Be(command.TrainingProviderId);
        myApprenticeship.TrainingProviderName.Should().Be(command.TrainingProviderName);

    }
}
