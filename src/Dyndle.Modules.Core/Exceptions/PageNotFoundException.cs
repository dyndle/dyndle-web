using System;

namespace Dyndle.Modules.Core.Exceptions
{
    [Serializable]
    public class PageNotFoundException : Exception
    {
        /// <summary>
        /// PageNotFoundException
        /// </summary>
        public PageNotFoundException()
        {
        }

        /// <summary>
        /// PageNotFoundException that takes message.
        /// </summary>
        /// <param name="message">The message.</param>
        public PageNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// PageNotFoundException that takes message and innerException.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The innerException.</param>
        public PageNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// PageNotFoundException that takes serializationInfo and streamingContext
        /// </summary>
        /// <param name="serializationInfo">The serializationInfo.</param>
        /// <param name="streamingContext">The streamingContext.</param>
        protected PageNotFoundException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
        }
    }
}