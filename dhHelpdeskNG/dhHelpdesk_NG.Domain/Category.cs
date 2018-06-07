using DH.Helpdesk.Domain.Interfaces;

namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.Collections.Generic;

    public class Category : Entity, ICustomerEntity
    {
        public int Customer_Id { get; set; }
        public int IsActive { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public Guid? CategoryGUID { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? Parent_Category_Id { get; set; }

        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Category> SubCategories { get; set; }
    }
}
