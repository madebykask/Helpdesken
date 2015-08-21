namespace DH.Helpdesk.BusinessData.Models.Case.CaseSettingsOverview
{
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class CaseInfoSettings
    {
        public CaseInfoSettings(
            FieldOverviewSetting caseNumber, 
            FieldOverviewSetting registrationDate, 
            FieldOverviewSetting changeDate, 
            FieldOverviewSetting registratedBy, 
            FieldOverviewSetting caseType, 
            FieldOverviewSetting productArea, 
            FieldOverviewSetting system, 
            FieldOverviewSetting urgentDegree, 
            FieldOverviewSetting impact, 
            FieldOverviewSetting category, 
            FieldOverviewSetting supplier, 
            FieldOverviewSetting invoiceNumber, 
            FieldOverviewSetting referenceNumber, 
            FieldOverviewSetting caption, 
            FieldOverviewSetting description, 
            FieldOverviewSetting other, 
            FieldOverviewSetting phoneContact, 
            FieldOverviewSetting sms, 
            FieldOverviewSetting agreedDate, 
            FieldOverviewSetting available, 
            FieldOverviewSetting cost, 
            FieldOverviewSetting attachedFile,
            FieldOverviewSetting leadTime)
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
            this.LeadTime = leadTime;
        }

        [NotNull]
        public FieldOverviewSetting Case { get; private set; }

        [NotNull]
        public FieldOverviewSetting RegistrationDate { get; private set; }

        [NotNull]
        public FieldOverviewSetting ChangeDate { get; private set; }

        [NotNull]
        public FieldOverviewSetting RegistratedBy { get; private set; }

        [NotNull]
        public FieldOverviewSetting CaseType { get; private set; }

        [NotNull]
        public FieldOverviewSetting ProductArea { get; private set; }

        [NotNull]
        public FieldOverviewSetting System { get; private set; }

        [NotNull]
        public FieldOverviewSetting UrgentDegree { get; private set; }

        [NotNull]
        public FieldOverviewSetting Impact { get; private set; }

        [NotNull]
        public FieldOverviewSetting Category { get; private set; }

        [NotNull]
        public FieldOverviewSetting Supplier { get; private set; }

        [NotNull]
        public FieldOverviewSetting InvoiceNumber { get; private set; }

        [NotNull]
        public FieldOverviewSetting ReferenceNumber { get; private set; }

        [NotNull]
        public FieldOverviewSetting Caption { get; private set; }

        [NotNull]
        public FieldOverviewSetting Description { get; private set; }

        [NotNull]
        public FieldOverviewSetting Other { get; private set; }

        [NotNull]
        public FieldOverviewSetting PhoneContact { get; private set; }

        [NotNull]
        public FieldOverviewSetting Sms { get; private set; }

        [NotNull]
        public FieldOverviewSetting AgreedDate { get; private set; }

        [NotNull]
        public FieldOverviewSetting Available { get; private set; }

        [NotNull]
        public FieldOverviewSetting Cost { get; private set; }

        [NotNull]
        public FieldOverviewSetting AttachedFile { get; private set; }

        [NotNull]
        public FieldOverviewSetting LeadTime { get; private set; }
    }
}