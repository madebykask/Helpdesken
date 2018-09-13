using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Models.MailTemplates;

namespace DH.Helpdesk.Models.Case
{
    public class CaseEditOutputModel
    {
        public int Id { get; set; }
        public string BackUrl { get; set; }
        //public bool CanGetRelatedCases { get; set; }
        public DateTime ChangeTime { get; set; }
        public int CustomerId { get; set; }
        public DateTime? AgreedDate { get; set; }
        public int? ApprovedByUserId { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string Available { get; set; }
        public string Caption { get; set; }
        public int? CaseCleanUpId { get; set; }
        public Guid CaseGuid { get; set; }
        public decimal CaseNumber { get; set; }
        public int? CaseResponsibleUserId { get; set; }
        public string ReportedBy { get; set; }
        public string PersonsName { get; set; }
        public string PersonsEmail { get; set; }
        public string PersonsPhone { get; set; }
        public string PersonsCellphone { get; set; }
        public int? RegionId { get; set; }
        public int? DepartmentId { get; set; }
        public int? OuId { get; set; }
        public string CostCentre { get; set; }
        public string Place { get; set; }
        public string UserCode { get; set; }
        public string InventoryNumber { get; set; }
        public string InventoryType { get; set; }
        public string InventoryLocation { get; set; }
        public int? UserId { get; set; }
        public string IpAddress { get; set; }
        public int CaseTypeId { get; set; }
        public int? ProductAreaId { get; set; }
        public DateTime? ProductAreaSetDate { get; set; }
        public int? SystemId { get; set; }
        public int? UrgencyId { get; set; }
        public int? ImpactId { get; set; }
        public int? CategoryId { get; set; }
        public int? SupplierId { get; set; }
        public string InvoiceNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public string Description { get; set; }
        public string Miscellaneous { get; set; }
        public int ContactBeforeAction { get; set; }
        public int Sms { get; set; }
        public int Cost { get; set; }
        public int OtherCost { get; set; }
        public string Currency { get; set; }
        public int? PerformerUserId { get; set; }
        public int? PriorityId { get; set; }
        public int? StatusId { get; set; }
        public int? StateSecondaryId { get; set; }
        public int ExternalTime { get; set; }
        public int? ProjectId { get; set; }
        public int Verified { get; set; }
        public string VerifiedDescription { get; set; }
        public string SolutionRate { get; set; }
        public DateTime? PlanDate { get; set; }
        public DateTime? WatchDate { get; set; }
        public int? LockCaseToWorkingGroupId { get; set; }
        public int? WorkingGroupId { get; set; }
        public int? CaseSolutionId { get; set; }
        public int? CurrentCaseSolutionId { get; set; }
        public DateTime? FinishingDate { get; set; }
        public string FinishingDescription { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public int RegistrationSource { get; set; }
        public int? RegistrationSourceCustomerId { get; set; }
        public int RelatedCaseNumber { get; set; }
        public int? ProblemId { get; set; }
        public int? ChangeId { get; set; }
        //public int Deleted { get; set; }
        //public int Unread { get; set; }
        public int RegLanguageId { get; set; }
        public string RegUserId { get; set; }
        public string RegUserName { get; set; }
        public string RegUserDomain { get; set; }
        public int? ProductAreaQuestionVersionId { get; set; }
        public int LeadTime { get; set; }
        public DateTime RegTime { get; set; }
        public int? ChangeByUserId { get; set; }
        public int? DefaultOwnerWgId { get; set; }
        public int? CausingPartId { get; set; }
        public DateTime? LatestSlaCountDate { get; set; }

        //public virtual CaseIsAboutEntity IsAbout { get; set; }//TODO


        //public List<CustomMailTemplate> MailTemplates { get; set; }
        //public List<CaseFile> CaseFiles { get; set; }
        //public List<CaseHistory> CaseHistories { get; set; }
        //public List<CaseInvoiceRow> CaseInvoiceRows { get; set; }
        //public List<CaseQuestionHeader> CaseQuestionHeaders { get; set; }
        //public List<InvoiceRow> InvoiceRows { get; set; }
        //public List<CaseExtraFollower> CaseFollowers { get; set; }
        //public virtual List<Log> Logs { get; set; }
        //public virtual List<Mail2Ticket> Mail2Tickets { get; set; }
        //public virtual ICollection<Case_ExtendedCaseEntity> CaseExtendedCaseDatas { get; set; }
        //public virtual ICollection<Case_CaseSection_ExtendedCase> CaseSectionExtendedCaseDatas { get; set; }

    }
}
