using System;

namespace SFA.DAS.ApprenticeAccounts.Exceptions
{
    public class InvalidInputException : Exception
    {
        public static readonly string InvalidInputApprentice = "No Apprentice record found.";

        public static readonly string InvalidInputPreference = "No Preference record found.";

        public static readonly string MultipleInputs = "More than one apprentice Id has been entered.";

        public InvalidInputException(string message) : base(message) { }
    }

}
