namespace DH.Helpdesk.Domain.Changes
{
    using global::System;

    public class ChangeObjectEntity : Entity
    {
        public int Customer_Id { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

