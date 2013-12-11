using System;
using System.Collections.Generic;

namespace dhHelpdesk_NG.Domain
{
    public class MailTemplate : Entity
    {
        public MailTemplate() { }

        public MailTemplate(int customerId)
        {
            Customer_Id = customerId;
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
