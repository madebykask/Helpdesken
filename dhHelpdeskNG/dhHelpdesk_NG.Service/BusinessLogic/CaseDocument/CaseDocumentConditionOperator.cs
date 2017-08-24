using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Services.BusinessLogic.CaseDocument
{
	enum CaseDocumentConditionOperator
	{
		Equal,
		EqualOrEmpty,
		NotEqual,
		HasValue,
		IsEmpty,
		LargerThan,
		LargerThanOrEqual,
		LessThan,
		LessThanOrEqual,
		Exists,
		NotExists
	}
}
