using System;

namespace DH.Helpdesk.Services.BusinessLogic.Condition
{
    public class ExCaseConditionPropertyException : ConditionPropertyException
    {
        public ExCaseConditionPropertyException(int extendedCaseFormId, string property, Guid conditionTextGuid, string message) 
            : base(property, conditionTextGuid, message)
        {
            ExtendedCaseFormId = extendedCaseFormId;
        }

        public int ExtendedCaseFormId { get; protected set; }
    }
}