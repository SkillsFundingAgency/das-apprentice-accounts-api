using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.WorkflowTests;
using SFA.DAS.ApprenticeCommitments.Application.Commands.ChangeRegistrationCommand;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using SFA.DAS.ApprenticeCommitments.Messages.Events;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

#nullable enable

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Features
{
    [Binding]
    [Scope(Feature = "ChangeApprenticeship")]
    public class ChangeApprenticeshipSteps
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly TestContext _context;
        private ChangeRegistrationCommand _request = null!;
        private Revision _revision;
        private long _newApprenticeshipId;
        private long _commitmentsApprenticeshipId;

        public ChangeApprenticeshipSteps(TestContext context)
        {
            _fixture.Customizations.Add(new EmailPropertyCustomisation());
            _context = context;
            _commitmentsApprenticeshipId = _fixture.Create<long>();
            _revision = _fixture.Create<Revision>();
            _revision.SetProperty(p => p.CommitmentsApprenticeshipId, _commitmentsApprenticeshipId);
            _newApprenticeshipId = _fixture.Create<long>();
        }

        [Given("we have an existing apprenticeship")]
        public async Task GivenWeHaveAnExistingApprenticeship()
        {
            var apprentice = _fixture.Create<Apprentice>();
            apprentice.AddApprenticeship(_revision);

            _context.DbContext.Apprentices.Add(apprentice);
            await _context.DbContext.SaveChangesAsync();
        }

        [Given("we do not have an existing apprenticeship, confirmed or unconfirmed")]
        public void GivenWeDoNotHaveAnExistingApprenticeshipConfirmedOrUnconfirmed()
        {
        }

        [Given("we do not have an existing apprenticeship")]
        public void GivenWeDoNotHaveAnExistingApprenticeship()
        {
        }

        [Given(@"we do have a verified registration")]
        public async Task GivenWeDoHaveAVerifiedRegistration()
        {
            var registration = _fixture.Create<Registration>();
            registration.SetProperty(x => x.CommitmentsApprenticeshipId, _commitmentsApprenticeshipId);
            registration.SetProperty(x => x.ApprenticeId, Guid.NewGuid());

            _context.DbContext.Registrations.Add(registration);
            await _context.DbContext.SaveChangesAsync();
        }

        [Given(@"we do have an unconfirmed registration")]
        public async Task GivenWeDoHaveAnUnconfirmedRegistration()
        {
            var registration = _fixture.Create<Registration>();
            registration.SetProperty(x => x.CommitmentsApprenticeshipId, _commitmentsApprenticeshipId);

            _context.DbContext.Registrations.Add(registration);
            await _context.DbContext.SaveChangesAsync();
        }

        [Given("we have an update apprenticeship request")]
        public void GivenWeHaveAnUpdateApprenticeshipRequest()
        {
            var start = _fixture.Create<DateTime>();
            _request = _fixture.Build<ChangeRegistrationCommand>()
                .Without(x => x.CommitmentsContinuedApprenticeshipId)
                .With(x => x.CommitmentsApprenticeshipId, _commitmentsApprenticeshipId)
                .With(x => x.CommitmentsApprovedOn, (long days) => _revision.CommitmentsApprovedOn.AddDays(days))
                .With(x => x.PlannedStartDate, start)
                .With(x => x.PlannedEndDate, (long days) => start.AddDays(days + 1))
                .Create();
        }

        [Given("we have an update apprenticeship request without an email")]
        public void GivenWeHaveAnUpdateApprenticeshipRequestWithoutAnEmail()
        {
            var start = _fixture.Create<DateTime>();
            _request = _fixture.Build<ChangeRegistrationCommand>()
                .Without(x => x.CommitmentsContinuedApprenticeshipId)
                .With(x => x.CommitmentsApprenticeshipId, _commitmentsApprenticeshipId)
                .With(x => x.CommitmentsApprovedOn, (long days) => _revision.CommitmentsApprovedOn.AddDays(days))
                .With(x => x.PlannedStartDate, start)
                .With(x => x.PlannedEndDate, (long days) => start.AddDays(days + 1))
                .Without(x => x.Email)
                .Create();
        }

        [Given("we have an update apprenticeship request with no material change")]
        public void GivenWeHaveAnInconsequenticalUpdateApprenticeshipRequest()
        {
            _request = _fixture.Build<ChangeRegistrationCommand>()
                .Without(x => x.CommitmentsContinuedApprenticeshipId)
                .With(x => x.CommitmentsApprenticeshipId, _commitmentsApprenticeshipId)
                .With(x => x.EmployerAccountLegalEntityId, _revision.Details.EmployerAccountLegalEntityId)
                .With(x => x.EmployerName, _revision.Details.EmployerName)
                .With(x => x.TrainingProviderId, _revision.Details.TrainingProviderId)
                .With(x => x.TrainingProviderName, _revision.Details.TrainingProviderName)
                .With(x => x.CourseName, _revision.Details.Course.Name)
                .With(x => x.CourseLevel, _revision.Details.Course.Level)
                .With(x => x.CourseOption, _revision.Details.Course.Option)
                .With(x => x.PlannedStartDate, _revision.Details.Course.PlannedStartDate)
                .With(x => x.PlannedEndDate, _revision.Details.Course.PlannedEndDate)
                .Create();
        }

        [Given("we have a update apprenticeship continuation request")]
        public void GivenWeHaveANewApprenticeshipRequest()
        {
            var start = _fixture.Create<DateTime>();
            _request = _fixture.Build<ChangeRegistrationCommand>()
                .With(x => x.CommitmentsContinuedApprenticeshipId, _commitmentsApprenticeshipId)
                .With(x => x.CommitmentsApprenticeshipId, _newApprenticeshipId)
                .With(x => x.PlannedStartDate, start)
                .With(x => x.PlannedEndDate, (long days) => start.AddDays(days))
                .Create();
        }

        [When("the update is posted")]
        public async Task WhenTheUpdateIsPosted()
        {
            await _context.Api.Put("approvals", _request);
        }

        [Then("the result should return OK")]
        public void ThenTheResultShouldReturnOk()
        {
            _context.Api.Response.Should().Be200Ok();
        }

        [Then("the result should return Not Found")]
        public void ThenTheResultShouldReturnNotFound()
        {
            _context.Api.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Then("the new revision exists in database")]
        public void ThenTheRevisionExistsInDatabase()
        {
            var cs = _context.DbContext.Revisions.ToList();

            _context.DbContext.Revisions.Should().ContainEquivalentOf(new
            {
                _request.CommitmentsApprovedOn,
                Details = new
                {
                    _request.EmployerAccountLegalEntityId,
                    _request.EmployerName,
                    _request.TrainingProviderId,
                    _request.TrainingProviderName,
                    Course = new
                    {
                        Name = _request.CourseName,
                        Level = _request.CourseLevel,
                        Option = _request.CourseOption,
                        _request.PlannedStartDate,
                        _request.PlannedEndDate,
                    },
                },
                TrainingProviderCorrect = (bool?)null,
                EmployerCorrect = (bool?)null,
                RolesAndResponsibilitiesCorrect = (bool?)null,
                ApprenticeshipDetailsCorrect = (bool?)null,
                HowApprenticeshipDeliveredCorrect = (bool?)null,
                ApprenticeshipConfirmed = false,
            });
        }

        [Then(@"we have updated the apprenticeship details for the unconfirmed registration")]
        public void ThenWeHaveUpdatedTheApprenticeshipDetailsForTheUnconfirmedRegistration()
        {
            _context.DbContext.Registrations.Should().ContainEquivalentOf(new
            {
                Apprenticeship = new
                {
                    _request.EmployerAccountLegalEntityId,
                    _request.EmployerName,
                    _request.TrainingProviderId,
                    _request.TrainingProviderName,
                    Course = new
                    {
                        Name = _request.CourseName,
                        Level = _request.CourseLevel,
                        Option = _request.CourseOption,
                        _request.PlannedStartDate,
                        _request.PlannedEndDate,
                    },
                }
            });
        }

        [Then("the new revision has same commitments apprenticeship Id")]
        public void ThenTheCommitmentRevisionHasSameCommitmentsApprenticeshipId()
        {
            _context.DbContext.Revisions.Should().ContainEquivalentOf(new
            {
                _request.CommitmentsApprovedOn,
                _revision.CommitmentsApprenticeshipId
            });
        }

        [Then(@"the new revision has a new commitments apprenticeship Id")]
        public void ThenTheNewCommitmentRevisionHasANewCommitmentsApprenticeshipId()
        {
            _context.DbContext.Revisions.Should().ContainEquivalentOf(new
            {
                _request.CommitmentsApprovedOn,
                _request.CommitmentsApprenticeshipId
            });
        }

        [Then(@"registration commitments apprenticeship are updated correctly")]
        public void ThenRegistrationCommitmentsApprenticeshipAreUpdatedCorrectly()
        {
            _context.DbContext.Registrations.Should().ContainEquivalentOf(new
            {
                _request.CommitmentsApprovedOn,
                _request.CommitmentsApprenticeshipId,
                _request.FirstName,
                _request.LastName,
                _request.DateOfBirth
            });
        }

        [Then(@"a new registration record should exist with the correct information")]
        public void ThenANewRegistrationRecordShouldExistWithTheCorrectInformation()
        {
            _context.DbContext.Registrations.Should().ContainEquivalentOf(new
            {
                _request.CommitmentsApprovedOn,
                _request.CommitmentsApprenticeshipId,
                _request.FirstName,
                _request.LastName,
                _request.DateOfBirth,
                Apprenticeship = new
                {
                    _request.EmployerAccountLegalEntityId,
                    _request.EmployerName,
                    _request.TrainingProviderId,
                    _request.TrainingProviderName,
                    Course = new
                    {
                        Name = _request.CourseName,
                        Level = _request.CourseLevel,
                        Option = _request.CourseOption,
                        _request.PlannedStartDate,
                        _request.PlannedEndDate,
                    },
                }
            });
        }

        [Then("there should only be the original revision in the database")]
        public void ThenThereShouldOnlyBeTheOriginalCommitmentRevisionInTheDatabase()
        {
            _context.DbContext.Revisions.Should().HaveCount(1);
        }

        [Then("there should be no revisions in the database")]
        public void ThenThereShouldBeNoCommitmentRevisionsInTheDatabase()
        {
            _context.DbContext.Revisions.Should().BeEmpty();
        }

        [Then(@"a domain exception is thrown: ""(.*)""")]
        public async Task ThenADomainExceptionIsThrown(string detail)
        {
            var content = await _context.Api.Response.Content.ReadAsStringAsync();
            var problem = JsonConvert.DeserializeObject<ValidationProblemDetails>(content);
            problem.Detail.Should().StartWith(detail);
        }

        [Then(@"a validation exception is thrown for the field: ""(.*)""")]
        public async Task ThenAValidationExceptionIsThrownForTheField(string field)
        {
            var content = await _context.Api.Response.Content.ReadAsStringAsync();
            var problem = JsonConvert.DeserializeObject<ValidationProblemDetails>(content);
            problem.Errors.Should().ContainKey(field);
        }

        [Then("the response is bad request")]
        public void ThenTheResponseIsOK()
        {
            _context.Api.Response.Should().Be400BadRequest();
        }

        [Then("the Confirmation Commenced event is published")]
        public void ThenTheConfirmationStartedEventIsPublished()
        {
            var latest = _context.DbContext.Revisions.OrderBy(x => x.Id).Last();

            _context.Messages.PublishedMessages.Should().ContainEquivalentOf(new
            {
                Message = new ApprenticeshipConfirmationCommencedEvent
                {
                    ApprenticeId = _context.DbContext.Apprentices.Single().Id,
                    ApprenticeshipId = _revision.ApprenticeshipId,
                    ConfirmationId = latest.Id,
                    ConfirmationOverdueOn = latest.ConfirmBefore,
                    CommitmentsApprovedOn = _request.CommitmentsApprovedOn,
                    CommitmentsApprenticeshipId = _request.CommitmentsApprenticeshipId,
                }
            });
        }

        [Then("no Confirmation Commenced event is published")]
        public void ThenNoConfirmationCommencedEventIsPublished()
        {
            _context.Messages.PublishedMessages
                .Select(x => x.Message is ApprenticeshipConfirmationCommencedEvent)
                .Should().BeEmpty();
        }

        [Then("send a Change of Circumstance email to the user")]
        public void ThenSendAChangeOfCircumstanceEmailToTheUser()
        {
            var latest = _context.DbContext.Revisions.OrderBy(x => x.Id).Last();

            _context.Messages.PublishedMessages
                .Where(x => x.Message is ApprenticeshipChangedEvent)
                .Should().ContainEquivalentOf(new
                {
                    Message = new
                    {
                        ApprenticeId = _context.DbContext.Apprentices.Single().Id,
                        _revision.ApprenticeshipId,
                    }
                });
        }

        [Then(@"send a Apprenticeship Registered Event")]
        public void ThenSendAApprenticeshipRegisteredEvent()
        {
            var registration = _context.DbContext.Registrations.FirstOrDefault(x => x.CommitmentsApprenticeshipId == _commitmentsApprenticeshipId);

            _context.Messages.PublishedMessages
                .Where(x => x.Message is ApprenticeshipRegisteredEvent)
                .Should().ContainEquivalentOf(new
                {
                    Message = new
                    {
                        RegistrationId = registration.RegistrationId,
                    }
                });
        }
    }
}