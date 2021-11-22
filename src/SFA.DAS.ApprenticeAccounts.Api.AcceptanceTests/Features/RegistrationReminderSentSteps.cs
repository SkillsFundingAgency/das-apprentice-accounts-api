using System;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using System.Net;
using System.Threading.Tasks;
using SFA.DAS.ApprenticeCommitments.Api.Types;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Features
{
    [Binding]
    [Scope(Feature = "RegistrationReminderSent")]
    public class RegistrationReminderSentSteps
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly TestContext _context;
        private Registration _registration;
        private RegistrationReminderSentRequest _request;

        public RegistrationReminderSentSteps(TestContext context)
        {
            _context = context;
            _registration = _fixture.Create<Registration>();
            _request = _fixture.Create<RegistrationReminderSentRequest>();
        }

        [Given(@"the apprentice has not been sent a reminder")]
        public async Task GivenTheApprenticeHasNotBeenSentAReminder()
        {
            _registration.SetProperty(x => x.SignUpReminderSentOn, null);
            _context.DbContext.Registrations.Add(_registration);
            await _context.DbContext.SaveChangesAsync();
        }

        [Given(@"the apprentice has already been sent a reminder")]
        public async Task GivenTheApprenticeHasAlreadyBeenSentAReminder()
        {
            _registration.SetProperty(x => x.SignUpReminderSentOn, _fixture.Create<DateTime>());
            _context.DbContext.Registrations.Add(_registration);
            await _context.DbContext.SaveChangesAsync();
        }

        [When(@"we receive a request to say reminder has been sent")]
        public async Task WhenWeReceiveARequestToSayReminderHasBeenSent()
        {
            await _context.Api.Post($"registrations/{_registration.RegistrationId}/reminder", _request);
        }

        [Then(@"the response is OK")]
        public void ThenTheResponseIsOk()
        {
            _context.Api.Response.Should().Be200Ok();
        }

        [Then(@"the registration record is updated")]
        public void ThenTheRegistrationRecordIsUpdated()
        {
            var reg = _context.DbContext.Registrations.FirstOrDefault(x=>x.RegistrationId == _registration.RegistrationId);
            reg.Should().NotBeNull();
            reg.SignUpReminderSentOn.Should().Be(_request.SentOn);
        }

        [Then(@"the registration record is not updated")]
        public void ThenTheRegistrationRecordIsNotUpdated()
        {
            var reg = _context.DbContext.Registrations.FirstOrDefault(x => x.RegistrationId == _registration.RegistrationId);
            reg.Should().NotBeNull();
            reg.SignUpReminderSentOn.Should().NotBe(_request.SentOn);
        }
    }
}