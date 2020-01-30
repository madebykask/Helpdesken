using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.BusinessData.Models.Case
{
	public class ExtendedCaseReportModel
	{
		public int ID { get; set; }
		public string Name { get; set; }

		public int ExtendedCaseFormId { get; set; }

		public IList<ExtendedCaseReportFieldModel> Fields { get; set; }
	}
}
