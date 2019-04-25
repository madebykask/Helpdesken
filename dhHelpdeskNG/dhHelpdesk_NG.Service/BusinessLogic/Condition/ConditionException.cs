using System;

namespace DH.Helpdesk.Services.BusinessLogic.Condition
{
	public class ConditionException : ConditionBaseException
	{
		public ConditionException(string conditionKey, string message, Exception innerEx) : base(message, innerEx)
		{
			ConditionKey = conditionKey;
		}

		public string ConditionKey { get; protected set; }
	}
}
