using System;

namespace DH.Helpdesk.Domain.GDPR
{
    public class GDPRDataPrivacyFavorite : Entity
    {
        public string Name { get; set; }

        public int CustomerId { get; set; }
        public int RetentionPeriod { get; set; }
        public bool CalculateRegistrationDate { get; set; }
        public DateTime RegisterDateFrom { get; set; }
        public DateTime RegisterDateTo { get; set; }
        public bool ClosedOnly { get; set; }
        public string FieldsNames { get; set; }
        public bool ReplaceEmails { get; set; }
        public string ReplaceDataWith { get; set; }
        public DateTime? ReplaceDatesWith { get; set; }
        public bool RemoveCaseAttachments { get; set; }
        public bool RemoveLogAttachments { get; set; }
        public bool RemoveFileViewLogs { get; set; }

        public DateTime? CreatedDate { get; set; }
        public int? CreatedByUser_Id { get; set; }
        public DateTime? ChangedDate { get; set; }
        public int? ChangedByUser_Id  { get; set; }
        public int GDPRType { get; set; }
        public string CaseTypes { get; set; }

        public DateTime? FinishedDateFrom { get; set; }
        public DateTime? FinishedDateTo { get; set; }

        public string ProductAreas { get; set; }
    }
}