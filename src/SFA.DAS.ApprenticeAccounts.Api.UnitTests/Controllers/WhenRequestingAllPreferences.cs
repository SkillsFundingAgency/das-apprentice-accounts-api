using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Api.Controllers;
using SFA.DAS.ApprenticeAccounts.Application.Queries.PreferencesQuery;
using SFA.DAS.ApprenticeAccounts.DTOs.Preferences.GetAllPreferences;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Api.UnitTests.Controllers
{
    public class WhenRequestingAllPreferences
    {
        [Test]
        [MoqAutoData]
        public async Task AndMediatorCommandSuccessful_ThenReturnOk(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] PreferencesController controller,
            GetAllPreferencesDto response)
        {
            mediator.Setup(m => m.Send(It.IsAny<GetAllPreferencesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await controller.GetAllPreferences() as OkObjectResult;

            result.Should().NotBeNull();

            var model = result.Value;

            model.Should().BeEquivalentTo(response.Preferences);
        }
    }
}