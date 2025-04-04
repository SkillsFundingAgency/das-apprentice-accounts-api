using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Api.Controllers;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Api.UnitTests.Controllers.MyApprenticeship;
public class WhenGettingApprenticeSupportAndGuidance
{
      [Test, MoqAutoData]
      public async Task GetAllApprenticeArticles_Returns(
          [Greedy] ApprenticeSupportAndGuidance controller,
          Guid apprenticeId)
      {
            var result = await controller.GetAllApprenticeArticles(apprenticeId) as OkObjectResult;
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
      }

    [Test, MoqAutoData]
    public async Task GetAllApprenticeArticlesByIdentifier_returns(
        [Greedy] ApprenticeSupportAndGuidance controller,
        Guid apprenticeId, string articleIdentifier)
    {
        var result = await controller.GetAllApprenticeArticlesByIdentifier(apprenticeId, articleIdentifier) as OkObjectResult;
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(OkObjectResult));
    }

    [Test, MoqAutoData]
    public async Task AddOrUpdateApprenticeArticle_returns(
        [Greedy] ApprenticeSupportAndGuidance controller,
        Guid apprenticeId, string articleIdentifier, string artitleTitle,
        [Greedy] ApprenticeSupportAndGuidance.ApprenticeArticleRequest request)
    {
        var result = await controller.AddOrUpdateApprenticeArticle(apprenticeId, articleIdentifier, artitleTitle, request) as OkObjectResult;
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(OkObjectResult));
    }
}