using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Commands.CreateMyApprenticeCommand;
using SFA.DAS.ApprenticeAccounts.Application.Queries.MyApprenticeshipsQuery;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.MyApprenticeships;
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
            ApprenticeWithMyApprenticeshipsDto apprenticeWithMyApprenticeships,
            Guid apprenticeId
        )
    {
        var myApprenticeships = new List<Data.Models.MyApprenticeship>();
        foreach (var apprenticeship in apprenticeWithMyApprenticeships.MyApprenticeships)
        {
            apprenticeship.Id = apprenticeId;
            var command = new CreateMyApprenticeshipCommand
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
            DateTime.Now) {TermsOfUseAccepted = true, MyApprenticeships = myApprenticeships};

        mockApprenticeContext.Setup(x => x.Find(query.ApprenticeId)).ReturnsAsync(apprentice);
        
        var handler =
            new MyApprenticeshipQueryHandler(mockApprenticeContext.Object);
        var result = await handler.Handle(query, CancellationToken.None);
        
        result.MyApprenticeships.Count().Should().Be(myApprenticeships.Count);

        result.MyApprenticeships.Should().BeEquivalentTo(myApprenticeships.Select(m => m),
            l 
                => l.Excluding(a => a.DomainEvents)
                    .Excluding(a=>a.ApprenticeId)
                    .Excluding(a=>a.Apprentice)
                );
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task ThenNoMatchingApprenticeReturnsNull(
            MyApprenticeshipQuery query,
            Mock<IApprenticeContext> mockApprenticeContext,
            ApprenticeWithMyApprenticeshipsDto apprenticeWithMyApprenticeships,
            Guid apprenticeId
        )
    {
        mockApprenticeContext.Setup(x => x.Find(query.ApprenticeId)).ReturnsAsync((Apprentice)null);

        var handler =
            new MyApprenticeshipQueryHandler(mockApprenticeContext.Object);
        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeNull();
    }
}
