namespace DH.Helpdesk.Domain
{
    using global::System;

    public class Checklists : Entity
    {
        public int Customer_Id { get; set; }
        public string ChecklistName { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
