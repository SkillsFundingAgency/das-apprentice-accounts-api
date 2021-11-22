using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.WorkflowTests
{
    internal class SubsequentApprenticeship : ApiFixture
    {
        [Test]
        public async Task Updates_personal_details_in_registration()
        {
            // Given
            var firstApproval = await CreateRegistration();
            var firstApprenticeship = await VerifyRegistration(firstApproval);

            var subsequentApproval = await CreateRegistration(
                lastName: firstApproval.LastName, dateOfBirth: firstApproval.DateOfBirth);
            await VerifyRegistration(subsequentApproval.RegistrationId, firstApprenticeship.ApprenticeId);

            // When
            var apprenticeships = await GetApprenticeships(firstApprenticeship.ApprenticeId);

            // Then
            apprenticeships.Should().HaveCount(2).And.BeInDescendingOrder(x => x.ApprovedOn);
        }
    }
}
