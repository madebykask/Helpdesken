using System;
using System.Collections.Generic; 

namespace dhHelpdesk_NG.Domain
{
    using dhHelpdesk_NG.Domain.Problems;

    public class CaseHistory : Entity
    {
        public Guid CaseHistoryGUID { get; set; }
        public int Case_Id { get; set; }
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
        public int? Verified { get; set; }
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
        public int Unread { get; set; }
        public int RegLanguage_Id { get; set; }
        public String RegUserId { get; set; }
        public String RegUserDomain { get; set; }
        public int? ProductAreaQuestionVersion_Id { get; set; }
        public int LeadTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public String CreatedByUser { get; set; }
        public int Deleted { get; set; }

        public virtual Case Case { get; set; }
        public virtual Category Category { get; set; }
        public virtual StateSecondary StateSecondary { get; set; }
        public virtual Priority Priority { get; set; }
        public virtual Department Department { get; set; }
        public virtual Problem Problem { get; set; }
        public virtual ICollection<EmailLog> Emaillogs { get; set; }
        //public virtual User UserPerformer { get; set; }
        //public virtual Status Status { get; set; }
        //public virtual Urgency Urgency { get; set; }
    }
}

