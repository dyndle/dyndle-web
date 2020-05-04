using System;

namespace Dyndle.Modules.Core.Exceptions
{
    /// <summary>
    /// The exception that is thrown when during querying the found template does not match the item type. 
    /// </summary>
    [Serializable]
    public class ItemTypeMismatchException : Exception
    {
        /// <summary>
        /// ItemTypeMismatchException
        /// </summary>
        public ItemTypeMismatchException()
        {
        }

        /// <summary>
        /// ItemTypeMismatchException that takes message.
        /// </summary>
        /// <param name="message">The message.</param>
        public ItemTypeMismatchException(string message) : base(message)
        {
        }

        /// <summary>
        /// ItemTypeMismatchException that takes message and innerException.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The innerException.</param>
        public ItemTypeMismatchException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// ItemTypeMismatchException that takes serializationInfo and streamingContext
        /// </summary>
        /// <param name="serializationInfo">The serializationInfo.</param>
        /// <param name="streamingContext">The streamingContext.</param>
        protected ItemTypeMismatchException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
        }
    }
}