namespace DH.Helpdesk.BusinessData.Models.Case
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class RelatedCase
    {
        public RelatedCase(
            int caseId, 
            decimal caseNumber, 
            DateTime registrationDate, 
            string status, 
            string caption, 
            string description)
        {
            this.Description = description;
            this.Caption = caption;
            this.Status = status;
            this.RegistrationDate = registrationDate;
            this.CaseNumber = caseNumber;
            this.CaseId = caseId;
        }

        [IsId]
        public int CaseId { get; private set; }

        public decimal CaseNumber { get; private set; }

        public DateTime RegistrationDate { get; private set; }

        public string Status { get; private set; }

        [NotNull]
        public string Caption { get; private set; }

        [NotNull]
        public string Description { get; private set; }
    }
}