namespace DH.Helpdesk.BusinessData.Models.Case.CaseOverview
{
    using System;

    public sealed class CaseInfoOverview
    {
        public CaseInfoOverview(
                string caseNumber, 
                DateTime? registrationDate, 
                DateTime changeDate, 
                string registratedBy, 
                string caseType, 
                string productArea, 
                string system, 
                string urgentDegree, 
                string impact, 
                string category, 
                string supplier, 
                string invoiceNumber, 
                string referenceNumber, 
                string caption, 
                string description, 
                string other, 
                string phoneContact, 
                string sms, 
                DateTime? agreedDate, 
                string available, 
                int cost, 
                string attachedFile)
        {
            this.AttachedFile = attachedFile;
            this.Cost = cost;
            this.Available = available;
            this.AgreedDate = agreedDate;
            this.Sms = sms;
            this.PhoneContact = phoneContact;
            this.Other = other;
            this.Description = description;
            this.Caption = caption;
            this.ReferenceNumber = referenceNumber;
            this.InvoiceNumber = invoiceNumber;
            this.Supplier = supplier;
            this.Category = category;
            this.Impact = impact;
            this.UrgentDegree = urgentDegree;
            this.System = system;
            this.ProductArea = productArea;
            this.CaseType = caseType;
            this.RegistratedBy = registratedBy;
            this.ChangeDate = changeDate;
            this.RegistrationDate = registrationDate;
            this.Case = caseNumber;
        }

        public string Case { get; private set; }

        public DateTime? RegistrationDate { get; private set; }

        public DateTime ChangeDate { get; private set; }

        public string RegistratedBy { get; private set; }

        public string CaseType { get; private set; }

        public string ProductArea { get; private set; }

        public string System { get; private set; }

        public string UrgentDegree { get; private set; }

        public string Impact { get; private set; }

        public string Category { get; private set; }

        public string Supplier { get; private set; }

        public string InvoiceNumber { get; private set; }

        public string ReferenceNumber { get; private set; }

        public string Caption { get; private set; }

        public string Description { get; private set; }

        public string Other { get; private set; }

        public string PhoneContact { get; private set; }

        public string Sms { get; private set; }

        public DateTime? AgreedDate { get; private set; }

        public string Available { get; private set; }

        public int Cost { get; private set; }

        public string AttachedFile { get; private set; }
    }
}