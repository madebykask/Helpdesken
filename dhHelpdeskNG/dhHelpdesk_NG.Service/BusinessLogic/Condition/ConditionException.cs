

namespace DH.Helpdesk.Services.BusinessLogic.Condition
{
	public class ConditionException : ConditionBaseException
	{
		public ConditionException(string conditionKey, string message) : base(message)
		{
			ConditionKey = conditionKey;
		}

		public string ConditionKey { get; private set; }
	}
}
