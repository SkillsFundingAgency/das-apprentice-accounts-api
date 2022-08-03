using System;

namespace SFA.DAS.ApprenticeAccounts.Exceptions
{
    public class InvalidInputException : Exception
    {
        public static class ExceptionMessages
        {
            public static readonly string InvalidInputApprentice = "No Apprentice record found.";

            public static readonly string InvalidInputPreference = "No Preference record found.";

            public static readonly string MultipleInputs = "More than one apprentice Id has been entered.";
        }

        public static Exception CreateException(string message)
        { 
            var invalidInputException = new Exception(message);
            return invalidInputException;
        }
    }

}
