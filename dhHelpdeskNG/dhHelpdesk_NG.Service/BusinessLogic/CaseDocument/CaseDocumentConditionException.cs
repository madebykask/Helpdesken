using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Services.BusinessLogic.CaseDocument
{
	public class CaseDocumentConditionException : CaseDocumentConditionBaseException
	{
		public CaseDocumentConditionException(string conditionKey, string message) : base(message)
		{
			ConditionKey = conditionKey;
		}

		public string ConditionKey { get; private set; }
	}
}
