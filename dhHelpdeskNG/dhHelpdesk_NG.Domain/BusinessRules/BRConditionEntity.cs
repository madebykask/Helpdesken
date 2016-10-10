namespace DH.Helpdesk.Domain.BusinessRules
{
    using global::System;

    public class BRConditionEntity : Entity
    {        
        public int Rule_Id { get; set; }
        public string Field_Id { get; set; }
        public string FromValue { get; set; }
        public string ToValue { get; set; }
        public int Sequence { get; set; }

        public virtual BRRuleEntity BrRule { get; set; }
    }
}