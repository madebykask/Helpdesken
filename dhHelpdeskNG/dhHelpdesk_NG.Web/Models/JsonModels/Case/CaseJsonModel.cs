using System;
using DH.Helpdesk.Web.Models.JsonModels.Base;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Common.Constants;

namespace DH.Helpdesk.Web.Models.JsonModels.Case
{
    public class CaseJsonModelTest
    {
        public CaseJsonModelTest()
        {

        }
        public int Id { get; set; }
    }
    public class CaseJsonModel: BaseJsonModel<CaseModel>
    {
        public CaseJsonModel()
        {
          
        }

        #region Base

        public int Id { get; set; }

        /*Should be Converted to Guid*/
        public string CaseGuid { get; set; }

        public int CaseNumber { get; set; }        

        public int Customer_Id { get; set; }

        public int? User_Id { get; set; }

        public string IpAddress { get; set; }

        public int RegLanguage_Id { get; set; }

        public string RegUserId { get; set; }

        public string RegUserDomain { get; set; }

        public int? ChangedByUser_Id { get; set; }

        public int ExternalTime { get; set; }

        public int Deleted { get; set; }


        #endregion

        #region Initiator              

        public string ReportedBy { get; set; }

        public string PersonsName { get; set; }

        public string PersonsEmail { get; set; }

        public int NoMailToNotifier { get; set; }

        public string PersonsPhone { get; set; }

        public string PersonsCellPhone { get; set; }

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

        public int? CaseType_Id { get; set; }

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

        public int? PerformerUser_Id { get; set; }

        public int? CaseResponsibleUser_Id { get; set; }

        public int? Priority_Id { get; set; }

        public int? Status_Id { get; set; }

        public int? StateSecondary_Id { get; set; }

        public int? Project_Id { get; set; }

        public int? ProjectSchedule_Id { get; set; }

        public int? Problem_Id { get; set; }

        public int? CausingPart_Id { get; set; }

        public int? Change_Id { get; set; }

        public DateTime? PlanDate { get; set; }

        public DateTime? WatchDate { get; set; }

        public int? Verified { get; set; }

        public string VerifiedDescription { get; set; }

        public string SolutionRate { get; set; }

        public DateTime? AgreedDate { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public int? ApprovedBy_User_Id { get; set; }

        public DateTime? FinishingDate { get; set; }

        public string FinishingDescription { get; set; }

        public int? Status { get; set; }

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

        #endregion


        #region Methods

        public override CaseModel ToBussinessModel()
        {
            throw new NotImplementedException();
        }
                
        #endregion
    }

    public static class CaseJsonHelper
    {
       
        public static CaseJsonModel ToJsonModel(this CaseModel model) 
        {            
            return new CaseJsonModel
            {
                Id = model.Id,
                Caption = model.Caption,
                Customer_Id = model.Customer_Id.Value
            };

        }
    }
}