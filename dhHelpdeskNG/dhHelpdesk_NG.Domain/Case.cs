namespace DH.Helpdesk.Domain
{
    using DH.Helpdesk.Domain.Cases;
    using DH.Helpdesk.Domain.Problems;

    using global::System;
    using global::System.Collections.Generic;

    public class Case : Entity
    {
        public Guid CaseGUID { get; set; }
        public String ReportedBy { get; set; }
        public String PersonsName { get; set; }
        public String PersonsEmail { get; set; }
        public String PersonsPhone { get; set; }
        public String PersonsCellphone { get; set; }
        public int Customer_Id { get; set; }
        public int? Region_Id { get; set; }
        public int? Department_Id { get; set; }
        public int? OU_Id { get; set; }
        public String Place { get; set; }
        public String UserCode { get; set; }
        public String InventoryNumber { get; set; }
        public String InventoryType { get; set; }
        public String InventoryLocation { get; set; }
        public Decimal CaseNumber { get; set; }
        public int User_Id { get; set; }
        public String IpAddress { get; set; }
        public int CaseType_Id { get; set; }
        public int? ProductArea_Id { get; set; }
        public DateTime? ProductAreaSetDate { get; set; }
        public int? System_Id { get; set; }
        public int? Urgency_Id { get; set; }
        public int? Impact_Id { get; set; }
        public int? Category_Id { get; set; }
        public int? Supplier_Id { get; set; }
        public String InvoiceNumber { get; set; }
        public String ReferenceNumber { get; set; }
        public String Caption { get; set; }
        public String Description { get; set; }
        public String Miscellaneous { get; set; }
        public int ContactBeforeAction { get; set; }
        public int SMS { get; set; }
        public DateTime? AgreedDate { get; set; }
        public String Available { get; set; }
        public int Cost { get; set; }
        public int OtherCost { get; set; }
        public String Currency { get; set; }
        public int Performer_User_Id { get; set; }
        public int? CaseResponsibleUser_Id { get; set; }
        public int? Priority_Id { get; set; }
        public int? Status_Id { get; set; }
        public int? StateSecondary_Id { get; set; }
        public int ExternalTime { get; set; }
        public int? Project_Id { get; set; }
        public int Verified { get; set; }
        public String VerifiedDescription { get; set; }
        public String SolutionRate { get; set; }
        public DateTime? PlanDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int ApprovedBy_User_Id { get; set; }
        public DateTime? WatchDate { get; set; }
        public int? LockCaseToWorkingGroup_Id { get; set; }
        public int? WorkingGroup_Id { get; set; }
        public DateTime? FinishingDate { get; set; }
        public String FinishingDescription { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public int RegistrationSource { get; set; }
        public int RelatedCaseNumber { get; set; }
        public int? Problem_Id { get; set; }
        public int? Change_Id { get; set; }
        public int Deleted { get; set; }
        public int Unread { get; set; }
        public int RegLanguage_Id { get; set; }
        public String RegUserId { get; set; }
        public String RegUserDomain { get; set; }
        public int? ProductAreaQuestionVersion_Id { get; set; }
        public int LeadTime { get; set; }
        public int? CaseCleanUp_Id { get; set; }
        public DateTime RegTime { get; set; }
        public DateTime ChangeTime { get; set; }
        public int? ChangeByUser_Id { get; set; }
        public int? DefaultOwnerWG_Id { get; set; }

        /// <summary>
        /// Gets or sets the causing type id.
        /// </summary>
        public int? CausingPartId { get; set; }

        public virtual ProductArea ProductArea { get; set; }
        public virtual User LastChangedByUser { get; set; }
        public virtual User Administrator { get; set; }
        public virtual CaseType CaseType { get; set; }
        public virtual WorkingGroupEntity Workinggroup { get; set; }
        public virtual Category Category { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Department Department { get; set; }
        public virtual Language RegLanguage { get; set; }
        public virtual Urgency Urgency { get; set; }
        public virtual Problem Problem { get; set; }
        public virtual Priority Priority { get; set; }
        public virtual StateSecondary StateSecondary { get; set; }
        public virtual ICollection<CaseFile> CaseFiles { get; set; }
        public virtual ICollection<CaseHistory> CaseHistories { get; set; }
        public virtual ICollection<CaseInvoiceRow> CaseInvoiceRows { get; set; }
        public virtual ICollection<CaseQuestionHeader> CaseQuestionHeaders { get; set; }

        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        public virtual Region Region { get; set; }

        /// <summary>
        /// Gets or sets the causing type.
        /// </summary>
        public virtual CausingPart CausingPart { get; set; }
    }
}
