using System;
using DH.Helpdesk.Common.Enums.Condition;

namespace DH.Helpdesk.Domain
{
    public class ConditionEntity : EntityBase
    {
        public int Parent_Id { get; set; }
        public int ConditionType_Id { get; set; }
        public Guid GUID { get; set; }

        public string Property_Name { get; set; }
        public string Values { get; set; }
        public ConditionOperator Operator { get; set; } 
        
    }
}
