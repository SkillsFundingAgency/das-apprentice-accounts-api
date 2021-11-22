using FluentValidation;
using FluentValidation.Results;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace SFA.DAS.ApprenticeCommitments.Exceptions
{
// By design this class restricts construction options.
#pragma warning disable RCS1194 // Implement exception constructors.

    [Serializable]
    public class IdentityNotVerifiedException : ValidationException
    {
        public IdentityNotVerifiedException(string message)
            : base(message, new[]
            {
                new ValidationFailure("PersonalDetails", "Sorry, your identity has not been verified, please check your details"),
            })
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected IdentityNotVerifiedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}