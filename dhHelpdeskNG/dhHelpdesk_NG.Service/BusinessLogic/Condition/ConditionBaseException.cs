using System;

namespace DH.Helpdesk.Services.BusinessLogic.Condition
{
    public class ConditionBaseException : Exception
    {
        public ConditionBaseException(string message) : base(message)
        {
        }

        public ConditionBaseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}