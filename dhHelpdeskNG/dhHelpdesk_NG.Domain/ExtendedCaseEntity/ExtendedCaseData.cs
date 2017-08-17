using System;
using System.Collections.Generic;

namespace DH.Helpdesk.Domain.ExtendedCaseEntity
{
    public class ExtendedCaseDataEntity : Entity
    {     
        public int ExtendedCaseFormId { get; set; }

        public Guid ExtendedCaseGuid { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
     
        public virtual ExtendedCaseFormEntity ExtendedCaseForm { get; set; }

        public virtual ICollection<Case_ExtendedCaseEntity> CaseExtendedCaseDatas { get; set; }

        public virtual ICollection<ExtendedCaseValueEntity> ExtendedCaseValues { get; set; }

        
	}
}
