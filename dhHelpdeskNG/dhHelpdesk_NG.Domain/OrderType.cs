namespace DH.Helpdesk.Domain
{
    using DH.Helpdesk.Domain.Interfaces;

    using global::System;
    using global::System.Collections.Generic;

    public class OrderType : Entity, ICustomerEntity, INamedEntity
    {
        public int? CreateCase_CaseType_Id { get; set; }
        public int Customer_Id { get; set; }
        public int? Document_Id { get; set; }
        public int IsActive { get; set; }
        public int IsDefault { get; set; }
        public int? Parent_OrderType_Id { get; set; }
        public string CaptionOrdererInfo { get; set; }
        public string CaptionUserInfo { get; set; }
        public string Description { get; set; }
        public string EMail { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Case CreateCase_CaseType { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Document Document { get; set; }
        public virtual OrderType ParentOrderType { get; set; }
        public virtual ICollection<OrderType> SubOrderTypes { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
