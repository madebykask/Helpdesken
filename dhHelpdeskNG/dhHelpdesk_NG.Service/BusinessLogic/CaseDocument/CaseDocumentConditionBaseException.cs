using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Services.BusinessLogic.CaseDocument
{

	public class CaseDocumentConditionBaseException : Exception
	{
		public CaseDocumentConditionBaseException(string message) : base(message)
		{
		}
	}
}
