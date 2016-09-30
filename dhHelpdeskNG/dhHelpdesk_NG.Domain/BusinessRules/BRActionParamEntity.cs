namespace DH.Helpdesk.Domain.Orders
{
    using global::System;

    public class BRActionParamEntity : Entity
    {        
        public int RuleAction_Id { get; set; }
        public int ParamType_Id { get; set; }
        public string ParamValue { get; set; }        
    }
}