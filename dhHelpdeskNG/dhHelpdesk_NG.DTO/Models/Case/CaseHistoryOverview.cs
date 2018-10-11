using System;
using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Case.CaseOverview;
using DH.Helpdesk.BusinessData.Models.Case.Output;
using DH.Helpdesk.BusinessData.Models.Logs.Output;
using DH.Helpdesk.BusinessData.Models.Problem.Output;
using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
using DH.Helpdesk.BusinessData.Models.Projects.Output;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.BusinessData.Models.Case.CaseHistory
{
    public class CaseHistoryOverview
    {
        public CaseHistoryOverview()
        {
            Emaillogs = new List<EmailLogsOverview>();
            CaseExtraFollowers = string.Empty;
        }

        public int Id { get; set; }
        public Guid CaseHistoryGUID { get; set; }
        public int Case_Id { get; set; }
        public string ReportedBy { get; set; }
        public string PersonsName { get; set; }
        public string PersonsEmail { get; set; }
        public string PersonsPhone { get; set; }
        public string PersonsCellphone { get; set; }
        public int Customer_Id { get; set; }
        public int? Region_Id { get; set; }
        public int? Department_Id { get; set; }
        public int? OU_Id { get; set; }
        public string Place { get; set; }
        public string UserCode { get; set; }
        public string InventoryNumber { get; set; }
        public string InventoryType { get; set; }
        public string InventoryLocation { get; set; }
        public Decimal CaseNumber { get; set; }
        public int? User_Id { get; set; }
        public string IpAddress { get; set; }
        public int CaseType_Id { get; set; }
        public int? ProductArea_Id { get; set; }
        public DateTime? ProductAreaSetDate { get; set; }
        public int? System_Id { get; set; }
        public int? Urgency_Id { get; set; }
        public int? Impact_Id { get; set; }
        public int? Category_Id { get; set; }
        public int? Supplier_Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string Miscellaneous { get; set; }
        public int ContactBeforeAction { get; set; }
        public int SMS { get; set; }
        public DateTime? AgreedDate { get; set; }
        public string Available { get; set; }
        public int Cost { get; set; }
        public int OtherCost { get; set; }
        public string Currency { get; set; }
        public int? Performer_User_Id { get; set; }
        public int? CaseResponsibleUser_Id { get; set; }
        public int? Priority_Id { get; set; }
        public int? Status_Id { get; set; }
        public int? StateSecondary_Id { get; set; }
        public int ExternalTime { get; set; }
        public int? Project_Id { get; set; }
        public int? Verified { get; set; }
        public string VerifiedDescription { get; set; }
        public string SolutionRate { get; set; }
        public DateTime? PlanDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int? ApprovedBy_User_Id { get; set; }
        public DateTime? WatchDate { get; set; }
        public int? LockCaseToWorkingGroup_Id { get; set; }
        public int? WorkingGroup_Id { get; set; }
        public DateTime? FinishingDate { get; set; }
        public string FinishingDescription { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public int RegistrationSource { get; set; }
        public int RelatedCaseNumber { get; set; }
        public int? Problem_Id { get; set; }
        public int? Change_Id { get; set; }
        public int Unread { get; set; }
        public int RegLanguage_Id { get; set; }
        public string RegUserId { get; set; }
        public string RegUserDomain { get; set; }
        public int? ProductAreaQuestionVersion_Id { get; set; }
        public int LeadTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByUser { get; set; }
        public int Deleted { get; set; }
        public int? CausingPartId { get; set; }
        public int? DefaultOwnerWG_Id { get; set; }
        public string CaseFile { get; set; }
        public string LogFile { get; set; }
        public string CaseLog { get; set; }
        public string ClosingReason { get; set; }
        public int? RegistrationSourceCustomer_Id { get; set; }
        public string IsAbout_Persons_Name { get; set; }
        public string IsAbout_ReportedBy { get; set; }
        public string IsAbout_Persons_EMail { get; set; }
        public string IsAbout_Persons_Phone { get; set; }
        public string IsAbout_Persons_CellPhone { get; set; }
        public string IsAbout_UserCode { get; set; }
        public int? IsAbout_Department_Id { get; set; }
        public int? IsAbout_Region_Id { get; set; }
        public int? IsAbout_OU_Id { get; set; }
        public string CreatedByApp { get; set; }
        public DateTime? LatestSLACountDate { get; set; }
        public int ActionLeadTime { get; set; }
        public int ActionExternalTime { get; set; }
        public string CaseExtraFollowers { get; set; }
        public string CostCentre { get; set; }
        public string IsAbout_CostCentre { get; set; }
        public string IsAbout_Place { get; set; }

        public CategoryOverview Category { get; set; }
        public DepartmentOverview Department { get; set; }
        public RegistrationSourceCustomerOverview RegistrationSourceCustomer { get; set; }
        public CaseTypeOverview  CaseType { get; set; }
        public ProductAreaOverview ProductArea { get; set; }
        public ProblemOverview Problem { get; set; }
        public ProjectOverview Project { get; set; }
        public CausingPartOverview  CausingPart { get; set; }
        public UserBasicOvierview UserPerformer { get; set; }
        public UserBasicOvierview UserResponsible { get; set; }
        public PriorityOverview Priority { get; set; }
        public WorkingGroupOverview WorkingGroup { get; set; }
        public RegionOverview Region { get; set; }
        public OUOverview OU { get; set; }
        public StateSecondaryOverview StateSecondary { get; set; }
        public StatusOverview Status { get; set; }
        public DepartmentOverview IsAbout_Department { get; set; }
        public RegionOverview IsAbout_Region { get; set; }
        public OUOverview IsAbout_OU { get; set; }

        public IList<EmailLogsOverview> Emaillogs { get; set; }
    }
}