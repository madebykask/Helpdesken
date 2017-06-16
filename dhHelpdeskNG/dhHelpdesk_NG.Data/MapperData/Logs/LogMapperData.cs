using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.BusinessData.Models.Changes;
using DH.Helpdesk.Dal.MapperData.CaseHistory;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.MapperData.Logs
{
    public class LogMapperData
    {
        public Log Log { get; set; }
        public UserMapperData User { get; set; }
        public IEnumerable<EmailLogMapperData> EmailLogs { get; set; }
        public IEnumerable<LogFileMapperData> LogFiles { get; set; }
    }
}
