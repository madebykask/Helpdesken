using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Domain.ExtendedCaseEntity
{
	public class ExtendedCaseReportField : Entity
	{
		public int ExtendedCaseReport_Id { get; set; }
		public ExtendedCaseReport ExtendedCaseReport { get; set; }
		public string FieldId { get; set; }

		public string Name { get; set; }

		public int SortOrder { get; set; }
	}
}
