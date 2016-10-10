using System.Collections.Generic;

namespace DH.Helpdesk.Domain.BusinessRules
{
    using global::System;

    public class BRActionEntity : Entity
    {
        public BRActionEntity()
        {
            BrActionParams = new List<BRActionParamEntity>();
        }

        public int Rule_Id { get; set; }
        public int ActionType_Id { get; set; }
        public int Sequence { get; set; }
       
        public virtual BRRuleEntity BrRule { get; set; }

        public virtual ICollection<BRActionParamEntity> BrActionParams { get; set; }
    }
}