using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateAllApprenticePreferencesCommand;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.Apprentice;
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
        private Mock<ILogger<UpdateAllApprenticePreferencesCommandHandler>> _logger;

        [SetUp]
        public void SetUp()
        {
            _mockApprenticePreferencesContext = new Mock<IApprenticePreferencesContext>();
            _mockApprenticeContext = new Mock<IApprenticeContext>();
            _mockPreferencesContext = new Mock<IPreferencesContext>();
            _mockCommand = new UpdateAllApprenticePreferencesCommand
            {
                ApprenticePreferences = new List<ApprenticeAccounts.Application.Commands.UpdateAllApprenticePreferencesCommand.UpdateApprenticePreferenceCommand>()
            };
            _logger = new Mock<ILogger<UpdateAllApprenticePreferencesCommandHandler>>();
        }

        [Test]
        [MoqAutoData]
        public async Task And_ApprenticeIsNull_ReturnInvalidInputException(
            int mockPreferenceId,
            string mockPreferenceMeaning,
            string mockPreferenceHint,
            List<UpdateApprenticePreferenceCommand> apprenticePreferences)
        {
            _mockCommand.ApprenticePreferences = apprenticePreferences;

            foreach (var apprenticePreference in _mockCommand.ApprenticePreferences)
            {
                _mockApprenticeContext.Setup(a => a.Entities.FindAsync(_mockCommand.ApprenticeId))
                    .ReturnsAsync((Apprentice)null);
                _mockPreferencesContext.Setup(p => p.Entities.FindAsync(apprenticePreference.PreferenceId))
                    .ReturnsAsync(new Preference(mockPreferenceId, mockPreferenceMeaning, mockPreferenceHint));

                var handler = new UpdateAllApprenticePreferencesCommandHandler(_mockApprenticePreferencesContext.Object,
                    _mockApprenticeContext.Object, _mockPreferencesContext.Object, _logger.Object);
                Func<Task> result = async () => await handler.Handle(_mockCommand, CancellationToken.None);

                await result.Should().ThrowAsync<Exception>().WithMessage("No Apprentice record found.");
            }
        }

        [Test]
        [MoqAutoData]
        public async Task AndPreferencesIsNull_ReturnInvalidInputException(
            Apprentice apprentice,
            List<ApprenticeAccounts.Application.Commands.UpdateAllApprenticePreferencesCommand.UpdateApprenticePreferenceCommand> apprenticePreferences)
        {
            _mockCommand.ApprenticePreferences = apprenticePreferences;

            foreach (var apprenticePreference in _mockCommand.ApprenticePreferences)
            {
                _mockApprenticeContext.Setup(a => a.Entities.FindAsync(_mockCommand.ApprenticeId))
                    .ReturnsAsync(apprentice);
                _mockPreferencesContext.Setup(p => p.Entities.FindAsync(apprenticePreference.PreferenceId))
                    .ReturnsAsync((Preference)null);

                var handler = new UpdateAllApprenticePreferencesCommandHandler(_mockApprenticePreferencesContext.Object,
                    _mockApprenticeContext.Object, _mockPreferencesContext.Object, _logger.Object);
                Func<Task> result = async () => await handler.Handle(_mockCommand, CancellationToken.None);

                await result.Should().ThrowAsync<Exception>().WithMessage("No Preference record found.");
            }
        }


        [Test]
        [RecursiveMoqAutoData]
        public async Task AndRecordIsNull_ThenCreateApprenticePreferencesRecord(
            DateTime mockDateOfBirth,
            MailAddress mockApprenticeMailAddress,
            string mockPreferenceMeaning,
            string mockPreferenceHint,
            string mockFirstName,
            string mockLastName,
            ApprenticeAccounts.Application.Commands.UpdateAllApprenticePreferencesCommand.UpdateApprenticePreferenceCommand apprenticePreference)
        {
            _mockCommand.ApprenticePreferences = new List<ApprenticeAccounts.Application.Commands.UpdateAllApprenticePreferencesCommand.UpdateApprenticePreferenceCommand>(){apprenticePreference};
            foreach (var item in _mockCommand.ApprenticePreferences)
            {
                var apprentice = new Apprentice(_mockCommand.ApprenticeId, mockFirstName, mockLastName,
                    mockApprenticeMailAddress, mockDateOfBirth);
                var preference = new Preference(apprenticePreference.PreferenceId, mockPreferenceMeaning, mockPreferenceHint);

                _mockApprenticeContext.Setup(a => a.Entities.FindAsync(_mockCommand.ApprenticeId))
                    .ReturnsAsync(apprentice);
                _mockPreferencesContext.Setup(p => p.Entities.FindAsync(apprenticePreference.PreferenceId))
                    .ReturnsAsync(preference);
                _mockApprenticePreferencesContext
                    .Setup(ap => ap.GetApprenticePreferenceForApprenticeAndPreference(_mockCommand.ApprenticeId,
                        apprenticePreference.PreferenceId))
                    .ReturnsAsync((Data.Models.ApprenticePreferences)null);

                var handler = new UpdateAllApprenticePreferencesCommandHandler(_mockApprenticePreferencesContext.Object,
                    _mockApprenticeContext.Object, _mockPreferencesContext.Object, _logger.Object);
                var result = await handler.Handle(_mockCommand, CancellationToken.None);

                _mockApprenticePreferencesContext.Verify(a => a.AddAsync(It.IsAny<Data.Models.ApprenticePreferences>(), CancellationToken.None));
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
            string mockPreferenceHint,
            string mockFirstName,
            string mockLastName,
            ApprenticeAccounts.Application.Commands.UpdateAllApprenticePreferencesCommand.UpdateApprenticePreferenceCommand apprenticePreferences)
        {
            _mockCommand.ApprenticePreferences = new List<ApprenticeAccounts.Application.Commands.UpdateAllApprenticePreferencesCommand.UpdateApprenticePreferenceCommand>(){ apprenticePreferences };
            foreach (var item in _mockCommand.ApprenticePreferences)
            {
                var apprentice = new Apprentice(_mockCommand.ApprenticeId, mockFirstName, mockLastName,
                    mockApprenticeMailAddress, mockDateOfBirth);
                var preference = new Preference(item.PreferenceId, mockPreferenceMeaning, mockPreferenceHint);
                var response = new Data.Models.ApprenticePreferences(_mockCommand.ApprenticeId,
                    item.PreferenceId,
                    item.Status, mockCreatedOn, mockUpdatedOn);

                _mockApprenticeContext.Setup(a => a.Entities.FindAsync(_mockCommand.ApprenticeId))
                    .ReturnsAsync(apprentice);
                _mockPreferencesContext.Setup(p => p.Entities.FindAsync(item.PreferenceId))
                    .ReturnsAsync(preference);
                _mockApprenticePreferencesContext
                    .Setup(ap => ap.GetApprenticePreferenceForApprenticeAndPreference(_mockCommand.ApprenticeId,
                        item.PreferenceId))
                    .ReturnsAsync(response);

                var handler = new UpdateAllApprenticePreferencesCommandHandler(_mockApprenticePreferencesContext.Object,
                    _mockApprenticeContext.Object, _mockPreferencesContext.Object, _logger.Object);
                var result = await handler.Handle(_mockCommand, CancellationToken.None);

                result.GetType().Should().Be(typeof(Unit));
            }
        }
    }
}