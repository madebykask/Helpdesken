using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.BusinessData.Models.Case.CaseLock
{
    public class CaseLockInfo
    {
        public bool IsLocked { get; set; }

        public int CaseId { get; set; }

        public int UserId { get; set; }

        public string LockGuid { get; set; }

        public string BrowserSession { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime ExtendedTime { get; set; }

        public int ExtendValue { get; set; }

        public int TimerInterval { get; set; }

        public string UserFullName { get; set; }
    }
}
