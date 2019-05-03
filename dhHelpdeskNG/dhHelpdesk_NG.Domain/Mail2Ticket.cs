using System;

namespace DH.Helpdesk.Domain
{
    public class Mail2Ticket : Entity
    {
        public int? Case_Id { get; set; }
        public int? Log_Id { get; set; }
        public string EMailSubject { get; set; }
        public string EMailAddress { get; set; }
        public string Type { get; set; }
        public DateTime CreatedDate { get; set; }

        // navigation properties
        public virtual Case Case { get; set; }
    }
}
