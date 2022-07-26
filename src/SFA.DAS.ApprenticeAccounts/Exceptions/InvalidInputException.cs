using System;

namespace SFA.DAS.ApprenticeAccounts.Exceptions
{
    public class InvalidInputException : Exception
    {
        public InvalidInputException(Guid apprenticeId, int preferenceId) 
            :base($"No Apprentice record found, or no Preference record found, or neither found. Apprentice Id used: {apprenticeId}, Preference Id used: {preferenceId}")
        {
            
        }

        public InvalidInputException()
            : base("More than one apprentice Id has been entered.")
        {

        }
    }
}
