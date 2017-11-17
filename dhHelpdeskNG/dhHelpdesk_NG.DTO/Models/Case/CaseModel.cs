using DH.Helpdesk.BusinessData.Models.Shared.Input;
using System;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class CaseModel: INewBusinessModel
    {
        #region Base

        public int Id { get; set; }

        public decimal CaseNumber { get; set; }

        public Guid CaseGUID { get; set; }

        public int Customer_Id { get; set; }

        public int? User_Id { get; set; }

        public string IpAddress { get; set; }

        public int RegLanguage_Id { get; set; }

        public string RegUserId { get; set; }

        public string RegUserDomain {get; set;}

        public int? ChangeByUser_Id { get; set; }

        public int ExternalTime { get; set; } 

        public int Deleted { get; set; } 


        #endregion

        #region Initiator              

        public string ReportedBy { get; set; }

        public string PersonsName { get; set; }

        public string PersonsEmail { get; set; }

        public int NoMailToNotifier { get; set; }

        public string PersonsPhone { get; set; }

        public string PersonsCellphone { get; set; }

        public string CostCentre { get; set; }

        public string Place { get; set; }

        public string UserCode { get; set; }

        public int UpdateNotifierInformation { get; set; }

        public int? Region_Id { get; set; }

        public int? Department_Id { get; set; }

        public int? OU_Id { get; set; }

        #endregion

        #region IsAbout

        public string IsAbout_ReportedBy { get; set; }

        public string IsAbout_PersonsName { get; set; }

        public string IsAbout_PersonsEmail { get; set; }

        public string IsAbout_PersonsPhone { get; set; }

        public string IsAbout_PersonsCellPhone { get; set; }

        public string IsAbout_CostCentre { get; set; }

        public string IsAbout_Place { get; set; }

        public string IsAbout_UserCode { get; set; }

        public int? IsAbout_Region_Id { get; set; }

        public int? IsAbout_Department_Id { get; set; }

        public int? IsAbout_OU_Id { get; set; }

        #endregion

        #region Computer Info

        public string InventoryNumber { get; set; }

        public string InventoryType { get; set; }

        public string InventoryLocation { get; set; }

        #endregion

        #region Case Info

        public int? RegistrationSource { get; set; }

        public int CaseType_Id { get; set; }

        public int? ProductArea_Id { get; set; }

        public DateTime? ProductAreaSetDate { get; set; }

        public int? ProductAreaQuestionVersion_Id { get; set; } 

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

        public string Available { get; set; }

        public int Cost { get; set; }

        public int OtherCost { get; set; }

        public string Currency { get; set; }

        #endregion

        #region Other Info

        public int? WorkingGroup_Id { get; set; }

        public int? Performer_User_Id { get; set; }

        public int? CaseResponsibleUser_Id { get; set; }

        public int? Priority_Id { get; set; }

        public int? Status_Id { get; set; }

        public int? StateSecondary_Id { get; set; }

        public int? Project_Id { get; set; }

        public int? ProjectSchedule_Id { get; set; }

        public int? Problem_Id { get; set; }

        public int? CausingPartId { get; set; }

        public int? Change_Id { get; set; }

        public DateTime? PlanDate { get; set; }

        public DateTime? WatchDate { get; set; }

        public int?  Verified { get; set; }

        public string VerifiedDescription { get; set; }

        public string SolutionRate { get; set; }

        public DateTime? AgreedDate { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public int? ApprovedBy_User_Id { get; set; } 

        public DateTime? FinishingDate { get; set; }

        public string FinishingDescription { get; set; }

        public int? FinishingType_Id { get; set; }

        //public int? Status { get; set; }

        public int? RegistrationSourceCustomer_Id { get; set; }

        public int? LockCaseToWorkingGroup_Id { get; set; }

        public DateTime? FollowUpDate { get; set; }

        public int? RelatedCaseNumber { get; set; } 

        public int? LeadTime { get; set; } 

        public int? CaseCleanUp_Id { get; set; } 

        public int? DefaultOwnerWG_Id { get; set; } 

        public string RegUserName { get; set; }

        public int Moved { get; set; } 

        public DateTime? LatestSLACountDate { get; set; }

        public DateTime RegTime { get; set; }

        public DateTime ChangeTime { get; set; }

        #endregion

        #region Log
        public string Text_External { get; set; }

        public string Text_Internal { get; set; }

        #endregion

        #region Etc

        public int? CaseSolution_Id { get; set; }
        #endregion

        #region ExtendedCase
        public int? ExtendedCaseData_Id { get; set; }
        public int? ExtendedCaseForm_Id { get; set; }
        #endregion

    }
}
