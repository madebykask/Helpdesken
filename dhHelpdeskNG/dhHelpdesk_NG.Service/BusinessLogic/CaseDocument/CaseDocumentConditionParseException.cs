using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Services.BusinessLogic.CaseDocument
{
	public class CaseDocumentConditionParseException : CaseDocumentConditionBaseException 
	{
		public CaseDocumentConditionParseException(string value, string conditionKey, string conditionValues, string conditionPart, string message) : base(message) 
		{
			ConditionKey = conditionKey;
			Value = value;
			ConditionValues = conditionValues;
			ConditionPart = conditionPart;
		}

		public string Value { get; private set; }
		public string ConditionValues { get; private set; }
		public string ConditionKey { get; private set; }
		public string ConditionPart { get; private set; }

	}
}
