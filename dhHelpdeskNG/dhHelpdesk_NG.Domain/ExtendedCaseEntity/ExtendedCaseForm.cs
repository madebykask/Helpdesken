using System;
using global::System.Collections.Generic;

namespace DH.Helpdesk.Domain.ExtendedCaseEntity
{
    public class ExtendedCaseFormEntity : Entity
    {
        public ExtendedCaseFormEntity()
        {
        }


        public string Name { get; set; }
        public int Version { get; set; }


        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<CaseSolution> CaseSolutions { get; set; }
        public virtual ICollection<ExtendedCaseDataEntity> ExtendedCaseDatas { get; set; }

        public virtual ICollection<Case_ExtendedCaseEntity> Case_ExtendedCases { get; set; }
    }
}
