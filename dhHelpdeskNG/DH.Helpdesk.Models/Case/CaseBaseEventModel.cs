using System;
using DH.Helpdesk.BusinessData.Models.Logs.Output;

namespace DH.Helpdesk.Models.Case
{
    public class CaseBaseEventModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public CaseEventType Type { get; set; }
        public LogUserOverview CreatedBy { get; set; }
    }

    public enum CaseEventType
    {
        Unknown = 0,
        ExternalLogNote = 1,
        InternalLogNote = 2,
        ClosedCase = 3,
        AssignedAdministrator = 4,
        AssignedWorkingGroup = 5,
        ChangedSubStatus = 6,
        ChangedWatchDate = 7,
        ChangedPriority = 8,
        UpploadLogFile = 9,
        SentEmails = 10,
        Other = 11
    }
}
