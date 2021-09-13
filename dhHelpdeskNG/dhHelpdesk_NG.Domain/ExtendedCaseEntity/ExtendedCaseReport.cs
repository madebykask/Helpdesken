using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Domain.ExtendedCaseEntity
{
	public class ExtendedCaseReport : Entity
	{
		public int Id { get; set; }
		public ExtendedCaseFormEntity ExtendedCaseForm { get; set; }
		public Customer Customer { get; set; }

		public int Customer_Id { get; set; }
		public int ExtendedCaseForm_Id { get; set; }

        public int? Active { get; set; }

        public virtual ICollection<ExtendedCaseReportField> ExtendedCaseReportFields { get; set; }
	}
}
