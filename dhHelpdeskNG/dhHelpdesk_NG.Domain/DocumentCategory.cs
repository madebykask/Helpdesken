namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.Collections.Generic;

    public class DocumentCategory : Entity
    {
        public int? ChangedByUser_Id { get; set; }
        public int CreatedByUser_Id { get; set; }
        public int Customer_Id { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool ShowOnExternalPage { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual User CreatedByUser { get; set; }
        public virtual User ChangedByUser { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
    }
}
