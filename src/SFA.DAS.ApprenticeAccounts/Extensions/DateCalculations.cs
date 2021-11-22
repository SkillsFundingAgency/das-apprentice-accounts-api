#nullable enable

using System;

namespace SFA.DAS.ApprenticeCommitments.Extensions
{
    public static class DateCalculations
    {
        public static int DifferenceInMonths(DateTime earlier, DateTime later)
            => ((later.Year - earlier.Year) * 12)
                - earlier.Month
                + later.Month
                + 1; // inclusive of both ends
    }
}