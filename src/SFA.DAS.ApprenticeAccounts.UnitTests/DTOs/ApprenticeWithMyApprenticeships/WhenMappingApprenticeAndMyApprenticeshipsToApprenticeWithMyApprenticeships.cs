using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.MyApprenticeship;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.DTOs.ApprenticeWithMyApprenticeships;
public class WhenMappingApprenticeAndMyApprenticeshipsToApprenticeWithMyApprenticeships
{
    [Test, RecursiveMoqAutoData]
    public void ThenTheFieldsAreCorrectlyMappedAndOrdered(
        Apprentice apprentice,
        MyApprenticeship myApprenticeship1_CreatedBetweenFirstAndLast,
        MyApprenticeship myApprenticeship2_CreatedLeastRecently,
        MyApprenticeship myApprenticeship3_CreatedMostRecently)
    { 
        myApprenticeship1_CreatedBetweenFirstAndLast.CreatedOn = DateTime.Today.AddDays(-5);
        myApprenticeship2_CreatedLeastRecently.CreatedOn = DateTime.Today.AddDays(-10);
        myApprenticeship3_CreatedMostRecently.CreatedOn = DateTime.Today;

        var myApprenticeships = new List<MyApprenticeship>
            {
                myApprenticeship1_CreatedBetweenFirstAndLast,
                myApprenticeship2_CreatedLeastRecently,
                myApprenticeship3_CreatedMostRecently
            };

        var mappedApprenticeship = ApprenticeWithMyApprenticeshipsDto.Create(apprentice, myApprenticeships);

        mappedApprenticeship.Should().NotBeNull();
        mappedApprenticeship.MyApprenticeships.Count().Should().Be(myApprenticeships.Count);

        mappedApprenticeship.Should().BeEquivalentTo(apprentice,
            l=>l.Excluding(e=>e.Email)
                .Excluding(e=>e.Id)
                .Excluding(e=>e.PreviousEmailAddresses)
                .Excluding(e => e.Preferences)
                .Excluding(e=>e.TermsOfUseAccepted)
                .Excluding(e=>e.CreatedOn)
                .Excluding(e=>e.DomainEvents));

        mappedApprenticeship.MyApprenticeships.First().Should().BeEquivalentTo(myApprenticeship3_CreatedMostRecently,
            l=>l.Excluding(e=>e.DomainEvents)
                .Excluding(e=>e.ApprenticeId));
        mappedApprenticeship.MyApprenticeships.Skip(1).First().Should().BeEquivalentTo(myApprenticeship1_CreatedBetweenFirstAndLast,
            l => l.Excluding(e => e.DomainEvents)
                .Excluding(e => e.ApprenticeId));
        mappedApprenticeship.MyApprenticeships.Last().Should().BeEquivalentTo(myApprenticeship2_CreatedLeastRecently,
            l => l.Excluding(e => e.DomainEvents)
                .Excluding(e => e.ApprenticeId));

    }
}
