using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Domain.ExtendedCaseEntity
{
	public class Case_CaseSection_ExtendedCase
	{
		public int Case_Id { get; set; }

		public int ExtendedCaseData_Id { get; set; }

		public int CaseSection_Id { get; set; }

		public virtual Case CaseEntity { get; set; }

		public virtual ExtendedCaseDataEntity ExtendedCaseData { get; set; }

		public virtual Cases.CaseSection CaseSection { get; set; }
	}


}
