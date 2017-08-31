using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Services.BusinessLogic.CaseDocument
{
	class CaseDocumentReplacePropertiesResult
	{
		public CaseDocumentReplacePropertiesResult()
		{
			FailedMappings = new List<KeyValuePair<string, string>>();
		}
		public string Original { get; set; }
		public string Results { get; set; }

		public List<KeyValuePair<string, string>> FailedMappings { get; set; }
	}
}
