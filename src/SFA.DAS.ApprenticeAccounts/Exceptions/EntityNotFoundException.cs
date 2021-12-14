using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace SFA.DAS.ApprenticeAccounts.Exceptions
{
    [Serializable]
    public sealed class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entityName, string id)
            : base($"`{entityName}` Entity `{id}` not found ")
        {
        }

        public EntityNotFoundException(string entityName, string id, Exception innerException)
            : base($"`{entityName}` Entity `{id}` not found ", innerException)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        private EntityNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
