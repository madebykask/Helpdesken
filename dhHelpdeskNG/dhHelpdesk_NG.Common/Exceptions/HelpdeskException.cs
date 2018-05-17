using System;

namespace DH.Helpdesk.Common.Exceptions
{
    public class HelpdeskException : Exception
    {
        public HelpdeskException()
        {
        }

        public HelpdeskException(string message) : base(message)
        {
        }
    }
}