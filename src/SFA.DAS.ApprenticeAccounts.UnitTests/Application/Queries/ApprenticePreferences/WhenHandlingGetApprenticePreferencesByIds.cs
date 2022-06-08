using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Queries.GetApprenticePreferenceForApprenticeAndPreferenceQuery;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences.GetApprenticePreferenceForApprenticeAndPreference;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Application.Queries.ApprenticePreferences
{
    public class WhenHandlingGetApprenticePreferencesByIds
    {
        [Test]
        [RecursiveMoqAutoData]
        public async Task AndPreferenceIsPopulated_ThenCorrectRecordIsReturned(
            GetApprenticePreferenceForApprenticeAndPreferenceQuery query,
            Mock<IApprenticePreferencesContext> mockContext,
            bool mockStatus,
            DateTime mockDateOfBirth,
            DateTime mockCreatedOn,
            DateTime mockUpdatedOn,
            string mockPreferenceMeaning,
            MailAddress mockApprenticeMailAddress,
            string mockFirstName,
            string mockLastName)
        {
            var apprentice = new Apprentice(Guid.NewGuid(), mockFirstName, mockLastName, mockApprenticeMailAddress, mockDateOfBirth);
            var preference = new Preference(query.PreferenceId, mockPreferenceMeaning);
            var response =
                new Data.Models.ApprenticePreferences(query.ApprenticeId, query.PreferenceId, mockStatus, mockCreatedOn,
                    mockUpdatedOn) { Preference = preference, Apprentice = apprentice };

            mockContext.Setup(m =>
                    m.GetApprenticePreferenceForApprenticeAndPreference(query.ApprenticeId, query.PreferenceId))
                .ReturnsAsync(response);

            var handler = new GetApprenticePreferenceForApprenticeAndPreferenceQueryHandler(mockContext.Object);
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.AreEqual(response.PreferenceId, result.PreferenceId);
            Assert.AreEqual(response.Preference.PreferenceMeaning, result.PreferenceMeaning);
            Assert.AreEqual(response.Status, result.Status);
            Assert.AreEqual(response.UpdatedOn, result.UpdatedOn);
        }

        [Test]
        [MoqAutoData]
        public async Task AndPreferenceIsNull_ThenReturnNewDto(
            GetApprenticePreferenceForApprenticeAndPreferenceQuery query,
            Mock<IApprenticePreferencesContext> mockContext)
        {
            var response = Task.FromResult(new GetApprenticePreferenceForApprenticeAndPreferenceDto());
            mockContext.Setup(m =>
                    m.GetApprenticePreferenceForApprenticeAndPreference(query.ApprenticeId, query.PreferenceId))
                .ReturnsAsync((Data.Models.ApprenticePreferences)null);

            var handler = new GetApprenticePreferenceForApprenticeAndPreferenceQueryHandler(mockContext.Object);
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.AreEqual(result.PreferenceId, response.Result.PreferenceId);
            Assert.AreEqual(result.PreferenceMeaning, response.Result.PreferenceMeaning);
            Assert.AreEqual(result.Status, response.Result.Status);
            Assert.AreEqual(result.UpdatedOn, response.Result.UpdatedOn);
        }
    }
}