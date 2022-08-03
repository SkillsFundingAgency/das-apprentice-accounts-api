using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.ApprenticeAccounts.Configuration;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeAccounts.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "GetApprentice")]
    public class GetApprenticeSteps
    {
        private readonly TestContext _context;
        private readonly Fixture _fixture = new Fixture();
        private Apprentice _apprentice;

        public GetApprenticeSteps(TestContext context)
        {
            _context = context;
            _apprentice = _fixture.Build<Apprentice>().Create();

            _context.ApplicationSettings = _fixture.Build<ApplicationSettings>().Create();

            var startDate = new System.DateTime(2000, 01, 01);
        }

        [Given("there is one apprentice")]
        public async Task GiveThereIsOneApprentice()
        {
            _context.DbContext.Apprentices.Add(_apprentice);
            await _context.DbContext.SaveChangesAsync();
        }

        [Given("there is no apprentice")]
        public void GivenThereIsNoApprentice()
        {
        }

        [Given("that apprentice has accepted the terms of service")]
        public async Task GivenThatApprenticeHasAcceptedTheTermsOfService()
        {
            var logger = new Mock<ILogger>();
            var patchDto = new ApprenticePatchDto(_apprentice, logger.Object)
                { TermsOfUseAccepted = true };
            await _context.Api.Patch("apprentices/{id}", patchDto);
        }

        [Given("there is a new version of terms of service released")]
        public void GivenThereIsANewVersionOfTermsOfServiceReleased()
        {
            _context.ApplicationSettings.TermsOfServiceUpdatedOn = DateTime.UtcNow.AddDays(10);
        }

        [When("we try to retrieve the apprentice")]
        public async Task WhenWeTryToRetrieveTheApprenticeship()
        {
            await _context.Api.Get($"apprentices/{_apprentice.Id}");
        }

        [Then("the result should return ok")]
        public void ThenTheResultShouldReturnOk()
        {
            _context.Api.Response.Should().Be200Ok();
        }

        [Then("the response should match the expected apprentice values")]
        public void ThenTheResponseShouldMatchTheExpectedApprenticeValues()
        {
            _context.Api.Response
                .Should().BeAs(new
                {
                    ApprenticeId = _apprentice.Id,
                    _apprentice.FirstName,
                    _apprentice.LastName,
                });
        }

        [Then("the result should return Not Found")]
        public void ThenTheResultShouldReturnNotFound()
        {
            _context.Api.Response.Should().Be404NotFound();
        }

        [Then("the response terms of service should be set correctly")]
        public void ThenTheResponseTermsOfServiceShouldBeSetCorrectly()
        {
            _context.Api.Response
                .Should().BeAs(new
                {
                    TermsOfUseAccepted = false,
                    ReacceptTermsOfUseRequired = true,
                });
        }
    }
}