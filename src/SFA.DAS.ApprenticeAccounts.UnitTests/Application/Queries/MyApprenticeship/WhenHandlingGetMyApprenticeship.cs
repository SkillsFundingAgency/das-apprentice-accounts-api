using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Queries.MyApprenticeshipQuery;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.MyApprenticeship;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Application.Queries.MyApprenticeship;

public class WhenHandlingGetMyApprenticeship
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task ThenExpectedMyApprenticeshipIsReturned(
            MyApprenticeshipQuery query,
            Mock<IApprenticeContext> mockApprenticeContext,
            Mock<IMyApprenticeshipContext> mockMyApprenticeshipContext,
            Data.Models.MyApprenticeship myApprenticeship,
            Guid apprenticeId
        )
    {
        var apprentice = new Apprentice(apprenticeId, "first name", "last name", new MailAddress("test@test.com"),
            DateTime.Now) { TermsOfUseAccepted = true };
    
        mockApprenticeContext.Setup(x => x.Find(query.ApprenticeId)).ReturnsAsync(apprentice); 
        mockMyApprenticeshipContext.Setup(x=>x.FindByApprenticeId(query.ApprenticeId)).ReturnsAsync(myApprenticeship);
    
        var handler = new MyApprenticeshipQueryHandler(mockApprenticeContext.Object, mockMyApprenticeshipContext.Object);
    
        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeEquivalentTo(myApprenticeship,
            l 
                => l.Excluding(a => a.DomainEvents)
                    .Excluding(a => a.ApprenticeId)
                    .Excluding(a => a.Id)
                    .Excluding(a => a.CreatedOn)
        );
     }
    
    [Test]
    [RecursiveMoqAutoData]
    public async Task ThenNoMatchingApprenticeReturnsNull(
            MyApprenticeshipQuery query,
            Mock<IApprenticeContext> mockApprenticeContext,
            Mock<IMyApprenticeshipContext> mockMyApprenticeshipContext
    )
    {
        mockApprenticeContext.Setup(x => x.Find(query.ApprenticeId)).ReturnsAsync((Apprentice)null);
    
        var handler =
            new MyApprenticeshipQueryHandler(mockApprenticeContext.Object, mockMyApprenticeshipContext.Object);
        var result = await handler.Handle(query, CancellationToken.None);
    
        result.Should().BeNull();
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task ThenNoMatchingMyApprenticeshipIdReturnsEmptyRecord(
        MyApprenticeshipQuery query,
        Mock<IApprenticeContext> mockApprenticeContext,
        Mock<IMyApprenticeshipContext> mockMyApprenticeshipContext,
        Apprentice apprentice
    )
    {
        mockApprenticeContext.Setup(x => x.Find(query.ApprenticeId)).ReturnsAsync(apprentice);
        mockMyApprenticeshipContext.Setup(x => x.FindByApprenticeId(It.IsAny<Guid>())).ReturnsAsync((Data.Models.MyApprenticeship)null);

        var handler =
            new MyApprenticeshipQueryHandler(mockApprenticeContext.Object, mockMyApprenticeshipContext.Object);
        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeOfType<MyApprenticeshipDto>();
        result.IsEmpty().Should().BeTrue();

    }
}
