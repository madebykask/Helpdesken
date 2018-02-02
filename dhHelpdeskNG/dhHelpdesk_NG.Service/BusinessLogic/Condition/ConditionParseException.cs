
using DH.Helpdesk.Common.Enums.Condition;

namespace DH.Helpdesk.Services.BusinessLogic.Condition
{
	public class ConditionParseException : ConditionBaseException 
	{
		public ConditionParseException(string value, ConditionOperator conditionKey, string conditionValues, string conditionPart, string message) : base(message) 
		{
			ConditionKey = conditionKey;
			Value = value;
			ConditionValues = conditionValues;
			ConditionPart = conditionPart;
		}

		public string Value { get; private set; }
		public string ConditionValues { get; private set; }
		public ConditionOperator ConditionKey { get; private set; }
		public string ConditionPart { get; private set; }

	}
}
