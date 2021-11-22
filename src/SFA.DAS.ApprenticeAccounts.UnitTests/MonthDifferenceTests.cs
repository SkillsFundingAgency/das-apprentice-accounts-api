using FluentAssertions;
using NUnit.Framework;
using System;
using static SFA.DAS.ApprenticeCommitments.Extensions.DateCalculations;

namespace SFA.DAS.ApprenticeCommitments.UnitTests
{
    public class MonthDifferenceTests
    {
        private static readonly object[] MonthCases =
        {
            new object[] { new DateTime(2020, 01, 31), new DateTime(2020, 01, 01), 1 },
            new object[] { new DateTime(2020, 01, 01), new DateTime(2020, 01, 31), 1 },
            new object[] { new DateTime(2020, 01, 01), new DateTime(2020, 02, 01), 2 },
            new object[] { new DateTime(2020, 12, 01), new DateTime(2021, 01, 01), 2 },
            new object[] { new DateTime(2020, 01, 01), new DateTime(2021, 01, 01), 13 },
            new object[] { new DateTime(2020, 09, 01), new DateTime(2022, 07, 31), 23 },
        };

        [TestCaseSource(nameof(MonthCases))]
        public void Can_calculate_the_difference_between_two_dates_in_months(DateTime first, DateTime second, int expectedMonthDifference)
            => DifferenceInMonths(first, second).Should().Be(expectedMonthDifference);
    }
}