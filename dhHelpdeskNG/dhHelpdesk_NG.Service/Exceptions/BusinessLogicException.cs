namespace DH.Helpdesk.Services.Exceptions
{
    using System;

    public abstract class BusinessLogicException : Exception
    {
        protected BusinessLogicException(string message) : base(message)
        {
        }
    }
}