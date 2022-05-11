using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Api.Controllers;
using SFA.DAS.ApprenticeAccounts.Application.Queries.ApprenticePreferencesQuery;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Api.UnitTests.Controllers
{
    public class WhenRequestingPreferencesById
    {
        [Test, MoqAutoData]
        public async Task AndMediatorCommandSuccessful_ThenReturnOk(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ApprenticePreferencesController controller,
            ApprenticePreferencesDto response,
            Guid mockId)
        {
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticePreferencesByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await controller.GetApprenticePreferencesById(mockId) as OkObjectResult;

            result.Should().NotBeNull();

            var model = result.Value;

            model.Should().BeEquivalentTo(response.ApprenticePreferences);
        }
    }
}
