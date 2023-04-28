using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.DTOs.MyApprenticeship;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Application.Commands.MyApprenticeship;

[TestFixture]
public class MyApprenticeshipDtoTests
{
    [Test, AutoData]
    public void Operator_TransformsModelToDto(Data.Models.MyApprenticeship myApprenticeship)
    {
        var dto = (MyApprenticeshipDto)myApprenticeship;
        dto.Should().BeEquivalentTo(myApprenticeship, l => l
            .Excluding(a => a.Id)
            .Excluding(a => a.ApprenticeId)
            .Excluding(a => a.CreatedOn)
            .Excluding(a => a.DomainEvents)
        );
    }

    [Test]
    public void Operator_HandlesNull()
    {
        var dto = (MyApprenticeshipDto)(Data.Models.MyApprenticeship)null;
        dto.Should().BeNull();
    }
}
