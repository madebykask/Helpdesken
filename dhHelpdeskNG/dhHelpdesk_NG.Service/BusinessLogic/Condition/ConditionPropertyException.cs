using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Services.BusinessLogic.Condition
{
	class ConditionPropertyException : ConditionBaseException
	{
		public ConditionPropertyException(int extendedFormCaseID, string property, Guid conditionTextGuid, string message) : base(message)
		{
			ExtendedCaseFormID = extendedFormCaseID;
			Property = property;
			ConditionTextGuid = conditionTextGuid;
		}

		public int ExtendedCaseFormID { get; protected set; }
		public string Property { get; protected set; }

		public Guid ConditionTextGuid { get; set; }
	}
}
