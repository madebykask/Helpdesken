using System;

namespace DH.Helpdesk.Models.Case
{
    public class CaseLockModel
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