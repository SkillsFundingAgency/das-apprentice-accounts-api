using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Api.Controllers;
using SFA.DAS.ApprenticeAccounts.Application.Queries.GetApprenticePreferenceForApprenticeAndPreferenceQuery;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences.GetApprenticePreferenceForApprenticeAndPreference;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Api.UnitTests.Controllers
{
    public class WhenRequestingApprenticePreferenceByApprenticeAndPreference
    {
        [Test]
        [MoqAutoData]
        public async Task AndMediatorCommandIsSuccessful_ThenReturnOk(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ApprenticePreferencesController controller,
            GetApprenticePreferenceForApprenticeAndPreferenceDto response,
            GetApprenticePreferenceForApprenticeAndPreferenceQuery query)
        {
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticePreferenceForApprenticeAndPreferenceQuery>(),
                CancellationToken.None)).ReturnsAsync(response);

            var result =
                await controller.GetApprenticePreferenceForApprenticeAndPreference(query.ApprenticeId,
                    query.PreferenceId) as OkObjectResult;

            var model = result.Value;

            model.Should().NotBeNull();
            model.Should().BeEquivalentTo(response);
        }

        [Test]
        [MoqAutoData]
        public async Task AndMediatorCommandReturnsNull_ThenReturnNotFound(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ApprenticePreferencesController controller,
            GetApprenticePreferenceForApprenticeAndPreferenceDto response,
            GetApprenticePreferenceForApprenticeAndPreferenceQuery query)
        {
            response.PreferenceId = 0;
            mediator.Setup(m => m.Send(It.IsAny<GetApprenticePreferenceForApprenticeAndPreferenceQuery>(),
                CancellationToken.None)).ReturnsAsync(response);


            var result =
                await controller.GetApprenticePreferenceForApprenticeAndPreference(query.ApprenticeId,
                    query.PreferenceId);

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}