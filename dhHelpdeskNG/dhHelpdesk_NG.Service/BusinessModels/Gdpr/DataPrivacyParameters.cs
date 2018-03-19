using System;
using System.Collections.Generic;

namespace DH.Helpdesk.Services.BusinessLogic.Gdpr
{
    public class DataPrivacyParameters
    {
        public int SelectedCustomerId { get; set; }
        public int RetentionPeriod { get; set; }
        public DateTime? RegisterDateFrom { get; set; }
        public DateTime? RegisterDateTo { get; set; }
        public bool ClosedOnly { get; set; }
        public List<string> FieldsNames { get; set; }
        public string ReplaceDataWith { get; set; }
        public DateTime? ReplaceDatesWith { get; set; }
        public bool RemoveCaseAttachments { get; set; }
        public bool RemoveLogAttachments { get; set; }
        public bool RemoveCaseHistory { get; set; }
    }
}