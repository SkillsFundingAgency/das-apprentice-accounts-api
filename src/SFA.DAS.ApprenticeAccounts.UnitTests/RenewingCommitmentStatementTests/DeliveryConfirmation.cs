using System;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeCommitments.Data.Models;

namespace SFA.DAS.ApprenticeCommitments.UnitTests.RenewingRevisionTests
{
    public class DeliveryConfirmation
    {
        Fixture _f = new Fixture();
        private Revision _existingRevision;
        private Apprenticeship _apprenticeship;
        private long _commitmentsApprenticeshipId;

        [SetUp]
        public void Arrange()
        {
            _commitmentsApprenticeshipId = _f.Create<long>();
            _existingRevision = _f.Create<Revision>();
            _existingRevision.SetProperty(p => p.CommitmentsApprenticeshipId, _commitmentsApprenticeshipId);
            _apprenticeship = new Apprenticeship(_existingRevision);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void When_delivery_section_confirmation_status_is_not_set_Then_delivery_section_remains_not_set_regardless_of_data_changes(bool withSameData)
        {
            _existingRevision.SetProperty(p => p.HowApprenticeshipDeliveredCorrect, null);

            var details = withSameData ? _existingRevision.Details.Clone() : _f.Create<ApprenticeshipDetails>();

            _apprenticeship.Revise(_commitmentsApprenticeshipId, details, DateTime.Now);

            _apprenticeship.Revisions.Last().HowApprenticeshipDeliveredCorrect.Should().BeNull();
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void When_delivery_section_confirmation_status_is_set_Then_delivery_section_does_not_change_status_regardless_of_data_changes(bool confirmationStatus, bool withSameData)
        {
            _existingRevision.SetProperty(p => p.HowApprenticeshipDeliveredCorrect, confirmationStatus);
            var details = withSameData ? _existingRevision.Details.Clone() : _f.Create<ApprenticeshipDetails>();

            _apprenticeship.Revise(_commitmentsApprenticeshipId, details, DateTime.Now);
            
            _apprenticeship.Revisions.Last().HowApprenticeshipDeliveredCorrect.Should().Be(confirmationStatus);
        }
    }
}