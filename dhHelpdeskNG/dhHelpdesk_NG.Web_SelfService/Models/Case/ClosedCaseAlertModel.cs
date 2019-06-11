using System;

namespace DH.Helpdesk.SelfService.Models.Case
{
    public class ClosedCaseAlertModel
    {
        public DateTime? FinishingDate { get; set; }
        public int CaseComplaintDays { get; set; }
    }
}