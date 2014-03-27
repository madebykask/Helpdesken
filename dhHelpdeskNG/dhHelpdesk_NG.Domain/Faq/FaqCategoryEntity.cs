namespace DH.Helpdesk.Domain.Faq
{
    using global::System;
    using global::System.Collections.Generic;

    public class FaqCategoryEntity : Entity
    {
        #region Public Properties

        public DateTime ChangedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }

        public int? Customer_Id { get; set; }

        public virtual ICollection<FaqEntity> FAQs { get; set; }

        public string Name { get; set; }

        public virtual FaqCategoryEntity ParentFAQCategory { get; set; }

        public int? Parent_FAQCategory_Id { get; set; }

        public int PublicFAQCategory { get; set; }

        public virtual ICollection<FaqCategoryEntity> SubFAQCategories { get; set; }

        #endregion
    }
}