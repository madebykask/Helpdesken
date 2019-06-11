using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.BusinessData.Models.ReportService
{
	public class HistoricalDataResult
	{
		public int CaseID { get; set; }
		public int CaseTypeID { get; set; }
		public int? WorkingGroupID { get; set; }
		public DateTime Created { get; set; }
		public string WorkingGroup { get; set; }
		public string CaseType { get; set; }
	}
}
