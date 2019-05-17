using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.BusinessData.Models.ReportService
{
	public class HistoricalDataFilter: CommonReportDataFilter
	{
		public DateTime ChangeFrom { get; set; }
		public DateTime ChangeTo { get; set; }
		public List<int> ChangeWorkingGroups { get; set; }
		public bool IncludeCasesWithNoWorkingGroup { get; set; }
	}
}
