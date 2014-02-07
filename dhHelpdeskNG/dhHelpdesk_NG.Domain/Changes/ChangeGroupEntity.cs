namespace DH.Helpdesk.Domain.Changes
{
    using global::System;

    public class ChangeGroupEntity : Entity
    {
        public int Customer_Id { get; set; }
        
        public string ChangeGroup { get; set; }
        
        public DateTime ChangedDate { get; set; }
        
        public DateTime CreatedDate { get; set; }
    }
}

