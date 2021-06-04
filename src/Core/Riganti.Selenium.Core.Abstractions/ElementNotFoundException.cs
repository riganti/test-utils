using System;
using System.Runtime.Serialization;

namespace Riganti.Selenium.Core.Abstractions.Exceptions
{
    [Serializable]
    public class ElementNotFoundException : TestExceptionBase
    {
        public ElementNotFoundException()
        {
        }

        public ElementNotFoundException(string message) : base(message)
        {
        }

        public ElementNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ElementNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}