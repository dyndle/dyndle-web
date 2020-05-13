using System;

namespace Dyndle.Modules.Core.Exceptions
{
    /// <summary>
    /// The exception that is thrown if cache agent is not configured properly.
    /// </summary>
    [Serializable]
    public class CacheAgentMismatchException : Exception
    {
        /// <summary>
        /// CacheAgentMismatchException
        /// </summary>
        public CacheAgentMismatchException()
        {
        }

        /// <summary>
        /// CacheAgentMismatchException that takes message.
        /// </summary>
        /// <param name="message">The message.</param>
        public CacheAgentMismatchException(string message) : base(message)
        {
        }

        /// <summary>
        /// CacheAgentMismatchException that takes message and innerException.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The innerException.</param>
        public CacheAgentMismatchException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// CacheAgentMismatchException that takes serializationInfo and streamingContext
        /// </summary>
        /// <param name="serializationInfo">The serializationInfo.</param>
        /// <param name="streamingContext">The streamingContext.</param>
        protected CacheAgentMismatchException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
        }
    }
}