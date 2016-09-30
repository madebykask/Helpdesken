namespace DH.Helpdesk.Domain.Orders
{
    using global::System;

    public class BRRuleEntity : Entity
    {

        public int Customer_Id { get; set; }
        public string Name { get; set; }
        public int Event_Id { get; set; }
        public int Sequence { get; set; }
        public int Status { get; set; }
        public bool ContinueOnSuccess { get; set; }
        public bool ContinueOnError { get; set; }
        
        public DateTime CreatedTime { get; set; }
        public int CreatedByUser_Id { get; set; }

        public DateTime ChangedTime { get; set; }
        public int ChangedByUser_Id { get; set; }

        public virtual User CreatedByUser { get; set; }
    }
}