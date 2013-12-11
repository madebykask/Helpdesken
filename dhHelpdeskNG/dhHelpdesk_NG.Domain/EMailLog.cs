using System;

namespace dhHelpdesk_NG.Domain
{
    public class EMailLog : Entity
    {
        public int CaseHistory_Id { get; set; }
        public int Log_Id { get; set; }
        public int MailID { get; set; }
        public string EMailAddress { get; set; }
        public string MessageId { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid EMailLogGUID { get; set; }

        public virtual CaseHistory CaseHistory { get; set; }
        public virtual Log Log { get; set; }
    }
}
