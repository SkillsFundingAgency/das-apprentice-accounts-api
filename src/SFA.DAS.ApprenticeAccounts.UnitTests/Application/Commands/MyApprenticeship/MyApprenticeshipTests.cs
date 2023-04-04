using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand;
using System;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Application.Commands.MyApprenticeship;

[TestFixture]
public class MyApprenticeshipConstructorTests
{
    [Test, AutoData]
    public void Constructor_TransformsCommandToEntity(Guid id, CreateMyApprenticeshipCommand command)
    {
        var entity = new Data.Models.MyApprenticeship(id,command);
        entity.Should().BeEquivalentTo(command);
    }
}
