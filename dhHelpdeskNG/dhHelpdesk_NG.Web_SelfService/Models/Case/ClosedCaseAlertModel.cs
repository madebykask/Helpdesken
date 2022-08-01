using DH.Helpdesk.BusinessData.Models.Case.MergedCase;
using DH.Helpdesk.Domain;
using System;

namespace DH.Helpdesk.SelfService.Models.Case
{
    public class ClosedCaseAlertModel
    {
        public DateTime? FinishingDate { get; set; }
        public int CaseComplaintDays { get; set; }

        public string FinishingCause { get; set; }

        public bool IsMerged { get; set; }

        public MergedParentInfo MergedParentInfo { get; set; }
    }
}