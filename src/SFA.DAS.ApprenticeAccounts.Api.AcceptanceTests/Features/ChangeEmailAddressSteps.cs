using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeAccounts.Api.AcceptanceTests.Steps
{
    [Binding]
    [Scope(Feature = "ChangeEmailAddress")]
    public class ChangeEmailAddressSteps
    {
        private readonly TestContext _context;
        private Fixture _fixture = new Fixture();
        private Apprentice _apprentice;
        private string? _mailAddress;

        public ChangeEmailAddressSteps(TestContext context)
        {
            _context = context;
        }

        [Given(@"we have an existing apprentice")]
        public void GivenWeHaveAnExistingApprentice()
        {
            _apprentice = _fixture.Build<Apprentice>()
                .Without(a => a.TermsOfUseAccepted)
                .Without(a=>a.MyApprenticeships)
                .Create();

            _context.DbContext.Apprentices.Add(_apprentice);
            _context.DbContext.SaveChanges();
        }

        [Given(@"a ChangeEmailCommand with a valid email address")]
        public void GivenAChangeEmailCommandWithAValidEmailAddress()
        {
            _mailAddress = _fixture.Create<MailAddress>().ToString();
        }

        [Given(@"a ChangeEmailCommand with an invalid email address")]
        public void GivenAChangeEmailCommandWithAnInvalidEmailAddress()
        {
            _mailAddress = _fixture.Create<long>().ToString();
        }

        [Given("a ChangeEmailCommand with the current email address")]
        public void GivenAChangeEmailCommandWithTheCurrentEmailAddress()
        {
            _mailAddress = _apprentice.Email.ToString();
        }

        [When(@"we change the apprentice's email address")]
        public async Task WhenWeChangeTheApprenticesEmailAddress()
        {
            var patch = new JsonPatchDocument<ApprenticePatchDto>().Replace(x => x.Email, _mailAddress);
            await _context.Api.Patch($"apprentices/{_apprentice.Id}", patch);
        }

        [Then(@"the apprentice record is updated")]
        public void ThenTheApprenticeRecordIsCreated()
        {
            _context.Api.Response.Should().Be2XXSuccessful();
             _context.DbContext.Apprentices.Should().ContainEquivalentOf(new
             {
                 _apprentice.Id,
                 Email = new MailAddress(_mailAddress),
             });
        }

        [Then(@"the apprentice record is not updated")]
        public void ThenTheApprenticeRecordIsNotUpdated()
        {
            _context.DbContext.Apprentices.Should().ContainEquivalentOf(_apprentice);
        }

        [Then(@"the change history is recorded")]
        public void ThenTheChangeHistoryIsRecorded()
        {
            var modified = _context.DbContext
                .Apprentices.Include(x => x.PreviousEmailAddresses)
                .Single(x => x.Id == _apprentice.Id);

            modified.PreviousEmailAddresses.Should().ContainEquivalentOf(new
            {
                EmailAddress = new MailAddress(_mailAddress),
            });
        }
    }
}