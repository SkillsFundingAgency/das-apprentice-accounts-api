using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateMyApprenticeshipCommand;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Application.Commands.MyApprenticeship;
[TestFixture]
public class UpdateMyApprenticeshipCommandTests
{
    [Test, AutoData]
    public void Operator_TransformsToCommand(UpdateMyApprenticeshipRequest request)
    {
        UpdateMyApprenticeshipCommand cmd = request;
        cmd.Should().BeEquivalentTo(request);
    }
}
