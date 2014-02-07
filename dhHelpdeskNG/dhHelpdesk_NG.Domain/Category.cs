namespace DH.Helpdesk.Domain
{
    using global::System;

    public class Category : Entity
    {
        public int Customer_Id { get; set; }
        public int IsActive { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
