namespace dhHelpdesk_NG.Common.Exceptions
{
    using System;

    public abstract class BusinessLogicException : Exception
    {
        protected BusinessLogicException(string message) : base(message)
        {
        }
    }
}