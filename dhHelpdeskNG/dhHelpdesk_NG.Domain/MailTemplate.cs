namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.Collections.Generic;

    public class MailTemplate : Entity
    {
        public MailTemplate() { }

        public MailTemplate(int customerId)
        {
            this.Customer_Id = customerId;
        }

        public int? AccountActivity_Id { get; set; }
        public int? Customer_Id { get; set; }
        public int IsStandard { get; set; }
        public int MailID { get; set; }
        public int? OrderType_Id { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid MailTemplateGUID { get; set; }

        public virtual AccountActivity AccountActivity { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual OrderType OrderType { get; set; }
        public virtual ICollection<MailTemplateLanguage> MailTemplateLanguages { get; set; }
    }
}
