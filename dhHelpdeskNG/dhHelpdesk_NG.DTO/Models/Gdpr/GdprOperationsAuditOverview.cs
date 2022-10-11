using System;
using System.Collections.Generic;

namespace DH.Helpdesk.BusinessData.Models.Gdpr
{
    public class GdprOperationsAuditOverview
    {
        public int CustomerId { get; set; }

        public DateTime? RegDateFrom { get; set; }
        public DateTime? RegDateTo { get; set; } 
        public bool ClosedOnly { get; set; }
        public List<string> Fields { get; set; }
        public List<string> CaseTypes { get; set; }
        public bool RemoveCaseAttachments { get; set; }
        public bool RemoveLogAttachments { get; set; }
        
        public DateTime ExecutedDate { get; set; }
        public bool ReplaceEmails { get; set; }
    }
}