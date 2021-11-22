using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.ApprenticeCommitments.Application.Commands.CreateApprenticeAccountCommand;
using SFA.DAS.ApprenticeCommitments.Application.Commands.CreateRegistrationCommand;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.WorkflowTests
{
    public class Step2CreateApprenticeAccount : ApiFixture
    {
        [Test]
        public async Task Validates_command()
        {
            var create = fixture.Build<CreateApprenticeAccountCommand>()
                .Without(p => p.Email).
                Create();
            var response = await PostCreateAccountCommand(create);
            response
                .Should().Be400BadRequest()
                .And.MatchInContent("*Email*");
        }

        [Test]
        public async Task Cannot_retrieve_missing_apprentice()
        {
            var response = await client.GetAsync($"apprentices/{Guid.NewGuid()}");
            response.Should().Be404NotFound();
        }

        [Test]
        public async Task Can_retrieve_apprentice()
        {
            var approval = fixture.Create<CreateRegistrationCommand>();
            var account = await CreateAccount(approval);
            var apprentice = await GetApprentice(account.ApprenticeId);
            apprentice.Should().BeEquivalentTo(new
            {
                Id = account.ApprenticeId,
                approval.DateOfBirth,
                approval.Email,
                approval.FirstName,
                approval.LastName,
            });
        }

        [Test]
        public async Task Stores_email_address_history()
        {
            var approval = fixture.Create<CreateRegistrationCommand>();

            var account = await CreateAccount(approval);

            var apprentice = await Database.Apprentices
                .Include(x => x.PreviousEmailAddresses)
                .FirstOrDefaultAsync(x => x.Id == account.ApprenticeId);
            apprentice.PreviousEmailAddresses.Should().ContainEquivalentOf(new
            {
                EmailAddress = new MailAddress(account.Email),
            });
        }

        [Test]
        public async Task Can_update_apprentice_before_matched()
        {
            var approval = fixture.Create<CreateRegistrationCommand>();
            var account = await CreateAccount(approval);

            account.FirstName = fixture.Create("updated first name");
            account.LastName = fixture.Create("updated last name");
            account.DateOfBirth = fixture.Create<DateTime>().Date;

            await UpdateAccount(account.ApprenticeId, account.FirstName, account.LastName, account.DateOfBirth);

            Database.Apprentices.Should().ContainEquivalentOf(new
            {
                account.FirstName,
                account.LastName,
                account.DateOfBirth,
                Email = new MailAddress(account.Email),
            });
        }

        [Test]
        public async Task Update_apprentice_must_use_valid_values()
        {
            var approval = fixture.Create<CreateRegistrationCommand>();
            var account = await CreateAccount(approval);

            var response = await SendUpdateAccountRequest(account.ApprenticeId, null, "", DateTime.MinValue);
            response.Should().Be400BadRequest().And.BeAs(new
            {
                Errors = new Dictionary<string, string[]>
                {
                    { "FirstName", new[]{ "'First Name' must not be empty." } },
                    { "LastName", new[]{ "'Last Name' must not be empty." } },
                    { "DateOfBirth", new[]{ "'Date Of Birth' is not a valid date." } },
                },
            });

            Database.Apprentices.Should().ContainEquivalentOf(new
            {
                account.FirstName,
                account.LastName,
                account.DateOfBirth,
                Email = new MailAddress(account.Email),
            });
        }

        [Test]
        public async Task Update_apprentice_accept_terms_of_use()
        {
            var account = await CreateAccount();

            var response = await SendAcceptTermsOfUseRequest(account.ApprenticeId);
            response.Should().Be2XXSuccessful();

            Database.Apprentices.Should().ContainEquivalentOf(new
            {
                Id = account.ApprenticeId,
                TermsOfUseAccepted = true,
            });
        }

        [Test]
        public async Task Update_apprentice_decline_terms_of_use()
        {
            var account = await CreateAccount();

            var response = await SendAcceptTermsOfUseRequest(account.ApprenticeId, accept: false);
            
            response.Should().Be500InternalServerError();
        }
    }
}