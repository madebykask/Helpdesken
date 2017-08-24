using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Services.BusinessLogic.CaseDocument
{
	// TODO: Move to own files, JWE
	public class CaseDocumentConditionResult
	{
		public bool Show { get; set; }
		public Exception FieldException { get; set; }
	}
}
