namespace DH.Helpdesk.Domain.Changes
{
    using global::System;

    public class ChangePriorityEntity : Entity
    {
        public int Customer_Id { get; set; }

        public virtual Customer Customer { get; set; }

        public string ChangePriority { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

