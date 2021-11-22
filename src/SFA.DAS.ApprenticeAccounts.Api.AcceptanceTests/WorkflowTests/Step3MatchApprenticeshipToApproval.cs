using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.WorkflowTests
{
    public class Step3MatchApprenticeshipToApproval : ApiFixture
    {
        [Test]
        public async Task Cannot_find_registration_that_doesnt_exist()
        {
            var response = await PostVerifyRegistrationCommand(Guid.NewGuid(), Guid.NewGuid());

            response
                .Should().Be400BadRequest()
                .And.MatchInContent("*\"Registration for Apprentice * not found\"*");
        }

        [Test]
        public async Task Cannot_find_apprentice_that_doesnt_exist()
        {
            var approval = await CreateRegistration();

            var response = await PostVerifyRegistrationCommand(approval.RegistrationId, Guid.NewGuid());

            response
                .Should().Be400BadRequest()
                .And.MatchInContent("*\"Apprentice * not found\"*");
        }

        [Test]
        public async Task Can_match_incorrect_email()
        {
            var approval = await CreateRegistration();

            var account = await CreateAccount(approval, email: fixture.Create<MailAddress>());
            var response = await PostVerifyRegistrationCommand(approval.RegistrationId, account.ApprenticeId);

            response.Should().Be2XXSuccessful();
        }

        [Test]
        public async Task Cannot_match_incorrect_date_of_birth()
        {
            var approval = await CreateRegistration();

            var account = await CreateAccount(approval, dateOfBirth: fixture.Create<DateTime>());
            var response = await PostVerifyRegistrationCommand(approval.RegistrationId, account.ApprenticeId);

            response
                .Should().Be400BadRequest()
                .And.MatchInContent("*\"Sorry, your identity has not been verified, please check your details\"*");
        }

        [Test]
        public async Task Valid_match_creates_apprenticeship()
        {
            var approval = await CreateRegistration();
            var account = await CreateAccount(approval);

            await VerifyRegistration(approval.RegistrationId, account.ApprenticeId);

            var apprenticeships = await GetApprenticeships(account.ApprenticeId);
            apprenticeships.Should().ContainEquivalentOf(new
            {
                account.ApprenticeId,
                approval.CommitmentsApprenticeshipId,
                //approval.CommitmentsApprovedOn,
                EmployerCorrect = (bool?)null,
                TrainingProviderCorrect = (bool?)null,
                ApprenticeshipDetailsCorrect = (bool?)null,
                HowApprenticeshipDeliveredCorrect = (bool?)null,
                RolesAndResponsibilitiesCorrect = (bool?)null,
            });
        }

        [Test]
        public async Task Second_match_for_same_apprentice_is_accepted()
        {
            var approval = await CreateRegistration();
            var account = await CreateAccount(approval);
            await VerifyRegistration(approval.RegistrationId, account.ApprenticeId);

            var response = await PostVerifyRegistrationCommand(approval.RegistrationId, account.ApprenticeId);

            response.Should().Be2XXSuccessful();
        }

        [Test]
        public async Task Cannot_match_a_different_apprentice_against_already_matched_registration()
        {
            var approval = await CreateRegistration();
            var firstAccount = await CreateAccount(approval);
            await VerifyRegistration(approval.RegistrationId, firstAccount.ApprenticeId);
            var secondAccount = await CreateAccount();

            var response = await PostVerifyRegistrationCommand(approval.RegistrationId, secondAccount.ApprenticeId);

            response
                .Should().Be400BadRequest()
                .And.MatchInContent("*\"Registration * is already verified\"*");
        }
    }
}