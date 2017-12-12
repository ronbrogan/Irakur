using System;
using System.Runtime.Serialization;

namespace Irakur.Pdf.Infrastructure.Serialization
{
    [Serializable]
    internal class SerializationOrderException : Exception
    {
        public SerializationOrderException()
        {
        }

        public SerializationOrderException(string message) : base(message)
        {
        }

        public SerializationOrderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SerializationOrderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}