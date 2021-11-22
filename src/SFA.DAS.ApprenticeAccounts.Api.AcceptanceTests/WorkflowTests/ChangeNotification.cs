using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.WorkflowTests
{
    public class ChangeNotification : ChangeNotificationFixture
    {
        [Test]
        public async Task Incomplete_and_not_changed_does_not_show_notification()
        {
            var apprenticeship = await CreateVerifiedApprenticeship();

            var retrieved = await GetApprenticeship(apprenticeship);
            retrieved.Should().BeEquivalentTo(new
            {
                ChangeOfCircumstanceNotifications = ChangeOfCircumstanceNotifications.None,
            });
        }

        [Test]
        public async Task Incomplete_and_then_changed_shows_notification()
        {
            var apprenticeship = await CreateVerifiedApprenticeship();
            await ViewApprenticeship(apprenticeship);

            await ChangeApprenticeship(new ChangeBuilder(apprenticeship));

            var retrieved = await GetApprenticeship(apprenticeship);
            retrieved.ChangeOfCircumstanceNotifications.Should().HaveFlag(ChangeOfCircumstanceNotifications.EmployerDetailsChanged);
            retrieved.ChangeOfCircumstanceNotifications.Should().HaveFlag(ChangeOfCircumstanceNotifications.ProviderDetailsChanged);
            retrieved.ChangeOfCircumstanceNotifications.Should().HaveFlag(ChangeOfCircumstanceNotifications.ApprenticeshipDetailsChanged);
        }

        [Test]
        public async Task Confirmed_and_not_changed_does_not_show_notification()
        {
            var apprenticeship = await CreateVerifiedApprenticeship();

            await ConfirmApprenticeship(apprenticeship, new ConfirmationBuilder());

            var retrieved = await GetApprenticeship(apprenticeship);
            retrieved.Should().BeEquivalentTo(new
            {
                ChangeOfCircumstanceNotifications = ChangeOfCircumstanceNotifications.None,
            });
        }

        [Test]
        public async Task Confirmed_and_then_changed_shows_notification()
        {
            var apprenticeship = await CreateVerifiedApprenticeship();
            await ViewApprenticeship(apprenticeship);

            await ConfirmApprenticeship(apprenticeship, new ConfirmationBuilder());
            await ChangeApprenticeship(new ChangeBuilder(apprenticeship));
            var retrieved = await GetApprenticeship(apprenticeship);
            retrieved.ChangeOfCircumstanceNotifications.Should().HaveFlag(ChangeOfCircumstanceNotifications.EmployerDetailsChanged);
            retrieved.ChangeOfCircumstanceNotifications.Should().HaveFlag(ChangeOfCircumstanceNotifications.ProviderDetailsChanged);
            retrieved.ChangeOfCircumstanceNotifications.Should().HaveFlag(ChangeOfCircumstanceNotifications.ApprenticeshipDetailsChanged);
        }

        [Test]
        public async Task Negatively_confirmed_and_not_changed_does_not_show_notification()
        {
            var apprenticeship = await CreateVerifiedApprenticeship();

            await ConfirmApprenticeship(apprenticeship, new ConfirmationBuilder());

            var retrieved = await GetApprenticeship(apprenticeship);
            retrieved.Should().BeEquivalentTo(new
            {
                ChangeOfCircumstanceNotifications = ChangeOfCircumstanceNotifications.None,
            });
        }

        [Test]
        public async Task Negatively_confirmed_and_then_changed_shows_notification()
        {
            var apprenticeship = await CreateVerifiedApprenticeship();
            await ViewApprenticeship(apprenticeship);

            await ConfirmApprenticeship(apprenticeship, new ConfirmationBuilder());
            await ChangeApprenticeship(new ChangeBuilder(apprenticeship));

            var retrieved = await GetApprenticeship(apprenticeship);
            retrieved.ChangeOfCircumstanceNotifications.Should().HaveFlag(ChangeOfCircumstanceNotifications.EmployerDetailsChanged);
            retrieved.ChangeOfCircumstanceNotifications.Should().HaveFlag(ChangeOfCircumstanceNotifications.ProviderDetailsChanged);
            retrieved.ChangeOfCircumstanceNotifications.Should().HaveFlag(ChangeOfCircumstanceNotifications.ApprenticeshipDetailsChanged);
        }

        [Test]
        public async Task One_section_confirmed_and_then_all_changed_shows_notification()
        {
            var apprenticeship = await CreateVerifiedApprenticeship();
            await ViewApprenticeship(apprenticeship);

            await ConfirmApprenticeship(apprenticeship, new ConfirmationBuilder());
            await ChangeApprenticeship(new ChangeBuilder(apprenticeship).OnlyChangeEmployer());

            var retrieved = await GetApprenticeship(apprenticeship);
            retrieved.ChangeOfCircumstanceNotifications.Should().HaveFlag(ChangeOfCircumstanceNotifications.EmployerDetailsChanged);
            retrieved.ChangeOfCircumstanceNotifications.Should().NotHaveFlag(ChangeOfCircumstanceNotifications.ProviderDetailsChanged);
            retrieved.ChangeOfCircumstanceNotifications.Should().NotHaveFlag(ChangeOfCircumstanceNotifications.ApprenticeshipDetailsChanged);
        }

        [Test]
        public async Task Multiple_changes_occur_sequentially_then_apprentice_is_notifiied_only_of_differences_between_last_viewed_and_latest()
        {
            var apprenticeship = await CreateVerifiedApprenticeship();
            await ViewApprenticeship(apprenticeship);
            await ChangeApprenticeship(new ChangeBuilder(apprenticeship).ChangedOn(DateTimeOffset.Now.AddHours(1)));

            var firstChange = await GetApprenticeship(apprenticeship);
            firstChange.ChangeOfCircumstanceNotifications.Should().HaveFlag(ChangeOfCircumstanceNotifications.EmployerDetailsChanged);
            firstChange.ChangeOfCircumstanceNotifications.Should().HaveFlag(ChangeOfCircumstanceNotifications.ProviderDetailsChanged);
            firstChange.ChangeOfCircumstanceNotifications.Should().HaveFlag(ChangeOfCircumstanceNotifications.ApprenticeshipDetailsChanged);

            await ChangeApprenticeship(new ChangeBuilder(apprenticeship).OnlyChangeEmployer().ChangedOn(DateTimeOffset.Now.AddHours(2)));
            var app3 = await GetApprenticeship(apprenticeship);

            var secondChange = await GetApprenticeship(apprenticeship);
            secondChange.ChangeOfCircumstanceNotifications.Should().HaveFlag(ChangeOfCircumstanceNotifications.EmployerDetailsChanged);
            secondChange.ChangeOfCircumstanceNotifications.Should().NotHaveFlag(ChangeOfCircumstanceNotifications.ProviderDetailsChanged);
            secondChange.ChangeOfCircumstanceNotifications.Should().NotHaveFlag(ChangeOfCircumstanceNotifications.ApprenticeshipDetailsChanged);
        }

        [Test]
        public async Task Multiple_changes_occur_sequentially_then_apprentice_is_not_told_as_they_havent_viewed()
        {
            var apprenticeship = await CreateVerifiedApprenticeship();
            await ChangeApprenticeship(new ChangeBuilder(apprenticeship).OnlyChangeProvider().ChangedOn(DateTimeOffset.Now.AddHours(1)));

            var firstChange = await GetApprenticeship(apprenticeship);
            firstChange.ChangeOfCircumstanceNotifications.Should().NotHaveFlag(ChangeOfCircumstanceNotifications.EmployerDetailsChanged);
            firstChange.ChangeOfCircumstanceNotifications.Should().NotHaveFlag(ChangeOfCircumstanceNotifications.ProviderDetailsChanged);
            firstChange.ChangeOfCircumstanceNotifications.Should().NotHaveFlag(ChangeOfCircumstanceNotifications.ApprenticeshipDetailsChanged);

            await ChangeApprenticeship(new ChangeBuilder(apprenticeship).ChangedOn(DateTimeOffset.Now.AddHours(12)));

            var secondChange = await GetApprenticeship(apprenticeship);
            secondChange.ChangeOfCircumstanceNotifications.Should().NotHaveFlag(ChangeOfCircumstanceNotifications.EmployerDetailsChanged);
            secondChange.ChangeOfCircumstanceNotifications.Should().NotHaveFlag(ChangeOfCircumstanceNotifications.ProviderDetailsChanged);
            secondChange.ChangeOfCircumstanceNotifications.Should().NotHaveFlag(ChangeOfCircumstanceNotifications.ApprenticeshipDetailsChanged);
        }

        [Test]
        public async Task Employer_section_changed_and_then_confirmed_hides_notification()
        {
            var apprenticeship = await CreateVerifiedApprenticeship();

            await ConfirmApprenticeship(apprenticeship, new ConfirmationBuilder().ConfirmOnlyEmployer());
            await ChangeApprenticeship(new ChangeBuilder(apprenticeship).OnlyChangeEmployer());
            await ConfirmApprenticeship(apprenticeship, new ConfirmationBuilder().ConfirmOnlyEmployer());

            var retrieved = await GetApprenticeship(apprenticeship);
            retrieved.Should().BeEquivalentTo(new
            {
                ChangeOfCircumstanceNotifications = ChangeOfCircumstanceNotifications.None,
            });
        }

        [Test]
        public async Task Provider_section_changed_and_then_confirmed_hides_notification()
        {
            var apprenticeship = await CreateVerifiedApprenticeship();
            await ViewApprenticeship(apprenticeship);

            await ConfirmApprenticeship(apprenticeship, new ConfirmationBuilder().ConfirmOnlyProvider());
            await ChangeApprenticeship(new ChangeBuilder(apprenticeship).OnlyChangeProvider());

            var retrieved = await GetApprenticeship(apprenticeship);
            retrieved.ChangeOfCircumstanceNotifications.Should().NotHaveFlag(ChangeOfCircumstanceNotifications.EmployerDetailsChanged);
            retrieved.ChangeOfCircumstanceNotifications.Should().HaveFlag(ChangeOfCircumstanceNotifications.ProviderDetailsChanged);
            retrieved.ChangeOfCircumstanceNotifications.Should().NotHaveFlag(ChangeOfCircumstanceNotifications.ApprenticeshipDetailsChanged);
        }

        [Test]
        public async Task Provider_section_confirmed_and_then_changed_shows_notification()
        {
            var apprenticeship = await CreateVerifiedApprenticeship();

            await ConfirmApprenticeship(apprenticeship, new ConfirmationBuilder().ConfirmOnlyProvider());
            await ChangeApprenticeship(new ChangeBuilder(apprenticeship).OnlyChangeProvider());
            await ConfirmApprenticeship(apprenticeship, new ConfirmationBuilder().ConfirmOnlyProvider());

            var retrieved = await GetApprenticeship(apprenticeship);
            retrieved.Should().BeEquivalentTo(new
            {
                ChangeOfCircumstanceNotifications = ChangeOfCircumstanceNotifications.None,
            });
        }

        [Test]
        public async Task Confirmed_and_then_changed_and_also_reconfirmed_does_not_show_notification()
        {
            var apprenticeship = await CreateVerifiedApprenticeship();

            await ConfirmApprenticeship(apprenticeship, new ConfirmationBuilder());
            await ChangeApprenticeship(new ChangeBuilder(apprenticeship));
            await ConfirmApprenticeship(apprenticeship, new ConfirmationBuilder());

            var retrieved = await GetApprenticeship(apprenticeship);
            retrieved.Should().BeEquivalentTo(new
            {
                ChangeOfCircumstanceNotifications = ChangeOfCircumstanceNotifications.None,
            });
        }

        [Test]
        public async Task Negatively_confirmed_and_then_changed_and_also_reconfirmed_does_not_show_notification()
        {
            var apprenticeship = await CreateVerifiedApprenticeship();

            await ConfirmApprenticeship(apprenticeship, new ConfirmationBuilder().AsIncomplete());
            await ChangeApprenticeship(new ChangeBuilder(apprenticeship));
            await ConfirmApprenticeship(apprenticeship, new ConfirmationBuilder());

            var retrieved = await GetApprenticeship(apprenticeship);
            retrieved.Should().BeEquivalentTo(new
            {
                ChangeOfCircumstanceNotifications = ChangeOfCircumstanceNotifications.None,
            });
        }

        [Test]
        public async Task Negatively_confirmed_and_then_changed_and_negatively_confirmed_again_does_not_show_notification()
        {
            var apprenticeship = await CreateVerifiedApprenticeship();

            await ConfirmApprenticeship(apprenticeship, new ConfirmationBuilder().AsIncomplete());
            await ChangeApprenticeship(new ChangeBuilder(apprenticeship));
            await ConfirmApprenticeship(apprenticeship, new ConfirmationBuilder().AsIncomplete());

            var retrieved = await GetApprenticeship(apprenticeship);
            retrieved.Should().BeEquivalentTo(new
            {
                ChangeOfCircumstanceNotifications = ChangeOfCircumstanceNotifications.None,
            });
        }
    }
}
