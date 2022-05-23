using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Application.Queries.GetSingleApprenticePreferenceValueByIdsQuery;
using SFA.DAS.ApprenticeAccounts.Data;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences.GetSingleApprenticePreferenceByIds;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.Application.Queries.ApprenticePreferences
{
    public class WhenHandlingGetApprenticePreferencesByIds
    {
        [Test, RecursiveMoqAutoData]
        public async Task AndPreferenceIsPopulated_ThenCorrectRecordIsReturned(
            GetSingleApprenticePreferenceValueQuery query,
            Mock<IApprenticePreferencesContext> mockContext,
            bool status,
            DateTime dateTimeOne,
            DateTime dateTimeTwo,
            string mockMeaning,
            MailAddress mockAddress)
        {
            var apprentice = new Apprentice(Guid.NewGuid(), "test", "test", mockAddress, dateTimeTwo);
            var preference = new Preference(query.PreferenceId, mockMeaning);
            var response = new Data.Models.ApprenticePreferences(query.ApprenticeId, query.PreferenceId, status, dateTimeOne, dateTimeTwo)
            {
                Preference = preference,
                Apprentice = apprentice
            };

            mockContext.Setup(m => m.GetSinglePreferenceValueAsync(query.ApprenticeId, query.PreferenceId)).ReturnsAsync(response);

            var handler = new GetSingleApprenticePreferenceValueQueryHandler(mockContext.Object);
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.AreEqual(response.PreferenceId, result.PreferenceId);
            Assert.AreEqual(response.Preference.PreferenceMeaning, result.PreferenceMeaning);
            Assert.AreEqual(response.Status, result.Status);
            Assert.AreEqual(response.UpdatedOn, result.UpdatedOn);

        }

        [Test, MoqAutoData]
        public async Task AndPreferenceIsNull_ThenReturnNewDto(
            GetSingleApprenticePreferenceValueQuery query,
            Mock<IApprenticePreferencesContext> mockContext)
        {
            var response = Task.FromResult(new GetSingleApprenticePreferenceDto());
            mockContext.Setup(m => m.GetSinglePreferenceValueAsync(query.ApprenticeId, query.PreferenceId)).ReturnsAsync((Data.Models.ApprenticePreferences)null);

            var handler = new GetSingleApprenticePreferenceValueQueryHandler(mockContext.Object);
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.AreEqual(result.PreferenceId, response.Result.PreferenceId);
            Assert.AreEqual(result.PreferenceMeaning, response.Result.PreferenceMeaning);
            Assert.AreEqual(result.Status, response.Result.Status);
            Assert.AreEqual(result.UpdatedOn, response.Result.UpdatedOn);

        }
    }
}
