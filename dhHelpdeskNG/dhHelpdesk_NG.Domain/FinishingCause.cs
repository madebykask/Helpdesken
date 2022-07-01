namespace DH.Helpdesk.Domain
{
    using DH.Helpdesk.Domain.Interfaces;

    using global::System;
    using global::System.Collections.Generic;

    public class FinishingCause : Entity, ICustomerEntity, IActiveEntity
    {
        public int Customer_Id { get; set; }
        public int IsActive { get; set; }
        public int? FinishingCauseCategory_Id { get; set; }
        public int? Parent_FinishingCause_Id { get; set; }
        public int PromptUser { get; set; }
        public string Name { get; set; }
        public bool Merged { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? FinishingCauseGUID { get; set; }
        
        public virtual Customer Customer { get; set; }
        public virtual FinishingCauseCategory FinishingCauseCategory { get; set; }
        public virtual FinishingCause ParentFinishingCause { get; set; }
        public virtual ICollection<FinishingCause> SubFinishingCauses { get; set; }
    }
}
