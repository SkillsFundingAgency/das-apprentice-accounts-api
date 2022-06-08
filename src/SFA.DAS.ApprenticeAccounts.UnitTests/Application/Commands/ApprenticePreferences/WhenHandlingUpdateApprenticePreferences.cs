using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateAllApprenticePreferencesCommand;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateApprenticePreferenceCommand;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Application.Commands.ApprenticePreferences
{
    public class WhenHandlingUpdateApprenticePreferences
    {
        private Mock<IApprenticePreferencesContext> _mockApprenticePreferencesContext;
        private Mock<IApprenticeContext> _mockApprenticeContext;
        private Mock<IPreferencesContext> _mockPreferencesContext;
        private UpdateAllApprenticePreferencesCommand _mockCommand;

        [SetUp]
        public void SetUp()
        {
            _mockApprenticePreferencesContext = new Mock<IApprenticePreferencesContext>();
            _mockApprenticeContext = new Mock<IApprenticeContext>();
            _mockPreferencesContext = new Mock<IPreferencesContext>();
            _mockCommand = new UpdateAllApprenticePreferencesCommand
            {
                ApprenticePreferences = new List<UpdateApprenticePreferenceCommand>()
            };
        }

        [Test]
        [MoqAutoData]
        public async Task And_ApprenticeIsNull_ReturnInvalidOperationException(
            int mockPreferenceId,
            string mockPreferenceMeaning)
        {
            foreach (var apprenticePreference in _mockCommand.ApprenticePreferences)
            {
                _mockApprenticeContext.Setup(a => a.Entities.FindAsync(apprenticePreference.ApprenticeId))
                    .ReturnsAsync((Apprentice)null);
                _mockPreferencesContext.Setup(p => p.Entities.FindAsync(apprenticePreference.PreferenceId))
                    .ReturnsAsync(new Preference(mockPreferenceId, mockPreferenceMeaning));

                var handler = new UpdateApprenticePreferencesCommandHandler(_mockApprenticePreferencesContext.Object,
                    _mockApprenticeContext.Object, _mockPreferencesContext.Object);
                Func<Task> result = async () => await handler.Handle(_mockCommand, CancellationToken.None);

                await result.Should().ThrowAsync<InvalidOperationException>();
            }
        }

        [Test]
        [MoqAutoData]
        public async Task AndPreferencesIsNull_ReturnInvalidOperationException(
            Apprentice apprentice)
        {
            foreach (var apprenticePreference in _mockCommand.ApprenticePreferences)
            {
                _mockApprenticeContext.Setup(a => a.Entities.FindAsync(apprenticePreference.ApprenticeId))
                    .ReturnsAsync(apprentice);
                _mockPreferencesContext.Setup(p => p.Entities.FindAsync(apprenticePreference.PreferenceId))
                    .ReturnsAsync((Preference)null);

                var handler = new UpdateApprenticePreferencesCommandHandler(_mockApprenticePreferencesContext.Object,
                    _mockApprenticeContext.Object, _mockPreferencesContext.Object);
                Func<Task> result = async () => await handler.Handle(_mockCommand, CancellationToken.None);

                await result.Should().ThrowAsync<InvalidOperationException>();
            }
        }

        [Test]
        public async Task And_BothApprenticeAndPreferencesAreNull_ReturnInvalidOperationException()
        {
            foreach (var apprenticePreference in _mockCommand.ApprenticePreferences)
            {
                _mockApprenticeContext.Setup(a => a.Entities.FindAsync(apprenticePreference.ApprenticeId))
                    .ReturnsAsync((Apprentice)null);
                _mockPreferencesContext.Setup(p => p.Entities.FindAsync(apprenticePreference.PreferenceId))
                    .ReturnsAsync((Preference)null);

                var handler = new UpdateApprenticePreferencesCommandHandler(_mockApprenticePreferencesContext.Object,
                    _mockApprenticeContext.Object, _mockPreferencesContext.Object);
                Func<Task> result = async () => await handler.Handle(_mockCommand, CancellationToken.None);

                await result.Should().ThrowAsync<InvalidOperationException>();
            }
        }

        [Test]
        [RecursiveMoqAutoData]
        public async Task AndRecordIsNull_ThenCreateApprenticePreferencesRecord(
            DateTime mockDateOfBirth,
            MailAddress mockApprenticeMailAddress,
            string mockPreferenceMeaning,
            string mockFirstName,
            string mockLastName)
        {
            foreach (var apprenticePreference in _mockCommand.ApprenticePreferences)
            {
                var apprentice = new Apprentice(apprenticePreference.ApprenticeId, mockFirstName, mockLastName,
                    mockApprenticeMailAddress, mockDateOfBirth);
                var preference = new Preference(apprenticePreference.PreferenceId, mockPreferenceMeaning);

                _mockApprenticeContext.Setup(a => a.Entities.FindAsync(apprenticePreference.ApprenticeId))
                    .ReturnsAsync(apprentice);
                _mockPreferencesContext.Setup(p => p.Entities.FindAsync(apprenticePreference.PreferenceId))
                    .ReturnsAsync(preference);
                _mockApprenticePreferencesContext
                    .Setup(ap => ap.GetApprenticePreferenceForApprenticeAndPreference(apprenticePreference.ApprenticeId,
                        apprenticePreference.PreferenceId))
                    .ReturnsAsync((Data.Models.ApprenticePreferences)null);

                var handler = new UpdateApprenticePreferencesCommandHandler(_mockApprenticePreferencesContext.Object,
                    _mockApprenticeContext.Object, _mockPreferencesContext.Object);
                var result = await handler.Handle(_mockCommand, CancellationToken.None);

                _mockApprenticePreferencesContext.Verify(a => a.Add(It.IsAny<Data.Models.ApprenticePreferences>()));
                result.GetType().Should().Be(typeof(Unit));
            }
        }

        [Test]
        [RecursiveMoqAutoData]
        public async Task AndRecordIsPopulated_ThenUpdateApprenticePreferencesRecord(
            DateTime mockDateOfBirth,
            DateTime mockCreatedOn,
            DateTime mockUpdatedOn,
            MailAddress mockApprenticeMailAddress,
            string mockPreferenceMeaning,
            string mockFirstName,
            string mockLastName)
        {
            foreach (var apprenticePreference in _mockCommand.ApprenticePreferences)
            {
                var apprentice = new Apprentice(apprenticePreference.ApprenticeId, mockFirstName, mockLastName,
                    mockApprenticeMailAddress, mockDateOfBirth);
                var preference = new Preference(apprenticePreference.PreferenceId, mockPreferenceMeaning);
                var response = new Data.Models.ApprenticePreferences(apprenticePreference.ApprenticeId,
                    apprenticePreference.PreferenceId,
                    apprenticePreference.Status, mockCreatedOn, mockUpdatedOn);

                _mockApprenticeContext.Setup(a => a.Entities.FindAsync(apprenticePreference.ApprenticeId))
                    .ReturnsAsync(apprentice);
                _mockPreferencesContext.Setup(p => p.Entities.FindAsync(apprenticePreference.PreferenceId))
                    .ReturnsAsync(preference);
                _mockApprenticePreferencesContext
                    .Setup(ap => ap.GetApprenticePreferenceForApprenticeAndPreference(apprenticePreference.ApprenticeId,
                        apprenticePreference.PreferenceId))
                    .ReturnsAsync(response);

                _mockApprenticePreferencesContext.Verify(a => a.Update(response));

                var handler = new UpdateApprenticePreferencesCommandHandler(_mockApprenticePreferencesContext.Object,
                    _mockApprenticeContext.Object, _mockPreferencesContext.Object);
                var result = await handler.Handle(_mockCommand, CancellationToken.None);

                result.GetType().Should().Be(typeof(Unit));
            }
        }
    }
}