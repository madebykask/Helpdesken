namespace dhHelpdesk_NG.Domain.Changes
{
    using global::System;

    public class ChangeContactEntity : Entity
    {
        public int Change_Id { get; set; }

        public virtual ChangeEntity Change { get; set; }

        public string ContactName { get; set; }

        public string ContactPhone { get; set; }

        public string ContactEMail { get; set; }

        public string ContactCompany { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ChangedDate { get; set; }
    }
}
