using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.WorkflowTests
{
    public class Step4Confirmations : ApiFixture
    {
        [TestCase("EmployerCorrect", true)]
        [TestCase("EmployerCorrect", false)]
        [TestCase("EmployerCorrect", null)]
        [TestCase("TrainingProviderCorrect", true)]
        [TestCase("TrainingProviderCorrect", false)]
        [TestCase("TrainingProviderCorrect", null)]
        [TestCase("ApprenticeshipDetailsCorrect", true)]
        [TestCase("ApprenticeshipDetailsCorrect", false)]
        [TestCase("ApprenticeshipDetailsCorrect", null)]
        [TestCase("HowApprenticeshipDeliveredCorrect", true)]
        [TestCase("HowApprenticeshipDeliveredCorrect", false)]
        [TestCase("HowApprenticeshipDeliveredCorrect", null)]
        [TestCase("RolesAndResponsibilitiesCorrect", true)]
        [TestCase("RolesAndResponsibilitiesCorrect", false)]
        [TestCase("RolesAndResponsibilitiesCorrect", null)]
        public async Task Can_confirm_new_api(string confirmation, bool? value)
        {
            var apprenticeship = await CreateVerifiedApprenticeship();

            object data = confirmation switch
            {
                "EmployerCorrect" => new { EmployerCorrect = value },
                "TrainingProviderCorrect" => new { TrainingProviderCorrect = value },
                "ApprenticeshipDetailsCorrect" => new { ApprenticeshipDetailsCorrect = value },
                "HowApprenticeshipDeliveredCorrect" => new { HowApprenticeshipDeliveredCorrect = value },
                "RolesAndResponsibilitiesCorrect" => new { RolesAndResponsibilitiesCorrect = value },
                _ => throw new ArgumentOutOfRangeException(nameof(confirmation)),
            };

            var r4 = await client.PatchValueAsync(
                $"apprentices/{apprenticeship.ApprenticeId}/apprenticeships/{apprenticeship.Id}/revisions/{apprenticeship.RevisionId}/confirmations",
                data);
            r4.Should().Be2XXSuccessful();

            apprenticeship = (await GetApprenticeships(apprenticeship.ApprenticeId))[0];
            apprenticeship.Should().BeEquivalentTo(data);
        }
    }
}