using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.ApprenticeCommitments.Application.Commands.CreateRegistrationCommand;
using SFA.DAS.ApprenticeCommitments.Messages.Events;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.WorkflowTests
{
    public class Step1RegisterApproval : ApiFixture
    {
        [Test]
        public async Task Validates_command()
        {
            var create = fixture.Build<CreateRegistrationCommand>()
                .Without(p => p.CommitmentsApprenticeshipId).
                Create();
            var response = await PostCreateRegistrationCommand(create);
            response
                .Should().Be400BadRequest()
                .And.MatchInContent("*CommitmentsApprenticeshipId*");
        }

        [Test, AutoData]
        public void Validates_email(CreateRegistrationCommandValidator sut, CreateRegistrationCommand data)
        {
            data.Email = default;
            var result = sut.TestValidate(data);
            result.ShouldHaveValidationErrorFor(p => p.Email);
        }

        [Test]
        public async Task Cannot_retrieve_missing_registration()
        {
            var response = await client.GetAsync($"registrations/{Guid.NewGuid()}");
            response.Should().Be404NotFound();
        }

        [Test]
        public async Task Can_retrieve_registration()
        {
            var create = await CreateRegistration();
            var registration = await GetRegistration(create.RegistrationId);
            registration.Should().BeEquivalentTo(new
            {
                create.RegistrationId,
                create.DateOfBirth,
                create.Email,
                HasViewedVerification = false,
                HasCompletedVerification = false,
            });
        }

        [Test]
        public async Task Triggers_ApprenticeshipRegisteredEvent()
        {
            var approval = await CreateRegistration();

            Messages.PublishedMessages.Should().ContainEquivalentOf(new
            {
                Message = new ApprenticeshipRegisteredEvent
                {
                    RegistrationId = approval.RegistrationId,
                }
            });
        }
    }
}