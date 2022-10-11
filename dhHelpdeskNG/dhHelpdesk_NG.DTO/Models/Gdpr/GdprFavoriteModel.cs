using DH.Helpdesk.BusinessData.Enums.BusinessRules;
using System;
using System.Collections.Generic;

namespace DH.Helpdesk.BusinessData.Models.Gdpr
{
    public class GdprFavoriteModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CustomerId { get; set; }
        public int RetentionPeriod { get; set; }
        public bool CalculateRegistrationDate { get; set; }
        public DateTime RegisterDateFrom { get; set; }
        public DateTime RegisterDateTo { get; set; }
        public bool ClosedOnly { get; set; }
        public List<string> FieldsNames { get; set; }
        public bool ReplaceEmails { get; set; }
        public string ReplaceDataWith { get; set; }
        public DateTime? ReplaceDatesWith { get; set; }
        public bool RemoveCaseAttachments { get; set; }
        public bool RemoveLogAttachments { get; set; }
        public bool RemoveFileViewLogs { get; set; }
        public List<string> CaseTypes { get; set; }
        public int GDPRType { get; set; }
    }
}