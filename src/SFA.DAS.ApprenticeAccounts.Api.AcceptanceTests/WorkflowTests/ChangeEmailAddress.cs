using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Messages.Events;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Api.AcceptanceTests.WorkflowTests
{
    internal class ChangeEmailAddress : ApiFixture
    {
        [Test, AutoData]
        public async Task Can_update_email_address(MailAddress newAddress)
        {
            var account = await CreateAccount();
            var response = await SendUpdateAccountRequest(account.ApprenticeId, newAddress);
            response.Should().Be2XXSuccessful();

            var account_ = await GetApprentice(account.ApprenticeId);
            account_.Should().BeEquivalentTo(new
            {
                account.ApprenticeId,
                Email = newAddress.ToString(),
            });
        }

        [Test, AutoData]
        public async Task Should_not_update_email_if_account_is_null(MailAddress newAddress)
        {
            var account = await CreateNullAccount();
            var response = await SendUpdateAccountRequest(account.ApprenticeId, newAddress);
            response.Should().Be400BadRequest();
        }

        [Test, AutoData]
        public async Task Publishes_event(MailAddress newAddress)
        {
            var account = await CreateAccount();
            await SendUpdateAccountRequest(account.ApprenticeId, newAddress);

            Published.Events.Should().ContainEquivalentOf(new
            {
                account.ApprenticeId,
            }).Which.Should().BeOfType<ApprenticeEmailAddressChanged>();
        }
    }
}