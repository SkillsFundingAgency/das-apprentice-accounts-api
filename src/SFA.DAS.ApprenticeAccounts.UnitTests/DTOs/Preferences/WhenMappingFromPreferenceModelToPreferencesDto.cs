﻿using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeAccounts.Data.Models;
using SFA.DAS.ApprenticeAccounts.DTOs.Preferences.GetAllPreferences;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.UnitTests.DTOs.Preferences
{
    public class WhenMappingFromPreferenceModelToPreferencesDto
    {
        [Test, RecursiveMoqAutoData]
        public async Task ThenTheFieldsAreCorrectlyMapped(
            int preferenceIdOne,
            int preferenceIdTwo,
            string preferenceMeaningOne,
            string preferenceMeaningTwo,
            string preferenceHintOne,
            string preferenceHintTwo)
        {
            var response = new List<PreferenceDto>();

            var preferences = new List<Preference>(2) { new Preference(preferenceIdOne, preferenceMeaningOne, preferenceHintOne), new Preference(preferenceIdTwo, preferenceMeaningTwo, preferenceHintTwo) };

            foreach (var p in preferences)
            {
                var preferenceDtoOption = new PreferenceDto()
                {
                    PreferenceId = p.PreferenceId,
                    PreferenceMeaning = p.PreferenceMeaning,
                    PreferenceHint = p.PreferenceHint
                };
                response.Add(preferenceDtoOption);
            }

            var result = preferences.MapToPreferenceDto();

            result.Preferences.Should().BeEquivalentTo(response);
        }
    }
}
