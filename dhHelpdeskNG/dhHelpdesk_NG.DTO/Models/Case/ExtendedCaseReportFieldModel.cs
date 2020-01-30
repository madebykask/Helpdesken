using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.BusinessData.Models.Case
{

	public class ExtendedCaseReportFieldModel
	{
		public string FieldId { get; set; }
		public string Name { get; set; }

		public int SortOrder { get; set; }
	}
}
