using System;

namespace DH.Helpdesk.BusinessData.Models.Mail2Tickets.Output
{
    public class Mail2TicketOverview
    {
        public int? CaseId { get; set; }
        public int? LogId { get; set; }
        public string EMailSubject { get; set; }
        public string EMailAddress { get; set; }
        public string Type { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}