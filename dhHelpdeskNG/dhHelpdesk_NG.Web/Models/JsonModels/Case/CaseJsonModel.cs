using System;
using DH.Helpdesk.Web.Models.JsonModels.Base;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Common.Extensions.String;
using DH.Helpdesk.Common.Constants;

namespace DH.Helpdesk.Web.Models.JsonModels.Case
{
    public class CaseJsonModel : BaseJsonModel<CaseModel>
    {
        public CaseJsonModel()
        {

        }

        #region Base

        public int Id { get; set; }        

        /*Should be Converted to Guid*/
        public string CaseGuid { get; set; }

        /*Should be Converted to Decimal*/
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

        public int? FinishingType_Id { get; set; }

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

        public DateTime RegTime { get; set; }

        public DateTime ChangedTime { get; set; }


        #endregion

        #region Log

        public string Text_External { get; set; }
        public string Text_Internal { get; set; }

        #endregion

        #region Methods

        public override CaseModel ToBussinessModel()
        {
            return new CaseModel
            {
                #region Base

                Id = Id,
                CaseGUID = CaseGuid.IsValueChanged() ? new Guid(CaseGuid) : NotChangedValue.GUID,
                CaseNumber = CaseNumber,
                Customer_Id = Customer_Id,
                User_Id = User_Id,
                IpAddress = IpAddress,
                RegLanguage_Id = RegLanguage_Id,
                RegUserId = RegUserId,
                RegUserDomain = RegUserDomain,
                ChangeByUser_Id = ChangedByUser_Id,
                ExternalTime = ExternalTime,
                Deleted = Deleted,

                #endregion

                #region Initiator              

                ReportedBy = ReportedBy,
                PersonsName = PersonsName,
                PersonsEmail = PersonsEmail,
                NoMailToNotifier = NoMailToNotifier,
                PersonsPhone = PersonsPhone,
                PersonsCellphone = PersonsCellphone,
                CostCentre = CostCentre,
                Place = Place,
                UserCode = UserCode,
                UpdateNotifierInformation = UpdateNotifierInformation,
                Region_Id = Region_Id,
                Department_Id = Department_Id,
                OU_Id = OU_Id,

                #endregion

                #region IsAbout

                IsAbout_ReportedBy = IsAbout_ReportedBy,
                IsAbout_PersonsName = IsAbout_PersonsName,
                IsAbout_PersonsEmail = IsAbout_PersonsEmail,
                IsAbout_PersonsPhone = IsAbout_PersonsPhone,
                IsAbout_PersonsCellPhone = IsAbout_PersonsCellPhone,
                IsAbout_CostCentre = IsAbout_CostCentre,
                IsAbout_Place = IsAbout_Place,
                IsAbout_UserCode = IsAbout_UserCode,
                IsAbout_Region_Id = IsAbout_Region_Id,
                IsAbout_Department_Id = IsAbout_Department_Id,
                IsAbout_OU_Id = IsAbout_OU_Id,

                #endregion

                #region Computer Info

                InventoryNumber = InventoryNumber,
                InventoryType = InventoryType,
                InventoryLocation = InventoryLocation,

                #endregion

                #region Case Info

                RegistrationSource = RegistrationSource,
                CaseType_Id = CaseType_Id,
                ProductArea_Id = ProductArea_Id,
                ProductAreaSetDate = ProductAreaSetDate,
                ProductAreaQuestionVersion_Id = ProductAreaQuestionVersion_Id,
                System_Id = System_Id,
                Urgency_Id = Urgency_Id,
                Impact_Id = Impact_Id,
                Category_Id = Category_Id,
                Supplier_Id = Supplier_Id,
                InvoiceNumber = InvoiceNumber,
                ReferenceNumber = ReferenceNumber,
                Caption = Caption,
                Description = Description,
                Miscellaneous = Miscellaneous,
                ContactBeforeAction = ContactBeforeAction,
                SMS = SMS,
                Available = Available,
                Cost = Cost,
                OtherCost = OtherCost,
                Currency = Currency,

                #endregion

                #region Other Info

                WorkingGroup_Id = WorkingGroup_Id,
                Performer_User_Id = PerformerUser_Id,
                CaseResponsibleUser_Id = CaseResponsibleUser_Id,
                Priority_Id = Priority_Id,
                Status_Id = Status_Id,
                StateSecondary_Id = StateSecondary_Id,
                Project_Id = Project_Id,
                ProjectSchedule_Id = ProjectSchedule_Id,
                Problem_Id = Problem_Id,
                CausingPartId = CausingPart_Id,
                Change_Id = Change_Id,
                PlanDate = PlanDate,
                WatchDate = WatchDate,
                Verified = Verified,
                VerifiedDescription = VerifiedDescription,
                SolutionRate = SolutionRate,
                AgreedDate = AgreedDate,
                ApprovedDate = ApprovedDate,
                ApprovedBy_User_Id = ApprovedBy_User_Id,
                FinishingDate = FinishingDate,
                FinishingDescription = FinishingDescription,
                FinishingType_Id = FinishingType_Id,                
                RegistrationSourceCustomer_Id = RegistrationSourceCustomer_Id,
                LockCaseToWorkingGroup_Id = LockCaseToWorkingGroup_Id,
                FollowUpDate = FollowUpDate,
                RelatedCaseNumber = RelatedCaseNumber,
                LeadTime = LeadTime,
                CaseCleanUp_Id = CaseCleanUp_Id,
                DefaultOwnerWG_Id = DefaultOwnerWG_Id,
                RegUserName = RegUserName,
                Moved = Moved,
                LatestSLACountDate = LatestSLACountDate,
                RegTime = RegTime,
                ChangeTime = ChangedTime,
                #endregion

                #region Log

                Text_External = Text_External,
                Text_Internal = Text_Internal

                #endregion
            };
        }

