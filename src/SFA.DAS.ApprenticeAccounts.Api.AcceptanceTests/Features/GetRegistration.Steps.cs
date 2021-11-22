using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeCommitments.Application.Queries.RegistrationQuery;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using System;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "GetRegistration")]
    public class GetRegistrationSteps
    {
        private readonly TestContext _context;
        private Fixture _fixture;
        private Registration _registration;

        public GetRegistrationSteps(TestContext context)
        {
            _fixture = new Fixture();
            _registration = _fixture.Create<Registration>();
            _context = context;
        }

        [Given("there is no registration")]
        public void GivenThereIsNoRegistration()
        {
        }

        [Given("there is a registration with a First Viewed on (.*) and has the (.*) assigned to it")]
        public Task GivenThereIsARegistrationWithAFirstViewedOnAndHasTheAssignedToIt(DateTime? viewedOn, Guid? userIdentityId)
        {
            _registration.SetProperty(x => x.FirstViewedOn, viewedOn);
            _registration.SetProperty(x => x.ApprenticeId, userIdentityId);
            _context.DbContext.Registrations.Add(_registration);
            return _context.DbContext.SaveChangesAsync();
        }

        [When("we try to retrieve the registration")]
        public Task WhenWeTryToRetrieveTheRegistration()
        {
            return _context.Api.Get($"registrations/{_registration.RegistrationId}");
        }

        [Given("there is an empty apprentice id")]
        public void GivenThereIsAnEmptyRegistration()
        {
            _fixture.Inject(Guid.Empty);
            _registration = _fixture.Create<Registration>();
        }

        [When("we try to retrieve the registration using a bad request format")]
        public Task WhenWeTryToRetrieveTheRegistrationUsingABadRequestFormat()
        {
            return _context.Api.Get($"registrations/1234-1234");
        }

        [Then("the result should return ok")]
        public void ThenTheResultShouldReturnOk()
        {
            _context.Api.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Then("the response should match the registration in the database with (.*) and (.*)")]
        public async Task ThenTheResponseShouldMatchTheRegistrationInTheDatabaseWithAnd(bool hasViewed, bool hasCompleted)
        {
            var content = await _context.Api.Response.Content.ReadAsStringAsync();
            content.Should().NotBeNull();
            var response = JsonConvert.DeserializeObject<RegistrationResponse>(content);
            response.DateOfBirth.Should().Be(_registration.DateOfBirth);
            response.Email.Should().Be(_registration.Email.ToString());
            response.FirstName.Should().Be(_registration.FirstName);
            response.RegistrationId.Should().Be(_registration.RegistrationId);
            response.HasViewedVerification.Should().Be(hasViewed);
            response.HasCompletedVerification.Should().Be(hasCompleted);
        }

        [Then("the result should return not found")]
        public void ThenTheResultShouldReturnNotFound()
        {
            _context.Api.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Then("the result should return bad request")]
        public void ThenTheResultShouldReturnBadRequest()
        {
            _context.Api.Response.Should().Be400BadRequest();
        }

        [Then("the error must be say apprentice id must be valid")]
        public async Task ThenTheErrorMustBeSayRegistrationMustBeValid()
        {
            var content = await _context.Api.Response.Content.ReadAsStringAsync();
            content.Should().NotBeNull();
            var response = JsonConvert.DeserializeObject<ValidationProblemDetails>(content);
            response.Errors.Should().ContainKey("ApprenticeId")
                .WhichValue.Should().Contain("The Apprentice Id must be valid");
        }
    }
}