using System;

namespace DH.Helpdesk.Models.Case
{
    public class CaseEditInputModel
    {
        public int? CaseId { get; set; }
        public string CaseGuid { get; set; }
        public int? ResponsibleUserId { get; set; }
        public int? PerformerId { get; set; }
        public int? WorkingGroupId { get; set; }
        public int? StateSecondaryId { get; set; }
        public int? PriorityId { get; set; }
        public int? ProductAreaId { get; set; }
        public DateTime? WatchDate { get; set; }
        public string LogInternalText { get; set; }
        public string LogExternalText { get; set; }
        public string Caption { get; set; }
        public string ReferenceNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string Available { get; set; }
        public int Cost { get; set; }
    }
}
