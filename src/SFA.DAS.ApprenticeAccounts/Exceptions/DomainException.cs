using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace SFA.DAS.ApprenticeAccounts.Exceptions
{
    [Serializable]
    public sealed class DomainException : Exception
    {
        public DomainException()
        {
        }

        public DomainException(string message)
            : base(message)
        {
        }

        public DomainException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        private DomainException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
