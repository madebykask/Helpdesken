namespace DH.Helpdesk.Domain
{
    using DH.Helpdesk.Domain.Cases;
    using DH.Helpdesk.Domain.Interfaces;
    using DH.Helpdesk.Domain.Problems;
    using ExtendedCaseEntity;
    using global::System;
    using global::System.Collections.Generic;

    public class Case : Entity, ICustomerEntity
    {
	    public Case()
	    {
		    InvoiceRows = new List<InvoiceRow>();
            //CaseExtendedCaseDatas = new ICollection<Case_ExtendedCaseEntity>();
        }

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
        public String CostCentre { get; set; }
        public String Place { get; set; }
        public String UserCode { get; set; }
        public String InventoryNumber { get; set; }
        public String InventoryType { get; set; }
        public String InventoryLocation { get; set; }
        public Decimal CaseNumber { get; set; }
        public int? User_Id { get; set; }
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
        public int? Performer_User_Id { get; set; }
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
        public int? ApprovedBy_User_Id { get; set; }
        public DateTime? WatchDate { get; set; }
        public int? LockCaseToWorkingGroup_Id { get; set; }
        public int? WorkingGroup_Id { get; set; }
        public int? CaseSolution_Id { get; set; }
		public int? CurrentCaseSolution_Id { get; set; }


		/// <summary>
		/// In UTC
		/// </summary>
		public DateTime? FinishingDate { get; set; }
        public String FinishingDescription { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public int RegistrationSource { get; set; }

        /// <summary>
        /// Id of extended case source for specific customer
        /// </summary>
        public int? RegistrationSourceCustomer_Id { get; set; }

        /// <summary>
        /// Exteded case source
        /// </summary>
        public virtual RegistrationSourceCustomer RegistrationSourceCustomer { get; set; }
        public virtual CaseIsAboutEntity IsAbout { get; set; }

        public int RelatedCaseNumber { get; set; }
        public int? Problem_Id { get; set; }
        public int? Change_Id { get; set; }
        public int Deleted { get; set; }
        public int Unread { get; set; }
        public int RegLanguage_Id { get; set; }
        public String RegUserId { get; set; }
        public String RegUserName { get; set; }
        public String RegUserDomain { get; set; }
        public int? ProductAreaQuestionVersion_Id { get; set; }
        public int LeadTime { get; set; }
        public int? CaseCleanUp_Id { get; set; }
        public DateTime RegTime { get; set; }

        /// <summary>
        /// Warning! when change this value, usually, ExternalTime field should be updated also
        /// </summary>
        public DateTime ChangeTime { get; set; }
        public int? ChangeByUser_Id { get; set; }
        public int? DefaultOwnerWG_Id { get; set; }

        /// <summary>
        /// Gets or sets the causing type id.
        /// </summary>
        public int? CausingPartId { get; set; }

        public DateTime? LatestSLACountDate { get; set; }

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
		public virtual ICollection<InvoiceRow> InvoiceRows { get; set; }
		public virtual ICollection<CaseExtraFollower> CaseFollowers { get; set; }

		public virtual User User { get; set; }

        public virtual Region Region { get; set; }

        public virtual CausingPart CausingPart { get; set; }

        public virtual OU Ou { get; set; }

        public virtual System System { get; set; }

        public virtual Impact Impact { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual User CaseResponsibleUser { get; set; }

        public virtual Status Status { get; set; }

        public virtual IList<Log> Logs { get; set; }

        public virtual ICollection<Mail2Ticket> Mail2Tickets { get; set; }

        public virtual CaseSolution CaseSolution { get; set; }

		public virtual CaseSolution CurrentCaseSolution { get; set; }

		public virtual ICollection<Case_ExtendedCaseEntity> CaseExtendedCaseDatas { get; set; }

        public bool IsClosed()
        {
            return this.FinishingDate != null;
        }
    }
}
