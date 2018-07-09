using System;

namespace Tradeas.Colfinancial.Provider.Exceptions
{
    public class BackOfficeOfflineException : Exception
    {
        public BackOfficeOfflineException()
        {}

        public BackOfficeOfflineException(string message) : base(message)
        {}

        public BackOfficeOfflineException(string message, Exception exception) : base(message, exception)
        {}
    }
}