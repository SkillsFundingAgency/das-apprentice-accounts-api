using FluentValidation;
using FluentValidation.Results;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace SFA.DAS.ApprenticeAccounts.Exceptions
{
    // By design this class restricts construction options.
#pragma warning disable RCS1194 // Implement exception constructors.

    [Serializable]
    public class RegistrationAlreadyMatchedException : ValidationException
    {
        public RegistrationAlreadyMatchedException(Guid registrationId)
            : base("Registration is already verified", new[]
            {
                new ValidationFailure("Registration", $"Registration {registrationId} is already verified"),
            })
        { }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected RegistrationAlreadyMatchedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}