using global::System.Collections.Generic;
using System;
using DH.Helpdesk.Domain.ExtendedCaseEntity;

namespace DH.Helpdesk.Domain.ExtendedCaseEntity
{
    public class ExtendedCaseDataEntity : Entity
    {
        public ExtendedCaseDataEntity()
        {
            this.Cases = new List<Case>();
        }

        public int ExtendedCaseFormId { get; set; }
        public Guid ExtendedCaseGuid { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual ICollection<Case> Cases { get; set; }

        public virtual ExtendedCaseFormEntity ExtendedCaseForm { get; set; }
    }
}
