namespace DH.Helpdesk.Domain.BusinessRules
{
    using global::System;

    public class BRActionParamEntity : Entity
    {        
        public int RuleAction_Id { get; set; }
        public int ParamType_Id { get; set; }
        public string ParamValue { get; set; }        
    }
}