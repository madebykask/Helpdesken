using DH.Helpdesk.Domain;
using System.Collections.Generic;
using DH.Helpdesk.Dal.MapperData.CaseHistory;

namespace DH.Helpdesk.Dal.MapperData.Logs
{
    public class LogMapperData
    {
        public Log Log { get; set; }
        public UserMapperData User { get; set; }
        public IList<EmailLogMapperData> EmailLogs { get; set; }
        public IList<LogFileMapperData> LogFiles { get; set; }
        public IList<Mail2TicketMapperData> Mail2Tickets { get; set; }
    }
}
