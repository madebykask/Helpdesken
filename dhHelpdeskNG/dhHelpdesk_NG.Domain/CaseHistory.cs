using System;

namespace dhHelpdesk_NG.Domain
{
    public class CaseHistory : Entity
    {
        public decimal CaseNumber { get; set; }
        public int Case_Id { get; set; }
        public int ContactBeforeAction { get; set; }
        public int Cost { get; set; }
        public int Deleted { get; set; }
        public int ExternalTime { get; set; }
        public int MOSS_DocVersion { get; set; }
        public int OtherCost { get; set; }
        public int RegistrationSource { get; set; }
        public int RelatedCaseNumber { get; set; }
        public int SMS { get; set; }
        public int Verified { get; set; }
        public string Available { get; set; }
        public string Caption { get; set; }
        public string Currency { get; set; }
        public string FinishingDescription { get; set; }
        public string Description { get; set; }
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
        public DateTime CreatedDate { get; set; }
        public DateTime? FinishingDate { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public DateTime? PlanDate { get; set; }
        public DateTime? ProductAreaSetDate { get; set; }
        public DateTime? WatchDate { get; set; }
        public Guid CaseHistoryGUID { get; set; }
        public Guid? MOSS_DocId { get; set; }

        /* ??????????????????
         * 
         * public User User { get; set; }
         * public User PerformerUser { get; set; }
         * public User ResponsibleUser { get; set; }
         * public User ApprovedByUser { get; set; }
         * public User ChangedByUser { get; set; }
         * 
         * public WorkingGroup CaseLookedToWorkingGroup { get; set; }
         * public WorkingGroup WorkingGroup { get; set; }
         * 
         * public Case RelatedCase { get; set; }
         * 
         */

        public virtual Case Case { get; set; }
        public virtual Category Category { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Department Department { get; set; }
        public virtual Language RegLanguage { get; set; }
        public virtual Status Status { get; set; }
        public virtual Urgency Urgency { get; set; }
    }
}
