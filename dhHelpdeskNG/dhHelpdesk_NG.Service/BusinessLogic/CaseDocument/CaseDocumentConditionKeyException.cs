using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Services.BusinessLogic.CaseDocument
{
	public class CaseDocumentConditionKeyException : CaseDocumentConditionBaseException
	{
		public CaseDocumentConditionKeyException(int extendedFormCaseID, string conditionKey, Guid conditionTextGuid, string message) : base (message)
		{
			ExtendedCaseFormID = extendedFormCaseID;
			ConditionKey = conditionKey;
			ConditionTextGuid = conditionTextGuid;
		}

		public int ExtendedCaseFormID { get; protected set; }
		public string ConditionKey { get; protected set; }

		public Guid ConditionTextGuid { get; set; }
	}
}
