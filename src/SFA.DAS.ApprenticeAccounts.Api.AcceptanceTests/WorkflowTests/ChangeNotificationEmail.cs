using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeCommitments.Messages.Events;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.WorkflowTests
{
    public class ChangeNotificationEmail : ChangeNotificationFixture
    {
        public ChangeNotificationEmail() => TimeBetweenActions = TimeSpan.Zero;

        [Test]
        public async Task First_change_within_24_hours_of_creation_sends_ApprenticeshipChanged_event()
        {
            var apprenticeship = await CreateVerifiedApprenticeship();

            context.Time.Advance(TimeSpan.FromHours(12));
            await ChangeApprenticeship(new ChangeBuilder(apprenticeship));

            context.Messages.PublishedMessages
                .Where(x => x.Message is ApprenticeshipChangedEvent)
                .Should().HaveCount(1);
        }

        [Test]
        public async Task First_change_after_24_hours_of_creation_sends_ApprenticeshipChanged_event()
        {
            var apprenticeship = await CreateVerifiedApprenticeship();

            context.Time.Advance(TimeSpan.FromHours(25));
            await ChangeApprenticeship(new ChangeBuilder(apprenticeship));

            context.Messages.PublishedMessages
                .Where(x => x.Message is ApprenticeshipChangedEvent)
                .Should().HaveCount(1);
        }

        [Test]
        public async Task Multiple_change_with_24_hours_sends_only_one_ApprenticeshipChanged_event()
        {
            var apprenticeship = await CreateVerifiedApprenticeship();
            context.Time.Advance(TimeSpan.FromDays(2));

            await ChangeApprenticeship(new ChangeBuilder(apprenticeship));
            context.Time.Advance(TimeSpan.FromHours(23));
            await ChangeApprenticeship(new ChangeBuilder(apprenticeship));

            context.Messages.PublishedMessages
                .Where(x => x.Message is ApprenticeshipChangedEvent)
                .Should().HaveCount(2);
        }

        [Test]
        public async Task Second_change_within_24_hours_after_apprentice_has_viewed_first_sends_ApprenticeshipChanged_event()
        {
            var apprenticeship = await CreateVerifiedApprenticeship();

            context.Time.Advance(TimeSpan.FromDays(5));
            await ChangeApprenticeship(new ChangeBuilder(apprenticeship));

            context.Time.Advance(TimeSpan.FromHours(12));
            await ViewApprenticeship(apprenticeship);

            context.Time.Advance(TimeSpan.FromHours(1));
            await ChangeApprenticeship(new ChangeBuilder(apprenticeship));

            context.Messages.PublishedMessages
                .Where(x => x.Message is ApprenticeshipChangedEvent)
                .Should().HaveCount(2);
        }
    }
}