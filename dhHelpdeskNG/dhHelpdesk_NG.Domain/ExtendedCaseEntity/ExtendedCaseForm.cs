using global::System.Collections.Generic;

namespace DH.Helpdesk.Domain.ExtendedCaseEntity
{
    public class ExtendedCaseFormEntity : Entity
    {
        public ExtendedCaseFormEntity()
        {
            this.CaseSolutions = new List<CaseSolution>();
            this.ExtendedCaseDatas = new List<ExtendedCaseDataEntity>();
        }


        public string Name { get; set; }
        public int Version { get; set; }

        public virtual ICollection<CaseSolution> CaseSolutions { get; set; }
        public virtual ICollection<ExtendedCaseDataEntity> ExtendedCaseDatas { get; set; }
    }
}
