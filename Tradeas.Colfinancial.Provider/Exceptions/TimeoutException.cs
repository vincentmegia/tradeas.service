using System;

namespace Tradeas.Colfinancial.Provider.Exceptions
{
    public class TimeoutException : Exception
    {
        public TimeoutException()
        {}

        public TimeoutException(string message) : base(message)
        {}

        public TimeoutException(string message, Exception exception) : base(message, exception)
        {}
    }
}