using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Commands.UpdateApprenticePreferencesCommand;
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
        Mock<IApprenticePreferencesContext> _mockApprenticePreferencesContext;
        Mock<IApprenticeContext> _mockApprenticeContext;
        Mock<IPreferencesContext> _mockPreferencesContext;
        UpdateApprenticePreferencesCommand _mockCommand;

        [SetUp]
        public void SetUp()
        {
            _mockApprenticePreferencesContext = new Mock<IApprenticePreferencesContext>();
            _mockApprenticeContext = new Mock<IApprenticeContext>();
            _mockPreferencesContext = new Mock<IPreferencesContext>();
            _mockCommand = new UpdateApprenticePreferencesCommand();
            _mockCommand.ApprenticePreferences = new List<UpdateApprenticePreferenceCommand>();
        }

        [Test, MoqAutoData]
        public async Task And_ApprenticeIsNull_ReturnInvalidOperationException(
            int preferenceId,
            string preferenceMeaning)
        {
            foreach (var item in _mockCommand.ApprenticePreferences)
            {
                _mockApprenticeContext.Setup(a => a.Entities.Find(item.ApprenticeId)).Returns((Apprentice)null);
                _mockPreferencesContext.Setup(a => a.Entities.Find(item.PreferenceId)).Returns(new Preference(preferenceId, preferenceMeaning));

                var handler = new UpdateApprenticePreferencesCommandHandler(_mockApprenticePreferencesContext.Object, _mockApprenticeContext.Object, _mockPreferencesContext.Object);
                Func<Task> result = async () => await handler.Handle(_mockCommand, CancellationToken.None);

                await result.Should().ThrowAsync<InvalidOperationException>();
            }
        }

        [Test, MoqAutoData]
        public async Task AndPreferencesIsNull_ReturnInvalidOperationException(
            Apprentice apprentice)
        {
            foreach (var item in _mockCommand.ApprenticePreferences)
            {
                _mockApprenticeContext.Setup(a => a.Entities.Find(item.ApprenticeId)).Returns(apprentice);
                _mockPreferencesContext.Setup(a => a.Entities.Find(item.PreferenceId)).Returns((Preference)null);

                var handler = new UpdateApprenticePreferencesCommandHandler(_mockApprenticePreferencesContext.Object, _mockApprenticeContext.Object, _mockPreferencesContext.Object);
                Func<Task> result = async () => await handler.Handle(_mockCommand, CancellationToken.None);

                await result.Should().ThrowAsync<InvalidOperationException>();
            }
        }

        [Test]
        public async Task And_BothApprenticeAndPreferencesAreNull_ReturnInvalidOperationException()
        {
            foreach (var item in _mockCommand.ApprenticePreferences)
            {
                _mockApprenticeContext.Setup(a => a.Entities.Find(item.ApprenticeId)).Returns((Apprentice)null);
                _mockPreferencesContext.Setup(a => a.Entities.Find(item.PreferenceId)).Returns((Preference)null);

                var handler = new UpdateApprenticePreferencesCommandHandler(_mockApprenticePreferencesContext.Object, _mockApprenticeContext.Object, _mockPreferencesContext.Object);
                Func<Task> result = async () => await handler.Handle(_mockCommand, CancellationToken.None);

                await result.Should().ThrowAsync<InvalidOperationException>();
            }
        }

        [Test, RecursiveMoqAutoData]
        public async Task AndRecordIsNull_ThenCreateApprenticePreferencesRecord(
            DateTime mockDateTime,
            MailAddress apprenticeAddress,
            string preferenceMeaning,
            string firstName,
            string lastName)
        {
            foreach (var item in _mockCommand.ApprenticePreferences)
            {
                var apprentice = new Apprentice(item.ApprenticeId, firstName, lastName, apprenticeAddress, mockDateTime);
                var preference = new Preference(item.PreferenceId, preferenceMeaning);

                _mockApprenticeContext.Setup(a => a.Entities.Find(item.ApprenticeId)).Returns(apprentice);
                _mockPreferencesContext.Setup(a => a.Entities.Find(item.PreferenceId)).Returns(preference);
                _mockApprenticePreferencesContext
                    .Setup(a => a.GetSinglePreferenceValueAsync(item.ApprenticeId, item.PreferenceId))
                    .ReturnsAsync((Data.Models.ApprenticePreferences)null);

                var handler = new UpdateApprenticePreferencesCommandHandler(_mockApprenticePreferencesContext.Object,
                    _mockApprenticeContext.Object, _mockPreferencesContext.Object);
                var result = await handler.Handle(_mockCommand, CancellationToken.None);

                _mockApprenticePreferencesContext.Verify(a => a.Add(It.IsAny<Data.Models.ApprenticePreferences>()));
                result.GetType().Should().Be(typeof(Unit));
            }
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
            foreach (var item in _mockCommand.ApprenticePreferences)
            {
                var apprentice = new Apprentice(item.ApprenticeId, firstName, lastName, apprenticeAddress, mockDateTime);
                var preference = new Preference(item.PreferenceId, preferenceMeaning);
                var response = new Data.Models.ApprenticePreferences(item.ApprenticeId, item.PreferenceId,
                    item.Status, mockDateTime, mockDateTime2);

                _mockApprenticeContext.Setup(a => a.Entities.Find(item.ApprenticeId)).Returns(apprentice);
                _mockPreferencesContext.Setup(a => a.Entities.Find(item.PreferenceId)).Returns(preference);
                _mockApprenticePreferencesContext
                    .Setup(a => a.GetSinglePreferenceValueAsync(item.ApprenticeId, item.PreferenceId))
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
