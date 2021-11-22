using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.ApprenticeCommitments.Application.Queries.RegistrationRemindersQuery;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.Features
{
    [Binding]
    [Scope(Feature = "GetRegistrationsReminders")]
    public class GetRegistrationsRemindersSteps
    {
        private readonly TestContext _context;
        private Fixture _fixture;
        private List<Registration> _registrations;
        private List<RegistrationTest> _testData;

        public GetRegistrationsRemindersSteps(TestContext context)
        {
            _fixture = new Fixture();
            _context = context;
            _registrations = new List<Registration>();
        }

        [Given(@"the following registration details exist")]
        public async Task GivenTheFollowingRegistrationDetailsExist(Table table)
        {
            _testData = table.CreateSet<RegistrationTest>().ToList();

            foreach (var reg in _testData)
            {
                var registration = _fixture.Create<Registration>();

                registration.SetProperty(x => x.CreatedOn, reg.CreatedOn);
                registration.SetProperty(x => x.FirstName, reg.FirstName);
                registration.SetProperty(x => x.LastName, reg.LastName);
                registration.SetProperty(x => x.Email, new MailAddress(reg.Email));
                registration.SetProperty(x => x.FirstViewedOn, reg.FirstViewedOn);
                registration.SetProperty(x => x.ApprenticeId, reg.UserIdentityId);
                registration.SetProperty(x => x.SignUpReminderSentOn, reg.SignUpReminderSentOn);

                _registrations.Add(registration);
            }

            _context.DbContext.Registrations.AddRange(_registrations);
            await _context.DbContext.SaveChangesAsync();
        }

        [When(@"we want reminders before cut off date (.*)")]
        public async Task WhenWeGetRemindersBeforeCutOffDate(DateTime cutOffTime)
        {
            await _context.Api.Get($"registrations/reminders/?invitationCutOffTime={cutOffTime.ToString("yyy-MM-dd")}");
        }

        [Then(@"the result should return (.*) matching registration")]
        public async Task ThenTheResultShouldReturnMatchingRegistration(int count)
        {
            var content = await _context.Api.Response.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<RegistrationRemindersResponse>(content);
            content.Should().NotBeNull();
            response.Registrations.Count.Should().Be(count);
        }

        [Then(@"there should be a registration with the email (.*) and it's expected values")]
        public async Task ThenThatShouldBeHasARegistrationWithTheEmailAndItSExpectedValues(string email)
        {
            var content = await _context.Api.Response.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<RegistrationRemindersResponse>(content);
            content.Should().NotBeNull();

            var expected = _testData.FirstOrDefault(x => x.Email == email);
            var match = response.Registrations.FirstOrDefault(x => x.Email == email);
            match.Should().NotBeNull();
            match.CreatedOn.Should().Be(expected.CreatedOn);
            match.UserIdentityId.Should().Be(expected.UserIdentityId);
            match.FirstName.Should().Be(expected.FirstName);
            match.LastName.Should().Be(expected.LastName);
        }

        public class RegistrationTest
        {
            public string FirstName;
            public string LastName;
            public string Email;
            public DateTime? CreatedOn;
            public DateTime? FirstViewedOn;
            public DateTime? SignUpReminderSentOn;
            public Guid? UserIdentityId;
        }
    }
}