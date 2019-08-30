using System;
using System.Runtime.Serialization;

namespace LevelCounter.Exceptions
{
    [Serializable]
    internal class HostMisMatchException : Exception
    {
        public HostMisMatchException()
        {
        }

        public HostMisMatchException(string message) : base(message)
        {
        }

        public HostMisMatchException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HostMisMatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}