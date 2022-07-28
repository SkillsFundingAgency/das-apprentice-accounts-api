﻿using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateApprenticePreferenceCommand;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.Exceptions;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Application.Commands.ApprenticePreferences
{
    //all unit tests failing here
    public class WhenHandlingUpdateApprenticePreference
    {
        private Mock<IApprenticePreferencesContext> _mockApprenticePreferencesContext;
        private Mock<IApprenticeContext> _mockApprenticeContext;
        private Mock<IPreferencesContext> _mockPreferencesContext;
        private Mock<ApprenticeAccountsDbContext> _dbContext;
        private UpdateApprenticePreferenceCommand _mockCommand;
        private Mock<ILogger<UpdateApprenticePreferenceCommandHandler>> _logger;

        [SetUp]
        public void SetUp()
        {
            _mockApprenticePreferencesContext = new Mock<IApprenticePreferencesContext>();
            _mockApprenticeContext = new Mock<IApprenticeContext>();
            _mockPreferencesContext = new Mock<IPreferencesContext>();
            _dbContext = new Mock<ApprenticeAccountsDbContext>();
            _mockCommand = new UpdateApprenticePreferenceCommand()
            {
                ApprenticeId = new Guid(),
                PreferenceId = new int(),
                Status = new bool()

            };
            _logger = new Mock<ILogger<UpdateApprenticePreferenceCommandHandler>>();
        }


        [Test]
        [MoqAutoData]
        public async Task And_ApprenticeIsNull_ReturnInvalidInputException(
            int mockPreferenceId,
            string mockPreferenceMeaning)
        {
            _mockApprenticeContext.Setup(a => a.Entities.FindAsync(_mockCommand.ApprenticeId)).ReturnsAsync((Apprentice)null);
            _mockPreferencesContext.Setup(p => p.Entities.FindAsync(_mockCommand.PreferenceId))
                .ReturnsAsync(new Preference(mockPreferenceId, mockPreferenceMeaning));

            var handler = new UpdateApprenticePreferenceCommandHandler(_mockApprenticePreferencesContext.Object,
                _mockApprenticeContext.Object, _mockPreferencesContext.Object, _dbContext.Object ,_logger.Object);
            Func<Task> result = async () => await handler.Handle(_mockCommand, CancellationToken.None);

            await result.Should().ThrowAsync<InvalidInputException>();
        }

        [Test]
        [MoqAutoData]
        public async Task AndPreferencesIsNull_ReturnInvalidInputException(
            Apprentice mockApprentice)
        {
            _mockApprenticeContext.Setup(a => a.Entities.FindAsync(_mockCommand.ApprenticeId)).ReturnsAsync(mockApprentice);
            _mockPreferencesContext.Setup(p => p.Entities.FindAsync(_mockCommand.PreferenceId)).ReturnsAsync((Preference)null);

            var handler = new UpdateApprenticePreferenceCommandHandler(_mockApprenticePreferencesContext.Object,
                _mockApprenticeContext.Object, _mockPreferencesContext.Object, _dbContext.Object, _logger.Object);
            Func<Task> result = async () => await handler.Handle(_mockCommand, CancellationToken.None);

            await result.Should().ThrowAsync<InvalidInputException>();
        }

        [Test]
        public async Task And_BothApprenticeAndPreferencesAreNull_ReturnInvalidInputException()
        {
            _mockApprenticeContext.Setup(a => a.Entities.FindAsync(_mockCommand.ApprenticeId)).ReturnsAsync((Apprentice)null);
            _mockPreferencesContext.Setup(p => p.Entities.FindAsync(_mockCommand.PreferenceId)).ReturnsAsync((Preference)null);

            var handler = new UpdateApprenticePreferenceCommandHandler(_mockApprenticePreferencesContext.Object,
                _mockApprenticeContext.Object, _mockPreferencesContext.Object, _dbContext.Object, _logger.Object);
            Func<Task> result = async () => await handler.Handle(_mockCommand, CancellationToken.None);

            await result.Should().ThrowAsync<InvalidInputException>();
        }

        [Test]
        [RecursiveMoqAutoData]
        public async Task AndRecordIsNull_ThenCreateApprenticePreferencesRecord(
            DateTime mockDateTime,
            MailAddress mockApprenticeMailAddress,
            string mockPreferenceMeaning,
            string mockFirstName,
            string mockLastName)
        {
            var apprentice = new Apprentice(_mockCommand.ApprenticeId, mockFirstName, mockLastName,
                mockApprenticeMailAddress, mockDateTime);
            var preference = new Preference(_mockCommand.PreferenceId, mockPreferenceMeaning);

            _mockApprenticeContext.Setup(a => a.Entities.FindAsync(_mockCommand.ApprenticeId)).ReturnsAsync(apprentice);
            _mockPreferencesContext.Setup(p => p.Entities.FindAsync(_mockCommand.PreferenceId)).ReturnsAsync(preference);
            _mockApprenticePreferencesContext
                .Setup(ap => ap.GetApprenticePreferenceForApprenticeAndPreference(_mockCommand.ApprenticeId,
                    _mockCommand.PreferenceId))
                .ReturnsAsync((Data.Models.ApprenticePreferences)null);

            var handler = new UpdateApprenticePreferenceCommandHandler(_mockApprenticePreferencesContext.Object,
                _mockApprenticeContext.Object, _mockPreferencesContext.Object, _dbContext.Object, _logger.Object);
            var result = await handler.Handle(_mockCommand, CancellationToken.None);

            _mockApprenticePreferencesContext.Verify(a => a.AddAsync(It.IsAny<Data.Models.ApprenticePreferences>(), CancellationToken.None));
            result.GetType().Should().Be(typeof(Unit));
        }

        [Test]
        [RecursiveMoqAutoData]
        public async Task AndRecordIsPopulated_ThenUpdateApprenticePreferencesRecord(
            DateTime mockCreatedOn,
            DateTime mockUpdatedOn,
            DateTime mockDateOfBirth,
            MailAddress mockApprenticeMailAddress,
            string mockPreferenceMeaning,
            string mockFirstName,
            string mockLastName)
        {
            var apprentice = new Apprentice(_mockCommand.ApprenticeId, mockFirstName, mockLastName,
                mockApprenticeMailAddress, mockDateOfBirth);
            var preference = new Preference(_mockCommand.PreferenceId, mockPreferenceMeaning);
            var response = new Data.Models.ApprenticePreferences(_mockCommand.ApprenticeId, _mockCommand.PreferenceId,
                _mockCommand.Status, mockCreatedOn, mockUpdatedOn);

            _mockApprenticeContext.Setup(a => a.Entities.FindAsync(_mockCommand.ApprenticeId)).ReturnsAsync(apprentice);
            _mockPreferencesContext.Setup(p => p.Entities.FindAsync(_mockCommand.PreferenceId)).ReturnsAsync(preference);
            _mockApprenticePreferencesContext
                .Setup(ap => ap.GetApprenticePreferenceForApprenticeAndPreference(_mockCommand.ApprenticeId,
                    _mockCommand.PreferenceId))
                .ReturnsAsync(response);

            var handler = new UpdateApprenticePreferenceCommandHandler(_mockApprenticePreferencesContext.Object,
                _mockApprenticeContext.Object, _mockPreferencesContext.Object, _dbContext.Object, _logger.Object);
            var result = await handler.Handle(_mockCommand, CancellationToken.None);

            _dbContext.Verify(a => a.SaveChangesAsync(CancellationToken.None));
            result.GetType().Should().Be(typeof(Unit));
        }
    }
}