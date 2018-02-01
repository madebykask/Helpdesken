using DH.Helpdesk.Domain.Cases;
using DH.Helpdesk.Domain.ExtendedCaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Domain.ExtendedCaseEntity
{
	public class CaseSolution_CaseSection_ExtendedCaseForm : Entity
	{
		public int CaseSectionID { get; set; }

		public int ExtendedCaseFormID { get; set; }

		public int CaseSolutionID { get; set; }

		public CaseSection CaseSection { get; set; }
		public ExtendedCaseFormEntity ExtendedCaseForm { get; set; }

		public CaseSolution CaseSolution { get; set; }
	}
}
