using AutoFixture;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Commands.CreateApprenticeAccountCommand;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Api.AcceptanceTests.WorkflowTests
{
    public class Step2CreateApprenticeAccount : ApiFixture
    {
        [Test]
        public async Task Validates_command()
        {
            var create = fixture.Build<CreateApprenticeAccountCommand>()
                .Without(p => p.Email)
                .Do(x => x.Email = " ")
                .Create();
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
            var account = await CreateAccount();
            var apprentice = await GetApprentice(account.ApprenticeId);
            apprentice.Should().BeEquivalentTo(new
            {
                account.ApprenticeId,
                account.DateOfBirth,
                account.Email,
                account.FirstName,
                account.LastName,
            });
        }

        [Test]
        public async Task Stores_email_address_history()
        {
            var account = await CreateAccount();

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
            var account = await CreateAccount();

            account.FirstName = fixture.Create("updated first name");
            account.LastName = fixture.Create("updated last name");
            account.DateOfBirth = fixture.Create<DateTime>().Date;

            await UpdateAccount(account.ApprenticeId, account.FirstName, account.LastName, account.DateOfBirth);

            var expectedApprentice = new
            {
                account.FirstName,
                account.LastName,
                account.DateOfBirth,
                Email = new MailAddress(account.Email),
                UpdatedOn = DateTime.UtcNow
            };

            Database.Apprentices.Should().ContainEquivalentOf(
                expectedApprentice, 
                options => options.Using<DateTime>(x => x.Subject.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(2)))
            .When(info => info.Path.EndsWith("UpdatedOn")));
        }

        [Test]
        public async Task Update_apprentice_must_use_valid_values()
        {
            var account = await CreateAccount();

            var response = await SendUpdateAccountRequest(account.ApprenticeId, null, "", DateTime.MinValue);
            response.Should().Be400BadRequest().And.BeAs(new
            {
                Errors = new Dictionary<string, string[]>
                {
                    { "FirstName", new[]{ "Enter your first name" } },
                    { "LastName", new[]{ "Enter your last name" } },
                    { "DateOfBirth", new[]{ "Enter your date of birth" } },
                },
            });

            Database.Apprentices.Should().ContainEquivalentOf(new
            {
                account.FirstName,
                account.LastName,
                account.DateOfBirth,
                Email = new MailAddress(account.Email)
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