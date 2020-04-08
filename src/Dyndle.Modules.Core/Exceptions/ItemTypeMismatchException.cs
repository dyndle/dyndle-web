using System;

namespace Dyndle.Modules.Core.Exceptions
{
    public class ItemTypeMismatchException : Exception
    {
        public ItemTypeMismatchException()
        {
        }

        public ItemTypeMismatchException(string message) : base(message)
        {
        }

        public ItemTypeMismatchException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}