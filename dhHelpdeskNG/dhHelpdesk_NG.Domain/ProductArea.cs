namespace DH.Helpdesk.Domain
{
    using DH.Helpdesk.Domain.Interfaces;
    using DH.Helpdesk.Domain.MailTemplates;

    using global::System;
    using global::System.Collections.Generic;

    public class ProductArea : Entity, ICustomerEntity, IActiveEntity
    {
        public int Customer_Id { get; set; }
        public int? MailID { get; set; }
        public int? Parent_ProductArea_Id { get; set; }
        public int IsActive { get; set; }
        public int? WorkingGroup_Id { get; set; }
        public int? Priority_Id { get; set; }
        public int ShowOnExternalPage { get; set; }
        public string Description { get; set; }
        public string InformUserText { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ProductAreaGUID { get; set; }
        public int ShowOnExtPageCases { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual MailTemplateEntity MailTemplate { get; set; }
        public virtual ProductArea ParentProductArea { get; set; }
        public virtual WorkingGroupEntity WorkingGroup { get; set; }
        public virtual ICollection<ProductArea> SubProductAreas { get; set; }
        public virtual ICollection<WorkingGroupEntity> WorkingGroups { get; set; }
        public virtual ICollection<Invoice.InvoiceArticleEntity> InvoiceArticles { get; set; }
        public virtual ICollection<CaseTypeProductArea> CaseTypeProductAreas { get; set; }
        public virtual Priority Priority { get; set; }
    }

    
}
