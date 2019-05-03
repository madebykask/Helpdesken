using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.BusinessData.Models.ReportService
{
	public class HistoricalDataFilter
	{
		public DateTime From { get; set; }
		public DateTime To { get; set; }

		public int CustomerID { get; set; }
	}
}
