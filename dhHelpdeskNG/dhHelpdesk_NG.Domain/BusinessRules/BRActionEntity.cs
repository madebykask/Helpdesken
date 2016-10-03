namespace DH.Helpdesk.Domain.BusinessRules
{
    using global::System;

    public class BRActionEntity : Entity
    {        
        public int Rule_Id { get; set; }
        public int ActionType_Id { get; set; }
        public int Sequence { get; set; }        
    }
}