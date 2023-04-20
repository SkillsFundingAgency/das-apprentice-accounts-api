using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.DTOs.ApprenticeDto;
public class WhenMappingApprenticeshipToApprenticeDto
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

        var mappedApprenticeship =
            ApprenticeAccounts.DTOs.MyApprenticeship.ApprenticeDto.Create(apprentice, myApprenticeships, null);

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

    [Test, RecursiveMoqAutoData]
    public void AndApprenticeshipIdIsAParameter_ThenTheFieldsAreCorrectlyMappedAndReturned(
    Apprentice apprentice,
    MyApprenticeship myApprenticeshipCreatedFirstNotMatchingApprenticeshipId,
    MyApprenticeship myApprenticeCreatedSecondMatchingApprenticeshipId,
    MyApprenticeship myApprenticeshipCreatedMostRecently,
    int apprenticeshipId)
    {
        myApprenticeshipCreatedFirstNotMatchingApprenticeshipId.CreatedOn = DateTime.Today.AddDays(-50);
        myApprenticeshipCreatedFirstNotMatchingApprenticeshipId.ApprenticeshipId = apprenticeshipId+1;
        myApprenticeCreatedSecondMatchingApprenticeshipId.CreatedOn = DateTime.Today.AddDays(-10);
        myApprenticeCreatedSecondMatchingApprenticeshipId.ApprenticeshipId = apprenticeshipId;
        myApprenticeshipCreatedMostRecently.CreatedOn = DateTime.Today;
        myApprenticeshipCreatedMostRecently.ApprenticeshipId = apprenticeshipId + 2;
        var myApprenticeships = new List<MyApprenticeship>
            {
                myApprenticeshipCreatedFirstNotMatchingApprenticeshipId,
                myApprenticeCreatedSecondMatchingApprenticeshipId,
                myApprenticeshipCreatedMostRecently
            };

        var mappedApprenticeship =
            ApprenticeAccounts.DTOs.MyApprenticeship.ApprenticeDto.Create(apprentice, myApprenticeships, apprenticeshipId);

        mappedApprenticeship.Should().NotBeNull();
        mappedApprenticeship.MyApprenticeships.Count().Should().Be(1);

        mappedApprenticeship.Should().BeEquivalentTo(apprentice,
            l => l.Excluding(e => e.Email)
                .Excluding(e => e.Id)
                .Excluding(e => e.PreviousEmailAddresses)
                .Excluding(e => e.Preferences)
                .Excluding(e => e.TermsOfUseAccepted)
                .Excluding(e => e.CreatedOn)
                .Excluding(e => e.DomainEvents));

        mappedApprenticeship.MyApprenticeships.First().Should().BeEquivalentTo(myApprenticeCreatedSecondMatchingApprenticeshipId,
            l => l.Excluding(e => e.DomainEvents)
                .Excluding(e => e.ApprenticeId));
    }
}
