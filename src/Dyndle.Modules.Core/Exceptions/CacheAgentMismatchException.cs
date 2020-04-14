using System;

namespace Dyndle.Modules.Core.Exceptions
{
    [Serializable]
    public class CacheAgentMismatchException : Exception
    {
        /// <summary>
        /// ItemTypeMismatchException
        /// </summary>
        public CacheAgentMismatchException()
        {
        }

        /// <summary>
        /// ItemTypeMismatchException that takes message.
        /// </summary>
        /// <param name="message">The message.</param>
        public CacheAgentMismatchException(string message) : base(message)
        {
        }      
    }
}