namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.Collections.Generic;

    public class ProductArea : Entity
    {
        public int Customer_Id { get; set; }
        public int? MailID { get; set; }
        public int? Parent_ProductArea_Id { get; set; }
        public int IsActive { get; set; }
        public int? WorkingGroup_Id { get; set; }
        public int? Priority_Id { get; set; }
        public string Description { get; set; }
        public string InformUserText { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual MailTemplate MailTemplate { get; set; }
        public virtual ProductArea ParentProductArea { get; set; }
        public virtual WorkingGroupEntity WorkingGroup { get; set; }
        public virtual ICollection<ProductArea> SubProductAreas { get; set; }
        //public virtual ICollection<WorkingGroup> WorkingGroups { get; set; }
        //public virtual Priority Priority { get; set; }
    }
}
