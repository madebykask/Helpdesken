using System;
using System.Collections.Generic;

namespace dhHelpdesk_NG.Domain
{
    public class Case : Entity
    {
        public decimal CaseNumber { get; set; }
        public int ContactBeforeAction { get; set; }
        public int Cost { get; set; }
        public int Customer_Id { get; set; }
        public int Deleted { get; set; }
        public int? Department_Id { get; set; }
        public int? Region_Id { get; set; }
        public int? OU_Id { get; set; }
        public int? ProductArea_Id { get; set; }
        public int? System_Id { get; set; }
        public int? Urgency_Id { get; set; }
        public int? Impact_Id { get; set; }
        public int? Category_Id { get; set; }
        public int? Problem_Id { get; set; }
        public int? Project_Id { get; set; }
        public int? Change_Id { get; set; }
        public int? Supplier_Id { get; set; }
        public int? StateSecondary_Id { get; set; }
        public int ExternalTime { get; set; }
        public int MOSS_DocVersion { get; set; }
        public int OtherCost { get; set; }
        public int RegistrationSource { get; set; }
        public int? RegLanguage_Id { get; set; }
        public int RelatedCaseNumber { get; set; }
        public int? WorkingGroup_Id { get; set; }
        public int? Status_Id { get; set; }
        public int? CaseResponsibleUser_Id { get; set; }
        public int SMS { get; set; }
        public int Verified { get; set; }
        public int User_Id { get; set; }
        public string Available { get; set; }
        public string Caption { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public string FinishingDescription { get; set; }
        public string InventoryLocation { get; set; }
        public string InventoryNumber { get; set; }
        public string InventoryType { get; set; }
        public string InvoiceNumber { get; set; }
        public string IpAddress { get; set; }
        public string Miscellaneous { get; set; }
        public string MOSS_DocUrl { get; set; }
        public string MOSS_DocUrlText { get; set; }
        public string PersonsCellphone { get; set; }
        public string PersonsEmail { get; set; }
        public string PersonsName { get; set; }
        public string PersonsPhone { get; set; }
        public string Place { get; set; }
        public string ReferenceNumber { get; set; }
        public string RegUserDomain { get; set; }
        public string RegUserId { get; set; }
        public string ReportedBy { get; set; }
        public string SolutionRate { get; set; }
        public string UserCode { get; set; }
        public string VerifiedDescription { get; set; }
        public DateTime? AgreedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime ChangeTime { get; set; }
        public DateTime? FinishingDate { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public DateTime? PlanDate { get; set; }
        public DateTime? ProductAreaSetDate { get; set; }
        public DateTime RegTime { get; set; }
        public DateTime? WatchDate { get; set; }
        public Guid CaseGUID { get; set; }
        public Guid? MOSS_DocId { get; set; }
        public int CaseType_Id { get; set; }
        public int Performer_User_Id { get; set; }
        public int? Priority_Id { get; set; }
        public int ApprovedBy_User_Id { get; set; }

        public virtual Category Category { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Department Department { get; set; }
        public virtual Language RegLanguage { get; set; }
        public virtual Urgency Urgency { get; set; }
        public virtual ICollection<CaseFile> CaseFiles { get; set; }
        public virtual ICollection<CaseHistory> CaseHistories { get; set; }
        public virtual ICollection<CaseInvoiceRow> CaseInvoiceRows { get; set; }
        public virtual ICollection<CaseQuestionHeader> CaseQuestionHeaders { get; set; }
    }
}
