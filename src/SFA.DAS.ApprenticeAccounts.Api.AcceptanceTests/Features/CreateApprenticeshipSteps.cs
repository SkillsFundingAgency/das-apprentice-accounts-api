using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeCommitments.Application.Commands.CreateRegistrationCommand;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using SFA.DAS.ApprenticeCommitments.Messages.Events;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "CreateApprenticeship")]
    public class CreateApprenticeshipSteps
    {
        private readonly TestContext _context;
        private CreateRegistrationCommand _createApprenticeshipRequest;

        public CreateApprenticeshipSteps(TestContext context)
        {
            _context = context;
        }

        [Given(@"we have an invalid apprenticeship request")]
        public void GivenWeHaveAnInvalidApprenticeshipRequest()
        {
            _createApprenticeshipRequest = new CreateRegistrationCommand();
        }

        [Given(@"we have a valid apprenticeship request")]
        public void GivenWeHaveAValidApprenticeshipRequest()
        {
            _createApprenticeshipRequest = new CreateRegistrationCommand
            {
                RegistrationId = Guid.NewGuid(),
                CommitmentsApprenticeshipId = 1233,
                FirstName = "Bob",
                LastName = "Bobbertson",
                DateOfBirth = new DateTime(2000, 1, 2),
                Email = "paul@fff.com",
                EmployerName = "My Company",
                EmployerAccountLegalEntityId = 61234,
                TrainingProviderId = 71234,
                TrainingProviderName = "My Training Provider",
                CourseName = "My course",
                CourseLevel = 5,
                CourseOption = "",
                PlannedStartDate = new DateTime(2001, 03, 20),
                PlannedEndDate = new DateTime(2003, 07, 15),
            };
        }

        [When(@"the apprenticeship is posted")]
        public async Task WhenTheApprenticeshipIsPosted()
        {
            await _context.Api.Post("approvals", _createApprenticeshipRequest);
        }

        [Then(@"the result should return bad request")]
        public void ThenTheResultShouldReturnBadRequest()
        {
            _context.Api.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Then(@"the content should contain error list")]
        public async Task ThenTheContentShouldContainErrorList()
        {
            var content = await _context.Api.Response.Content.ReadAsStringAsync();

            var errors = JsonConvert.DeserializeObject<ValidationProblemDetails>(content);
            errors.Errors.Count.Should().BeGreaterOrEqualTo(1);
        }

        [Then(@"the result should return OK")]
        public void ThenTheResultShouldReturnOk()
        {
            _context.Api.Response.Should().Be200Ok();
        }

        [Then(@"the registration exists in database")]
        public void ThenTheRegistrationExistsInDatabase()
        {
            var registration = _context.DbContext.Registrations
                .FirstOrDefault(x => x.RegistrationId == _createApprenticeshipRequest.RegistrationId);
            registration.Should().NotBeNull();
            registration.FirstName.Should().Be(_createApprenticeshipRequest.FirstName);
            registration.LastName.Should().Be(_createApprenticeshipRequest.LastName);
            registration.Email.Should().Be(_createApprenticeshipRequest.Email);
            registration.Apprenticeship.EmployerName.Should().Be(_createApprenticeshipRequest.EmployerName);
            registration.Apprenticeship.EmployerAccountLegalEntityId.Should().Be(_createApprenticeshipRequest.EmployerAccountLegalEntityId);
            registration.CommitmentsApprenticeshipId.Should().Be(_createApprenticeshipRequest.CommitmentsApprenticeshipId);
            registration.Apprenticeship.TrainingProviderName.Should().Be(_createApprenticeshipRequest.TrainingProviderName);
            registration.Apprenticeship.Course.Name.Should().Be(_createApprenticeshipRequest.CourseName);
            registration.Apprenticeship.Course.Level.Should().Be(_createApprenticeshipRequest.CourseLevel);
            registration.Apprenticeship.Course.Option.Should().Be(_createApprenticeshipRequest.CourseOption);
            registration.Apprenticeship.Course.PlannedStartDate.Should().Be(_createApprenticeshipRequest.PlannedStartDate);
            registration.Apprenticeship.Course.PlannedEndDate.Should().Be(_createApprenticeshipRequest.PlannedEndDate);
        }

        [Then("the Confirmation Commenced event is published")]
        public void ThenTheConfirmationStartedEventIsPublished()
        {
            _context.Messages.PublishedMessages.Should().ContainEquivalentOf(new
            {
                Message = new ApprenticeshipConfirmationCommencedEvent
                {
                    ApprenticeId = _createApprenticeshipRequest.RegistrationId,
                    ApprenticeshipId = (long?)null,
                    ConfirmationId = (long?)null,
                    ConfirmationOverdueOn = _createApprenticeshipRequest.CommitmentsApprovedOn.AddDays(Revision.DaysBeforeOverdue),
                    CommitmentsApprovedOn = _createApprenticeshipRequest.CommitmentsApprovedOn,
                    CommitmentsApprenticeshipId = _createApprenticeshipRequest.CommitmentsApprenticeshipId,
                }
            });
        }
    }
}