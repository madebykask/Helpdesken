using DH.Helpdesk.Domain.ExtendedCaseEntity;
using System;
using System.Collections.Generic;

namespace DH.Helpdesk.Domain.Cases
{
    public class CaseSection : Entity
    {
        public int Customer_Id { get; set; }

        public int SectionType { get; set; }

        public bool IsNewCollapsed { get; set; }

        public bool IsEditCollapsed { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual List<CaseSectionField> CaseSectionFields { get; set; }

        public virtual List<CaseSectionLanguage> CaseSectionLanguages { get; set; }

		public virtual List<Case_CaseSection_ExtendedCase> Case_CaseSection_ExtendedCases { get; set; }
	}
}