        #endregion
    }

    public static class CaseJsonHelper
    {

        public static CaseJsonModel ToJsonModel(this CaseModel model)
        {
            return new CaseJsonModel
            {
                #region Base

                Id = model.Id,
                CaseGuid = model.CaseGUID.ToString(),
                CaseNumber = (int) model.CaseNumber,
                Customer_Id = model.Customer_Id,
                User_Id = model.User_Id,
                IpAddress = model.IpAddress,
                RegLanguage_Id = model.RegLanguage_Id,
                RegUserId = model.RegUserId,
                RegUserDomain = model.RegUserDomain,
                ChangedByUser_Id = model.ChangeByUser_Id,
                ExternalTime = model.ExternalTime,
                Deleted = model.Deleted,

                #endregion

                #region Initiator              

                ReportedBy = model.ReportedBy,
                PersonsName = model.PersonsName,
                PersonsEmail = model.PersonsEmail,
                NoMailToNotifier = model.NoMailToNotifier,
                PersonsPhone = model.PersonsPhone,
                PersonsCellphone = model.PersonsCellphone,
                CostCentre = model.CostCentre,
                Place = model.Place,
                UserCode = model.UserCode,
                UpdateNotifierInformation = model.UpdateNotifierInformation,
                Region_Id = model.Region_Id,
                Department_Id = model.Department_Id,
                OU_Id = model.OU_Id,

                #endregion

                #region IsAbout

                IsAbout_ReportedBy = model.IsAbout_ReportedBy,
                IsAbout_PersonsName = model.IsAbout_PersonsName,
                IsAbout_PersonsEmail = model.IsAbout_PersonsEmail,
                IsAbout_PersonsPhone = model.IsAbout_PersonsPhone,
                IsAbout_PersonsCellPhone = model.IsAbout_PersonsCellPhone,
                IsAbout_CostCentre = model.IsAbout_CostCentre,
                IsAbout_Place = model.IsAbout_Place,
                IsAbout_UserCode = model.IsAbout_UserCode,
                IsAbout_Region_Id = model.IsAbout_Region_Id,
                IsAbout_Department_Id = model.IsAbout_Department_Id,
                IsAbout_OU_Id = model.IsAbout_OU_Id,

                #endregion

                #region Computer Info

                InventoryNumber = model.InventoryNumber,
                InventoryType = model.InventoryType,
                InventoryLocation = model.InventoryLocation,

                #endregion

                #region Case Info

                RegistrationSource = model.RegistrationSource,
                CaseType_Id = model.CaseType_Id,
                ProductArea_Id = model.ProductArea_Id,
                ProductAreaSetDate = model.ProductAreaSetDate,
                ProductAreaQuestionVersion_Id = model.ProductAreaQuestionVersion_Id,
                System_Id = model.System_Id,
                Urgency_Id = model.Urgency_Id,
                Impact_Id = model.Impact_Id,
                Category_Id = model.Category_Id,
                Supplier_Id = model.Supplier_Id,
                InvoiceNumber = model.InvoiceNumber,
                ReferenceNumber = model.ReferenceNumber,
                Caption = model.Caption,
                Description = model.Description,
                Miscellaneous = model.Miscellaneous,
                ContactBeforeAction = model.ContactBeforeAction,
                SMS = model.SMS,
                Available = model.Available,
                Cost = model.Cost,
                OtherCost = model.OtherCost,
                Currency = model.Currency,

                #endregion

                #region Other Info

                WorkingGroup_Id = model.WorkingGroup_Id,
                PerformerUser_Id = model.Performer_User_Id,
                CaseResponsibleUser_Id = model.CaseResponsibleUser_Id,
                Priority_Id = model.Priority_Id,
                Status_Id = model.Status_Id,
                StateSecondary_Id = model.StateSecondary_Id,
                Project_Id = model.Project_Id,
                ProjectSchedule_Id = model.ProjectSchedule_Id,
                Problem_Id = model.Problem_Id,
                CausingPart_Id = model.CausingPartId,
                Change_Id = model.Change_Id,
                PlanDate = model.PlanDate,
                WatchDate = model.WatchDate,
                Verified = model.Verified,
                VerifiedDescription = model.VerifiedDescription,
                SolutionRate = model.SolutionRate,
                AgreedDate = model.AgreedDate,
                ApprovedDate = model.ApprovedDate,
                ApprovedBy_User_Id = model.ApprovedBy_User_Id,
                FinishingDate = model.FinishingDate,
                FinishingDescription = model.FinishingDescription,
                FinishingType_Id  = model.FinishingType_Id,                
                RegistrationSourceCustomer_Id = model.RegistrationSourceCustomer_Id,
                LockCaseToWorkingGroup_Id = model.LockCaseToWorkingGroup_Id,
                FollowUpDate = model.FollowUpDate,
                RelatedCaseNumber = model.RelatedCaseNumber,
                LeadTime = model.LeadTime,
                CaseCleanUp_Id = model.CaseCleanUp_Id,
                DefaultOwnerWG_Id = model.DefaultOwnerWG_Id,
                RegUserName = model.RegUserName,
                Moved = model.Moved,
                LatestSLACountDate = model.LatestSLACountDate,
                RegTime = model.RegTime,
                ChangedTime = model.ChangeTime,
                #endregion

                #region Log

                Text_External = model.Text_External,
                Text_Internal = model.Text_Internal

                #endregion
    };
        }
    }
}