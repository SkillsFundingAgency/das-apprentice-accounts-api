using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand;

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
}
