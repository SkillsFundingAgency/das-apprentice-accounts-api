using System;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeCommitments.Data.Models;

namespace SFA.DAS.ApprenticeCommitments.UnitTests.RenewingRevisionTests
{
    public class EmployerConfirmation
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
        public void When_employer_section_confirmation_status_is_not_set_Then_employer_section_remains_not_set_regardless_of_data_changes(bool withSameData)
        {
            var details = withSameData ? _existingRevision.Details.Clone() : _f.Create<ApprenticeshipDetails>();

            _apprenticeship.Revise(_commitmentsApprenticeshipId, details, DateTime.Now);

            _apprenticeship.Revisions.Last().EmployerCorrect.Should().BeNull();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void When_employer_section_confirmation_status_is_set_And_no_change_to_employer_has_occurred_Then_employer_section_does_not_change_status(bool confirmationStatus)
        {
            _existingRevision.Confirm(new Confirmations { EmployerCorrect = confirmationStatus }, DateTime.UtcNow);

            _apprenticeship.Revise(_commitmentsApprenticeshipId, _existingRevision.Details.Clone(), DateTime.Now);

            _apprenticeship.Revisions.Last().EmployerCorrect.Should().Be(confirmationStatus);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void When_employer_section_confirmation_status_is_set_And_a_change_to_employer_name_has_occurred_Then_employer_section_is_not_confirmed(bool confirmationStatus)
        {
            _existingRevision.Confirm(new Confirmations { EmployerCorrect = confirmationStatus }, DateTime.UtcNow);
            var newDetails = _f.Create<ApprenticeshipDetails>();
            newDetails.SetProperty(p=>p.EmployerAccountLegalEntityId, _existingRevision.Details.EmployerAccountLegalEntityId);

            _apprenticeship.Revise(_commitmentsApprenticeshipId, newDetails, DateTime.Now);

            _apprenticeship.Revisions.Last().EmployerCorrect.Should().BeNull();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void When_employer_section_confirmation_is_set_And_a_change_to_employer_id_has_occurred_Then_employer_section_is_not_confirmed(bool confirmationStatus)
        {
            _existingRevision.Confirm(new Confirmations { EmployerCorrect = confirmationStatus }, DateTime.UtcNow);
            var newDetails = _f.Create<ApprenticeshipDetails>();
            newDetails.SetProperty(p => p.EmployerName, _existingRevision.Details.EmployerName);

            _apprenticeship.Revise(_commitmentsApprenticeshipId, newDetails, DateTime.Now);
            _apprenticeship.Revisions.Last().EmployerCorrect.Should().BeNull();
        }
    }
}