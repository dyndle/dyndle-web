using System;

namespace Dyndle.Modules.Core.Exceptions
{
    [Serializable]
    public class LocalizationNotFoundException : Exception
    {
        /// <summary>
        /// LocalizationNotFoundException
        /// </summary>
        public LocalizationNotFoundException()
        {
        }

        /// <summary>
        /// LocalizationNotFoundException that takes message.
        /// </summary>
        /// <param name="message">The message.</param>
        public LocalizationNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// LocalizationNotFoundException that takes message and innerException.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The innerException.</param>
        public LocalizationNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// LocalizationNotFoundException that takes serializationInfo and streamingContext
        /// </summary>
        /// <param name="serializationInfo">The serializationInfo.</param>
        /// <param name="streamingContext">The streamingContext.</param>
        protected LocalizationNotFoundException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
        }
    }
}