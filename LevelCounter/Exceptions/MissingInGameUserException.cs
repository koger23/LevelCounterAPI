using System;
using System.Runtime.Serialization;

namespace LevelCounter.Exceptions
{
    [Serializable]
    internal class MissingInGameUserException : Exception
    {
        public MissingInGameUserException()
        {
        }

        public MissingInGameUserException(string message) : base(message)
        {
        }

        public MissingInGameUserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MissingInGameUserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
