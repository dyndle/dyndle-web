using System;
using System.Runtime.Serialization;

namespace Dyndle.Modules.Core.Exceptions
{
    /// <summary>
    /// The exception that is thrown during publication resolving where no matching localization was found for the url. 
    /// </summary>
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
        protected LocalizationNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        {
        }
    }
}