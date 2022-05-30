using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateApprenticePreferencesCommand;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Application.Commands.ApprenticePreferences
{
    public class WhenHandlingUpdateApprenticePreferences
    { 
        Mock<IApprenticePreferencesContext> _mockApprenticePreferencesContext; 
        Mock<IApprenticeContext> _mockApprenticeContext; 
        Mock<IPreferencesContext> _mockPreferencesContext;
        UpdateApprenticePreferenceCommand _mockCommand;

        [SetUp]
        public void SetUp()
        {
            _mockApprenticePreferencesContext = new Mock<IApprenticePreferencesContext>();
            _mockApprenticeContext = new Mock<IApprenticeContext>();
            _mockPreferencesContext = new Mock<IPreferencesContext>();
            _mockCommand = new UpdateApprenticePreferenceCommand();
        }

        [Test, MoqAutoData]
        public async Task And_ApprenticeIsNull_ReturnInvalidOperationException(
            int preferenceId,
            string preferenceMeaning)
        {
            _mockApprenticeContext.Setup(a => a.Entities.Find(_mockCommand.ApprenticeId)).Returns((Apprentice)null);
            _mockPreferencesContext.Setup(a => a.Entities.Find(_mockCommand.PreferenceId)).Returns(new Preference(preferenceId, preferenceMeaning));

            var handler = new UpdateApprenticePreferenceCommandHandler(_mockApprenticePreferencesContext.Object, _mockApprenticeContext.Object, _mockPreferencesContext.Object);
            Func<Task> result = async () => await handler.Handle(_mockCommand, CancellationToken.None);

            await result.Should().ThrowAsync<InvalidOperationException>();
        }

        [Test, MoqAutoData]
        public async Task AndPreferencesIsNull_ReturnInvalidOperationException(
            Apprentice apprentice)
        {
            _mockApprenticeContext.Setup(a => a.Entities.Find(_mockCommand.ApprenticeId)).Returns(apprentice);
            _mockPreferencesContext.Setup(a => a.Entities.Find(_mockCommand.PreferenceId)).Returns((Preference)null);

            var handler = new UpdateApprenticePreferenceCommandHandler(_mockApprenticePreferencesContext.Object, _mockApprenticeContext.Object, _mockPreferencesContext.Object);
            Func<Task> result = async () => await handler.Handle(_mockCommand, CancellationToken.None);

            await result.Should().ThrowAsync<InvalidOperationException>();
        }

        [Test]
        public async Task And_BothApprenticeAndPreferencesAreNull_ReturnInvalidOperationException()
        {
            _mockApprenticeContext.Setup(a => a.Entities.Find(_mockCommand.ApprenticeId)).Returns((Apprentice)null);
            _mockPreferencesContext.Setup(a => a.Entities.Find(_mockCommand.PreferenceId)).Returns((Preference)null);

            var handler = new UpdateApprenticePreferenceCommandHandler(_mockApprenticePreferencesContext.Object, _mockApprenticeContext.Object, _mockPreferencesContext.Object);
            Func<Task> result = async () => await handler.Handle(_mockCommand, CancellationToken.None);

            await result.Should().ThrowAsync<InvalidOperationException>();
        }

        [Test, RecursiveMoqAutoData]
        public async Task AndRecordIsNull_ThenCreateApprenticePreferencesRecord(
            DateTime mockDateTime,
            MailAddress apprenticeAddress,
            string preferenceMeaning,
            string firstName,
            string lastName)
        {
            var apprentice = new Apprentice(_mockCommand.ApprenticeId, firstName, lastName, apprenticeAddress, mockDateTime);
            var preference = new Preference(_mockCommand.PreferenceId, preferenceMeaning);

            _mockApprenticeContext.Setup(a => a.Entities.Find(_mockCommand.ApprenticeId)).Returns(apprentice);
            _mockPreferencesContext.Setup(a => a.Entities.Find(_mockCommand.PreferenceId)).Returns(preference);
            _mockApprenticePreferencesContext
                .Setup(a => a.GetSinglePreferenceValueAsync(_mockCommand.ApprenticeId, _mockCommand.PreferenceId))
                .ReturnsAsync((Data.Models.ApprenticePreferences)null);

            var handler = new UpdateApprenticePreferenceCommandHandler(_mockApprenticePreferencesContext.Object,
                _mockApprenticeContext.Object, _mockPreferencesContext.Object);
            var result = await handler.Handle(_mockCommand, CancellationToken.None);

            _mockApprenticePreferencesContext.Verify(a => a.Add(It.IsAny<Data.Models.ApprenticePreferences>()));
            result.GetType().Should().Be(typeof(Unit));
        }

        [Test, RecursiveMoqAutoData]
        public async Task AndRecordIsPopulated_ThenUpdateApprenticePreferencesRecord(
            DateTime mockDateTime,
            DateTime mockDateTime2,
            MailAddress apprenticeAddress,
            string preferenceMeaning,
            string firstName,
            string lastName)
        {
            var apprentice = new Apprentice(_mockCommand.ApprenticeId, firstName, lastName, apprenticeAddress, mockDateTime);
            var preference = new Preference(_mockCommand.PreferenceId, preferenceMeaning);
            var response = new Data.Models.ApprenticePreferences(_mockCommand.ApprenticeId, _mockCommand.PreferenceId,
                _mockCommand.Status, mockDateTime, mockDateTime2);

            _mockApprenticeContext.Setup(a => a.Entities.Find(_mockCommand.ApprenticeId)).Returns(apprentice);
            _mockPreferencesContext.Setup(a => a.Entities.Find(_mockCommand.PreferenceId)).Returns(preference);
            _mockApprenticePreferencesContext
                .Setup(a => a.GetSinglePreferenceValueAsync(_mockCommand.ApprenticeId, _mockCommand.PreferenceId))
                .ReturnsAsync(response);

            var handler = new UpdateApprenticePreferenceCommandHandler(_mockApprenticePreferencesContext.Object,
                _mockApprenticeContext.Object, _mockPreferencesContext.Object);
            var result = await handler.Handle(_mockCommand, CancellationToken.None);

            _mockApprenticePreferencesContext.Verify(a => a.Update(response));
            result.GetType().Should().Be(typeof(Unit));
        }
    }
}
