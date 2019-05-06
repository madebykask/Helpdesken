using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.BusinessData.Models.ReportService
{
	public class HistoricalDataFilter
	{
		public int? CaseStatus { get; set; }
		public DateTime ChangeFrom { get; set; }
		public DateTime ChangeTo { get; set; }

		public int CustomerID { get; set; }
		public List<int> ChangeWorkingGroups { get; set; }
		public DateTime? RegisterFrom { get; set; }
		public DateTime? RegisterTo { get; set; }
		public DateTime? CloseFrom { get; set; }
		public DateTime? CloseTo { get; set; }
		public List<int> Administrators { get; set; }
		public List<int> Departments { get; set; }
		public List<int> CaseTypes { get; set; }
		public List<int> ProductAreas { get; set; }
		public List<int> WorkingGroups { get; set; }
	}
}
