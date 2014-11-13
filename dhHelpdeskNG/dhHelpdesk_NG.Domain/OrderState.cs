namespace DH.Helpdesk.Domain
{
    using DH.Helpdesk.Domain.Interfaces;

    using global::System;
    using global::System.Collections.Generic;

    public class OrderState : Entity, ICustomerEntity, INamedEntity
    {
        public int? ChangedByUser_Id { get; set; }
        public int CreateCase { get; set; }
        public int? CreatedByUser_Id { get; set; }
        public int Customer_Id { get; set; }
        public int EnableToOrderer { get; set; }
        public int IsActive { get; set; }
        public int NotifyOrderer { get; set; }
        public int NotifyReceiver { get; set; }
        public int SelectedInSearchCondition { get; set; }
        public int SortOrder { get; set; }
        public string EMailList { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        //public virtual Customer Customer { get; set; }
        public virtual User CreatedByUser { get; set; }
        public virtual User ChangedByUser { get; set; }
        public virtual ICollection<Setting> Settings { get; set; }
    }
}
