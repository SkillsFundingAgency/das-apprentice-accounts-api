using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApprenticeshipFromRegistrationCommand;
using SFA.DAS.ApprenticeCommitments.Data.FuzzyMatching;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using SFA.DAS.ApprenticeCommitments.Messages.Events;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "VerifyRegistration")]
    public class VerifyRegistrationSteps
    {
        private readonly TestContext _context;
        private CreateApprenticeshipFromRegistrationCommand _command;
        private Fixture _f;
        private Registration _registration;
        private Apprentice _apprentice;

        public VerifyRegistrationSteps(TestContext context)
        {
            _context = context;
            _f = new Fixture();
        }

        [Given(@"we have an existing registration")]
        public void GivenWeHaveAnExistingRegistration()
        {
            _registration = _f.Create<Registration>();
            _context.DbContext.Registrations.Add(_registration);
            _context.DbContext.SaveChanges();
        }

        [Given("we have a matching account")]
        public void GivenWeHaveAnExistingAccount()
        {
            _apprentice = new Apprentice(
                _f.Create<Guid>(),
                _registration.FirstName,
                _registration.LastName,
                _registration.Email,
                _registration.DateOfBirth
                                        );
            _context.DbContext.Apprentices.Add(_apprentice);
            _context.DbContext.SaveChanges();
        }

        [Given("we have an account with a non-matching email")]
        public void GivenWeHaveAnAccountWithANon_MatchingEmail()
        {
            _apprentice = new Apprentice(
                _f.Create<Guid>(),
                _registration.FirstName,
                _registration.LastName,
                new MailAddress("another@email.com"),
                _registration.DateOfBirth
                                        );
            _context.DbContext.Apprentices.Add(_apprentice);
            _context.DbContext.SaveChanges();
        }

        [Given("we have an account with a non-matching date of birth")]
        public void GivenWeHaveAnAccountWithANon_MatchingDateOfBirth()
        {
            _apprentice = new Apprentice(
                _f.Create<Guid>(),
                _registration.FirstName,
                _registration.LastName,
                _registration.Email,
                _registration.DateOfBirth.AddDays(90)
                                        );
            _context.DbContext.Apprentices.Add(_apprentice);
            _context.DbContext.SaveChanges();
        }

        [Given("the request matches registration details")]
        [Given("the request is for the account")]
        public void GivenTheRequestMatchesRegistrationDetails()
        {
            _command = _f.Build<CreateApprenticeshipFromRegistrationCommand>()
                .With(p => p.ApprenticeId, _apprentice.Id)
                .With(p => p.RegistrationId, _registration.RegistrationId)
                .Create();
        }

        [Given("the request is for a different account")]
        public void GivenTheRequestIsForADifferentAccount()
        {
            var apprentice = _f.Create<Apprentice>();
            _context.DbContext.Apprentices.Add(apprentice);
            _context.DbContext.SaveChanges();

            _command = _f.Build<CreateApprenticeshipFromRegistrationCommand>()
                .With(p => p.ApprenticeId, apprentice.Id)
                .With(p => p.RegistrationId, _registration.RegistrationId)
                .Create();
        }

        [Given(@"we have an existing already verified registration")]
        public void GivenWeHaveAnExistingAlreadyVerifiedRegistration()
        {
            _registration = _f.Create<Registration>();
            GivenWeHaveAnExistingAccount();
            _registration.AssociateWithApprentice(_apprentice, FuzzyMatcher.AlwaysMatcher);
            _context.DbContext.Registrations.Add(_registration);
            _context.DbContext.SaveChanges();
        }

        [Given(@"the verify registration request is invalid")]
        public void GivenTheVerifyRegistrationRequestIsInvalid()
        {
            _command = null;
        }

        [Given(@"we do NOT have an existing registration")]
        public void GivenWeDoNOTHaveAnExistingRegistration()
        {
        }

        [Given(@"a valid registration request is submitted")]
        public void GivenAValidRegistrationRequestIsSubmitted()
        {
            _command = _f.Create<CreateApprenticeshipFromRegistrationCommand>();
        }

        [When(@"we verify that registration")]
        public async Task WhenWeVerifyThatRegistration()
        {
            await _context.Api.Post("apprenticeships", _command);
        }

        [Then(@"the apprentice record is created")]
        public void ThenTheApprenticeRecordIsCreated()
        {
            var apprentice = _context.DbContext.Apprentices.FirstOrDefault(x => x.Id == _command.ApprenticeId);
            apprentice.Should().NotBeNull();
            apprentice.FirstName.Should().Be(_apprentice.FirstName);
            apprentice.LastName.Should().Be(_apprentice.LastName);
            apprentice.Email.Should().Be(_apprentice.Email);
            apprentice.DateOfBirth.Should().Be(_apprentice.DateOfBirth);
            apprentice.Id.Should().Be(_command.ApprenticeId);
        }

        [Then("an apprenticeship record is not yet created")]
        public void ThenAnApprenticeshipRecordIsNotYetCreated()
        {
            var apprentice = _context.DbContext
                .Apprentices.Include(x => x.Apprenticeships).ThenInclude(x => x.Revisions)
                .Should().Contain(x => x.Id == _command.ApprenticeId)
                .Which.Apprenticeships.Should().BeEmpty();
        }

        [Then(@"an apprenticeship record is created")]
        public void ThenAnApprenticeshipRecordIsCreated()
        {
            var apprentice = _context.DbContext
                .Apprentices.Include(x => x.Apprenticeships).ThenInclude(x => x.Revisions)
                .FirstOrDefault(x => x.Id == _command.ApprenticeId);

            apprentice.Apprenticeships.SelectMany(a => a.Revisions)
                .Should().ContainEquivalentOf(new
                {
                    CommitmentsApprenticeshipId = _registration.CommitmentsApprenticeshipId,
                    Details = new
                    {
                        _registration.Apprenticeship.EmployerName,
                        _registration.Apprenticeship.EmployerAccountLegalEntityId,
                        _registration.Apprenticeship.TrainingProviderId,
                        _registration.Apprenticeship.TrainingProviderName,
                        Course = new
                        {
                            _registration.Apprenticeship.Course.Name,
                            _registration.Apprenticeship.Course.Level,
                            _registration.Apprenticeship.Course.Option,
                            _registration.Apprenticeship.Course.PlannedStartDate,
                            _registration.Apprenticeship.Course.PlannedEndDate,
                        }
                    },
                });
        }

        [Then("the Confirmation Commenced event is published")]
        public void ThenTheConfirmationStartedEventIsPublished()
        {
            var latest = _context.DbContext.Revisions.Single();

            _context.Messages.PublishedMessages.Should().ContainEquivalentOf(new
            {
                Message = new ApprenticeshipConfirmationCommencedEvent
                {
                    ApprenticeId = _registration.RegistrationId,
                    ApprenticeshipId = latest.ApprenticeshipId,
                    ConfirmationId = latest.Id,
                    ConfirmationOverdueOn = latest.ConfirmBefore,
                    CommitmentsApprovedOn = _registration.CommitmentsApprovedOn,
                    CommitmentsApprenticeshipId = _registration.CommitmentsApprenticeshipId,
                }
            });
        }

        [Then("the response is OK")]
        public void ThenTheResponseIsOK() => _context.Api.Response.Should().Be2XXSuccessful();

        [Then(@"the registration has been marked as completed")]
        public void ThenTheRegistrationHasBeenMarkedAsCompleted()
        {
            var registration = _context.DbContext.Registrations.FirstOrDefault(x => x.RegistrationId == _registration.RegistrationId);
            registration.ApprenticeId.Should().Be(_command.ApprenticeId);
        }

        [Then(@"the registration CreatedOn field is unchanged")]
        public void ThenTheRegistrationCreatedOnFieldIsUnchanged()
        {
            _context.DbContext.Registrations.Should().ContainEquivalentOf(new
            {
                _registration.RegistrationId,
                _registration.CreatedOn
            });
        }

        [Then(@"the apprenticeship email address confirmed event is published")]
        public void ThenTheApprenticeshipEmailAddressConfirmedEventIsPublished()
        {
            _context.Messages.PublishedMessages.Should().ContainEquivalentOf(new
            {
                Message = new ApprenticeshipEmailAddressConfirmedEvent
                {
                    ApprenticeId = _context.DbContext.Apprentices.Single().Id,
                    CommitmentsApprenticeshipId = _registration.CommitmentsApprenticeshipId,
                }
            });
        }

        [Then(@"a bad request is returned")]
        public void ThenABadRequestIsReturned()
        {
            _context.Api.Response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Then(@"a email domain error is returned")]
        public async Task ThenAEmailDomainErrorIsReturned()
        {
            var content = await _context.Api.Response.Content.ReadAsStringAsync();
            var errors = JsonConvert.DeserializeObject<ValidationProblemDetails>(content);
            errors.Errors.Should().ContainKey("PersonalDetails")
                .WhichValue.Should().Contain("Sorry, your identity has not been verified, please check your details");
        }

        [Then(@"an identity mismatch domain error is returned")]
        public async Task ThenAnIdentityMismatchDomainErrorIsReturned()
        {
            var content = await _context.Api.Response.Content.ReadAsStringAsync();
            var errors = JsonConvert.DeserializeObject<ValidationProblemDetails>(content);
            errors.Errors.Should().ContainKey("PersonalDetails")
                .WhichValue.Should().Contain("Sorry, your identity has not been verified, please check your details");
        }

        [Then(@"an 'already verified' domain error is returned")]
        public async Task ThenAnAlreadyVerifiedDomainErrorIsReturned()
        {
            var content = await _context.Api.Response.Content.ReadAsStringAsync();
            content.Should().Contain($"Registration {_registration.RegistrationId} is already verified");
        }

        [Then("response contains the expected error messages")]
        public async Task ThenResponseContainsTheExpectedErrorMessages()
        {
            var content = await _context.Api.Response.Content.ReadAsStringAsync();
            var errors = JsonConvert.DeserializeObject<ValidationProblemDetails>(content);

            errors.Errors.Should().ContainKey("")
                .WhichValue.Should().Contain("A non-empty request body is required.");
        }

        [Then(@"response contains the not found error message")]
        public async Task ThenResponseContainsTheNotFoundErrorMessage()
        {
            var content = await _context.Api.Response.Content.ReadAsStringAsync();
            var errors = JsonConvert.DeserializeObject<ProblemDetails>(content);
            errors.Should().BeEquivalentTo(new
            {
                Detail = $"Registration for Apprentice {_command.RegistrationId} not found",
            });
        }

        [Then("do not send a Change of Circumstance email to the user")]
        public void ThenDoNotSendAChangeOfCircumstanceEmailToTheUser()
        {
            _context.Messages.PublishedMessages
                .Should().NotContain(x => x.Message is ApprenticeshipChangedEvent);
        }
    }
}