namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.Collections.Generic;

    public class FAQCategory : Entity
    {
        public int ? Customer_Id { get; set; }
        public int ? Parent_FAQCategory_Id { get; set; }
        public int PublicFAQCategory { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual FAQCategory ParentFAQCategory { get; set; }
        public virtual ICollection<FAQ> FAQs { get; set; }
        public virtual ICollection<FAQCategory> SubFAQCategories { get; set; }
    }
}
