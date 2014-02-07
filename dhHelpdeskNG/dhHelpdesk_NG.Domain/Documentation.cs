namespace DH.Helpdesk.Domain
{
    using global::System;

    public class Documentation : Entity
    {
        public int ChangedByUser_Id { get; set; }
        public int CreatedByUser_Id { get; set; }
        public int Customer_Id { get; set; }
        public string Name { get; set; }
        public DateTime ChangeDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
