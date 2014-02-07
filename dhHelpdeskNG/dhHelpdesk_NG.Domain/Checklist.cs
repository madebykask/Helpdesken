namespace DH.Helpdesk.Domain
{
    using global::System;

    public class Checklist : Entity
    {
        public int Customer_Id { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ChecklistDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
