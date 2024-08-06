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
            string mockPreferenceHint,
            MailAddress mockApprenticeMailAddress,
            string mockFirstName,
            string mockLastName, 
            string govUkIdentifier)
        {
            var apprentice = new Apprentice(Guid.NewGuid(), mockFirstName, mockLastName, mockApprenticeMailAddress, mockDateOfBirth, govUkIdentifier);
            var preference = new Preference(query.PreferenceId, mockPreferenceMeaning, mockPreferenceHint);
            var response =
                new Data.Models.ApprenticePreferences(query.ApprenticeId, query.PreferenceId, mockStatus, mockCreatedOn,
                    mockUpdatedOn) { Preference = preference, Apprentice = apprentice };

            mockContext.Setup(m =>
                    m.GetApprenticePreferenceForApprenticeAndPreference(query.ApprenticeId, query.PreferenceId))
                .ReturnsAsync(response);

            var handler = new GetApprenticePreferenceForApprenticeAndPreferenceQueryHandler(mockContext.Object);
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.PreferenceId, Is.EqualTo(response.PreferenceId));
            Assert.That(result.PreferenceMeaning, Is.EqualTo(response.Preference.PreferenceMeaning));
            Assert.That(response.Status, Is.EqualTo(result.Status));
            Assert.That(response.UpdatedOn, Is.EqualTo(result.UpdatedOn));
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

            Assert.That(response.Result.PreferenceId, Is.EqualTo(result.PreferenceId));
            Assert.That(response.Result.PreferenceMeaning, Is.EqualTo(result.PreferenceMeaning) );
            Assert.That(response.Result.Status, Is.EqualTo(result.Status));
            Assert.That(response.Result.UpdatedOn, Is.EqualTo(result.UpdatedOn));
        }
    }
}