using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using AutoFixture.NUnit3;
using System.Net.Mail;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using FluentAssertions;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.DTOs.ApprenticeSync;
public class WhenMappingFromApprenticeToApprenticeSyncDto
{
    [Test]
    [RecursiveMoqAutoData]
    public void ThenTheFieldsAreMappedCorrectly(
        [Frozen] Guid apprenticeId,
        [Frozen] string firstName,
        [Frozen] string surname,
        [Frozen] MailAddress email,
        [Frozen] DateTime dateOfBirth,
        [Frozen] DateTime updatedSinceDate
    )
    {
        var apprentice = new Apprentice(
            apprenticeId,
            firstName,
            surname,
            email,
            dateOfBirth
        )
        {
            UpdatedOn = updatedSinceDate
        };

        var expectedObject = new ApprenticeSyncDto()
        {
            ApprenticeID = apprenticeId,
            FirstName = firstName,
            LastName = surname,
            Email = email.Address,
            LastUpdatedDate = updatedSinceDate,
            DateOfBirth = dateOfBirth
        };

        var actualObject = ApprenticeSyncDto.MapToSyncResponse(apprentice);

        expectedObject.Should().BeEquivalentTo(actualObject);
    }
}
