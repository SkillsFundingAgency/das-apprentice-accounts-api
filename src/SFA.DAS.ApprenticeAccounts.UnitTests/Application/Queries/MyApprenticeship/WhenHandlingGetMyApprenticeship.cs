using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand;
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
    public async Task ThenExpectedMyApprenticeshipsAreReturned(
            MyApprenticeshipQuery query,
            Mock<IApprenticeContext> mockApprenticeContext,
            Mock<IMyApprenticeshipContext> mockMyApprenticeshipContext,
            ApprenticeDto apprenticeDto,
            Guid apprenticeId
        )
    {
        var myApprenticeships = new List<Data.Models.MyApprenticeship>();
        foreach (var apprenticeship in apprenticeDto.MyApprenticeships)
        {
            apprenticeship.Id = apprenticeId;
            var command = new ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand.MyApprenticeship
                {
                    Id = apprenticeship.Id, 
                    ApprenticeId = apprenticeId, 
                    ApprenticeshipId = apprenticeship.ApprenticeshipId,
                    StandardUId = apprenticeship.StandardUId,
                    StartDate = apprenticeship.StartDate,
                    EndDate = apprenticeship.EndDate,
                    EmployerName = apprenticeship.EmployerName,
                    TrainingCode = apprenticeship.TrainingCode,
                    TrainingProviderId = apprenticeship.TrainingProviderId,
                    TrainingProviderName = apprenticeship.TrainingProviderName,
                    Uln = apprenticeship.Uln
                };
    
            myApprenticeships.Add(command);
        }

        var apprentice = new Apprentice(apprenticeId, "first name", "last name", new MailAddress("test@test.com"),
            DateTime.Now) { TermsOfUseAccepted = true };

        mockApprenticeContext.Setup(x => x.Find(query.ApprenticeId)).ReturnsAsync(apprentice); 
        mockMyApprenticeshipContext.Setup(x=>x.FindAll(query.ApprenticeId)).ReturnsAsync(myApprenticeships);

        var handler = new MyApprenticeshipQueryHandler(mockApprenticeContext.Object, mockMyApprenticeshipContext.Object);

        var result = await handler.Handle(query, CancellationToken.None);
        
        result.MyApprenticeships.Count().Should().Be(myApprenticeships.Count);
    
        result.MyApprenticeships.Should().BeEquivalentTo(myApprenticeships.Select(m => m),
            l 
                => l.Excluding(a => a.DomainEvents)
                    .Excluding(a=>a.ApprenticeId)
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
}
