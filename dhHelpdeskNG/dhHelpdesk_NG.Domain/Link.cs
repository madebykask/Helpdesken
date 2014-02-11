namespace DH.Helpdesk.Domain
{
    using global::System.Collections.Generic;
    using global::System;

    public class Link : Entity
    {
        public int? Customer_Id { get; set; }
        public int? Document_Id { get; set; }
        public int OpenInNewWindow { get; set; }
        public int ShowOnStartPage { get; set; }
        public int? LinkGroup_Id { get; set; }
        public string URLAddress { get; set; }
        public string URLName { get; set; }
        public string SortOrder { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }


        public virtual Customer Customer { get; set; }
        public virtual Document Document { get; set; }
        public virtual ICollection<User> LinkUsers { get; set; }

    }
}
