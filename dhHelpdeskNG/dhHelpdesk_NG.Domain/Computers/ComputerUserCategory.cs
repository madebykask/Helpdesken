using DH.Helpdesk.Domain.ExtendedCaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Domain.Computers
{
	public class ComputerUserCategory
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public Guid ComputerUsersCategoryGuid { get; set; }

		public bool IsReadOnly { get; set; }
		public int? CaseSolutionID { get; set; }
		public int CustomerID { get; set; }

		public CaseSolution CaseSolution { get; set; }

		public int? ExtendedCaseFormID { get; set; }
		public ExtendedCaseFormEntity ExtendedCaseForm { get; set; }

		public Customer Customer { get; set; }
	}
}
