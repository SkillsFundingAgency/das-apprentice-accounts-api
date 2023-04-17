using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Application.Commands.MyApprenticeship;

[TestFixture]
public class CreateMyApprenticeshipCommandTests
{
    [Test, AutoData]
    public void Operator_TransformsToCommand(CreateMyApprenticeshipRequest request)
    {
        ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand.CreateMyApprenticeshipCommand cmd = request;
        cmd.Should().BeEquivalentTo(request);
    }
}
