namespace DH.Helpdesk.Domain.Orders
{
    using global::System;

    public class BRConditionEntity : Entity
    {        
        public int Rule_Id { get; set; }
        public string Field_Id { get; set; }
        public string FromValue { get; set; }
        public string ToValue { get; set; }
        public int Sequence { get; set; }        
    }
}