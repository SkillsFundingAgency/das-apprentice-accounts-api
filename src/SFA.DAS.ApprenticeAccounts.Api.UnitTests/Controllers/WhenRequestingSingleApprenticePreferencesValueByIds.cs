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
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Api.UnitTests.Controllers
{
    public class WhenRequestingSingleApprenticePreferencesValueByIds
    {
        [Test, MoqAutoData]
        public async Task AndMediatorCommandIsSuccessful_ThenReturnOk(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ApprenticePreferencesController controller,
            GetSingleApprenticePreferenceDto response,
            GetSingleApprenticePreferenceValueQuery query)
        {
            mediator.Setup(m => m.Send(It.IsAny<GetSingleApprenticePreferenceValueQuery>(), CancellationToken.None)).ReturnsAsync(response);

            var result = await controller.GetSingleApprenticePreferenceValue(query.ApprenticeId, query.PreferenceId) as OkObjectResult;

            var model = result.Value;

            model.Should().NotBeNull();
            model.Should().BeEquivalentTo(response);

        }

        [Test, MoqAutoData]
        public async Task AndMediatorCommandReturnsNull_ThenReturnNotFound(
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ApprenticePreferencesController controller,
            GetSingleApprenticePreferenceDto response,
            GetSingleApprenticePreferenceValueQuery query)
        {
            response.PreferenceId = 0;
            mediator.Setup(m => m.Send(It.IsAny<GetSingleApprenticePreferenceValueQuery>(), CancellationToken.None)).ReturnsAsync(response);
            

            var result = await controller.GetSingleApprenticePreferenceValue(query.ApprenticeId, query.PreferenceId);

            result.Should().BeOfType<NotFoundResult>();

        }
    }
}
