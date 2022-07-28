using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateAllApprenticePreferencesCommand;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateApprenticePreferenceCommand;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.Exceptions;
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
        private Mock<ApprenticeAccountsDbContext> _mockApprenticeAccountsDbContext;
        private UpdateAllApprenticePreferencesCommand _mockCommand;
        private Mock<ILogger<UpdateAllApprenticePreferencesCommandHandler>> _logger;

        [SetUp]
        public void SetUp()
        {
            _mockApprenticePreferencesContext = new Mock<IApprenticePreferencesContext>();
            _mockApprenticeContext = new Mock<IApprenticeContext>();
            _mockPreferencesContext = new Mock<IPreferencesContext>();
            _mockApprenticeAccountsDbContext = new Mock<ApprenticeAccountsDbContext>();
            _mockCommand = new UpdateAllApprenticePreferencesCommand
            {
                ApprenticePreferences = new List<UpdateApprenticePreferenceCommand>()
            };
            _logger = new Mock<ILogger<UpdateAllApprenticePreferencesCommandHandler>>();
        }

        [Test]
        [MoqAutoData]
        public async Task And_ApprenticeIsNull_ReturnInvalidInputException(
            int mockPreferenceId,
            string mockPreferenceMeaning)
        {
            foreach (var apprenticePreference in _mockCommand.ApprenticePreferences)
            {
                _mockApprenticeContext.Setup(a => a.Entities.FindAsync(apprenticePreference.ApprenticeId))
                    .ReturnsAsync((Apprentice)null);
                _mockPreferencesContext.Setup(p => p.Entities.FindAsync(apprenticePreference.PreferenceId))
                    .ReturnsAsync(new Preference(mockPreferenceId, mockPreferenceMeaning));

                var handler = new UpdateAllApprenticePreferencesCommandHandler(_mockApprenticePreferencesContext.Object,
                    _mockApprenticeContext.Object, _mockPreferencesContext.Object, _mockApprenticeAccountsDbContext.Object, _logger.Object);
                Func<Task> result = async () => await handler.Handle(_mockCommand, CancellationToken.None);

                await result.Should().ThrowAsync<InvalidInputException>();
            }
        }

        [Test]
        [MoqAutoData]
        public async Task AndPreferencesIsNull_ReturnInvalidInputException(
            Apprentice apprentice)
        {
            foreach (var apprenticePreference in _mockCommand.ApprenticePreferences)
            {
                _mockApprenticeContext.Setup(a => a.Entities.FindAsync(apprenticePreference.ApprenticeId))
                    .ReturnsAsync(apprentice);
                _mockPreferencesContext.Setup(p => p.Entities.FindAsync(apprenticePreference.PreferenceId))
                    .ReturnsAsync((Preference)null);

                var handler = new UpdateAllApprenticePreferencesCommandHandler(_mockApprenticePreferencesContext.Object,
                    _mockApprenticeContext.Object, _mockPreferencesContext.Object, _mockApprenticeAccountsDbContext.Object, _logger.Object);
                Func<Task> result = async () => await handler.Handle(_mockCommand, CancellationToken.None);

                await result.Should().ThrowAsync<InvalidInputException>();
            }
        }

        [Test]
        public async Task And_BothApprenticeAndPreferencesAreNull_ReturnInvalidInputException()
        {
            foreach (var apprenticePreference in _mockCommand.ApprenticePreferences)
            {
                _mockApprenticeContext.Setup(a => a.Entities.FindAsync(apprenticePreference.ApprenticeId))
                    .ReturnsAsync((Apprentice)null);
                _mockPreferencesContext.Setup(p => p.Entities.FindAsync(apprenticePreference.PreferenceId))
                    .ReturnsAsync((Preference)null);

                var handler = new UpdateAllApprenticePreferencesCommandHandler(_mockApprenticePreferencesContext.Object,
                    _mockApprenticeContext.Object, _mockPreferencesContext.Object, _mockApprenticeAccountsDbContext.Object, _logger.Object);
                Func<Task> result = async () => await handler.Handle(_mockCommand, CancellationToken.None);

                await result.Should().ThrowAsync<InvalidInputException>();
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

                var handler = new UpdateAllApprenticePreferencesCommandHandler(_mockApprenticePreferencesContext.Object,
                    _mockApprenticeContext.Object, _mockPreferencesContext.Object, _mockApprenticeAccountsDbContext.Object, _logger.Object);
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

                _mockApprenticeAccountsDbContext.Verify(a => a.SaveChangesAsync(CancellationToken.None));

                var handler = new UpdateAllApprenticePreferencesCommandHandler(_mockApprenticePreferencesContext.Object,
                    _mockApprenticeContext.Object, _mockPreferencesContext.Object, _mockApprenticeAccountsDbContext.Object, _logger.Object);
                var result = await handler.Handle(_mockCommand, CancellationToken.None);

                result.GetType().Should().Be(typeof(Unit));
            }
        }
    }
}