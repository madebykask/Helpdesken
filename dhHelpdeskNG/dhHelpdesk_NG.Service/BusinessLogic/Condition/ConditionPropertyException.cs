using System;

namespace DH.Helpdesk.Services.BusinessLogic.Condition
{
    public class ConditionPropertyException : ConditionBaseException
    {
        public ConditionPropertyException(string property, Guid conditionTextGuid, string message) : base(message)
        {
            Property = property;
            ConditionTextGuid = conditionTextGuid;
        }

        public string Property { get; protected set; }
        public Guid ConditionTextGuid { get; protected set; }
    }
}
