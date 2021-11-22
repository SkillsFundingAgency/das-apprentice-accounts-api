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
    [Scope(Feature = "RegistrationFirstSeen")]
    public class RegistrationFirstSeenSteps
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly TestContext _context;
        private Registration _registration;
        private RegistrationFirstSeenRequest _request;

        public RegistrationFirstSeenSteps(TestContext context)
        {
            _context = context;
            _registration = _fixture.Create<Registration>();
            _request = _fixture.Create<RegistrationFirstSeenRequest>();
        }

        [Given(@"this is the first time the apprentice has seen the identity flow")]
        public async Task GivenThisIsTheFirstTimeTheApprenticeHasSeenTheIdentityFlow()
        {
            _registration.SetProperty(x => x.FirstViewedOn, null);
            _context.DbContext.Registrations.Add(_registration);
            await _context.DbContext.SaveChangesAsync();
        }

        [Given(@"this is not the first time the apprentice has seen the identity flow")]
        public async Task GivenThisIsNotTheFirstTimeTheApprenticeHasSeenTheIdentityFlow()
        {
            _registration.SetProperty(x => x.FirstViewedOn, _fixture.Create<DateTime>());
            _context.DbContext.Registrations.Add(_registration);
            await _context.DbContext.SaveChangesAsync();
        }

        [When(@"we receive a request to mark registration as been viewed")]
        public async Task WhenWeReceiveARequestToMarkRegistrationAsBeenViewed()
        {
            await _context.Api.Post($"registrations/{_registration.RegistrationId}/firstseen", _request);
        }

        [Then(@"the response is OK")]
        public void ThenTheResponseIsOk()
        {
            _context.Api.Response.Should().Be200Ok();
        }

        [Then(@"the registration record is updated")]
        public void ThenTheRegistrationRecordIsUpdated()
        {
            var reg = _context.DbContext.Registrations.FirstOrDefault(x => x.RegistrationId == _registration.RegistrationId);
            reg.Should().NotBeNull();
            reg.FirstViewedOn.Should().Be(_request.SeenOn);
        }

        [Then(@"the registration record is not updated")]
        public void ThenTheRegistrationRecordIsNotUpdated()
        {
            var reg = _context.DbContext.Registrations.FirstOrDefault(x => x.RegistrationId == _registration.RegistrationId);
            reg.Should().NotBeNull();
            reg.FirstViewedOn.Should().NotBe(_request.SeenOn);
        }
    }
}