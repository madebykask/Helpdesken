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
    }
}