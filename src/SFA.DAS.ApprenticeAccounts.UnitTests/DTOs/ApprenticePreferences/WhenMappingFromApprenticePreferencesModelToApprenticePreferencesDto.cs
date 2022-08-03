using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.ApprenticePreferences.GetAllApprenticePreferencesForApprentice;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.DTOs.ApprenticePreferences
{
    public class WhenMappingFromApprenticePreferencesModelToApprenticePreferencesDto
    {
        [Test, RecursiveMoqAutoData]
        public async Task ThenTheFieldsAreCorrectlyMapped(Guid mockGuid, DateTime mockDateTimeOne, DateTime mockDateTimeTwo)
        {
            var preference = new Preference(1, "Pref meaning");
            var apprenticePreferences = new List<Data.Models.ApprenticePreferences>(2) 
            { 
                new Data.Models.ApprenticePreferences(mockGuid, 1, true, mockDateTimeOne, mockDateTimeTwo), 
                new Data.Models.ApprenticePreferences(mockGuid, 2, false, mockDateTimeTwo, mockDateTimeOne) 
            };
            foreach (var item in apprenticePreferences)
            { 
                item.Preference = preference;
            }

            var response =  apprenticePreferences.Select(a => new ApprenticePreferenceDto() { PreferenceId = a.PreferenceId, PreferenceMeaning = a.Preference.PreferenceMeaning, Status = a.Status }).ToList();

            var result = apprenticePreferences.MapToApprenticePreferencesDto();

            result.ApprenticePreferences.Should().BeEquivalentTo(response);
        }
    }
}
