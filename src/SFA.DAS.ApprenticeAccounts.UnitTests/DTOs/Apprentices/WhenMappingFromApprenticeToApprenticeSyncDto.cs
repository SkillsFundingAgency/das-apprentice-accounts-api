using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.DTOs.ApprenticeSync;
public class WhenMappingFromApprenticeToApprenticeSyncDto
{
    [Test]
    [RecursiveMoqAutoData]
    public void ThenTheFieldsAreMappedCorrectly(Apprentice apprentice)
    {
        var mappedObject = ApprenticeSyncDto.MapToSyncResponse(apprentice);

        mappedObject.ApprenticeID.Should().Be(apprentice.Id);
        mappedObject.FirstName.Should().Be(apprentice.FirstName);
        mappedObject.LastName.Should().Be(apprentice.LastName);
        mappedObject.LastUpdatedDate.Should().Be(apprentice.UpdatedOn);
        mappedObject.Email.Should().Be(apprentice.Email.Address);
        mappedObject.DateOfBirth.Should().Be(apprentice.DateOfBirth);
    }
}