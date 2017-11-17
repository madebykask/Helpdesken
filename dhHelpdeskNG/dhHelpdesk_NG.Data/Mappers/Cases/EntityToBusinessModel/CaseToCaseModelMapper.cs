namespace DH.Helpdesk.Dal.Mappers.Cases.EntityToBusinessModel
{
    using BusinessData.Models.Case;    
    using Domain;
    using System.Collections.Generic;

    public sealed class CaseToCaseModelMapper : IEntityToBusinessModelMapper<Case, CaseModel>
    {
        public CaseModel Map(Case entity)
        {
            if (entity == null)
                return null;

            var ret = new CaseModel
            {
                #region Base

                Id = entity.Id,
                CaseNumber = entity.CaseNumber,

                CaseGUID = entity.CaseGUID,

                Customer_Id = entity.Customer_Id,

                User_Id = entity.User_Id,

                IpAddress = entity.IpAddress,

                RegLanguage_Id = entity.RegLanguage_Id,

                RegUserId = entity.RegUserId,

                RegUserDomain = entity.RegUserDomain,

                ChangeByUser_Id = entity.ChangeByUser_Id,

                ExternalTime = entity.ExternalTime,

                Deleted = entity.Deleted,

                #endregion

                #region Initiator              

                ReportedBy = entity.ReportedBy,

                PersonsName = entity.PersonsName,

                PersonsEmail = entity.PersonsEmail,

                //NoMailToNotifier = entity.ma

                PersonsPhone = entity.PersonsPhone,

                PersonsCellphone = entity.PersonsCellphone,

                CostCentre = entity.CostCentre,

                Place = entity.Place,

                UserCode = entity.UserCode,

                //UpdateNotifierInformation = entity.

                Region_Id = entity.Region_Id,

                Department_Id = entity.Department_Id,

                OU_Id = entity.OU_Id,

                #endregion

                #region IsAbout

                IsAbout_ReportedBy = entity.IsAbout?.ReportedBy,

                IsAbout_PersonsName = entity.IsAbout?.Person_Name,

                IsAbout_PersonsEmail = entity.IsAbout?.Person_Email,

                IsAbout_PersonsPhone = entity.IsAbout?.Person_Phone,

                IsAbout_PersonsCellPhone = entity.IsAbout?.Person_Cellphone,

                IsAbout_CostCentre = entity.IsAbout?.CostCentre,

                IsAbout_Place = entity.IsAbout?.Place,

                IsAbout_UserCode = entity.IsAbout?.UserCode,

                IsAbout_Region_Id = entity.IsAbout?.Region_Id,

                IsAbout_Department_Id = entity.IsAbout?.Department_Id,

                IsAbout_OU_Id = entity.IsAbout?.OU_Id,

                #endregion

                #region Computer Info

                InventoryNumber = entity.InventoryNumber,

                InventoryType = entity.InventoryType,

                InventoryLocation = entity.InventoryLocation,

                #endregion

                #region Case Info

                RegistrationSource = entity.RegistrationSource,

                CaseType_Id = entity.CaseType_Id,

                ProductArea_Id = entity.ProductArea_Id,

                ProductAreaSetDate = entity.ProductAreaSetDate,

                System_Id = entity.System_Id,

                Urgency_Id = entity.Urgency_Id,

                Impact_Id = entity.Impact_Id,

                Category_Id = entity.Category_Id,

                Supplier_Id = entity.Supplier_Id,

                InvoiceNumber = entity.InvoiceNumber,

                ReferenceNumber = entity.ReferenceNumber,

                Caption = entity.Caption,

                Description = entity.Description,

                Miscellaneous = entity.Miscellaneous,

                ContactBeforeAction = entity.ContactBeforeAction,

                SMS = entity.SMS,

                Available = entity.Available,

                Cost = entity.Cost,

                OtherCost = entity.OtherCost,

                Currency = entity.Currency,

                #endregion

                #region Other Info

                WorkingGroup_Id = entity.WorkingGroup_Id,

                Performer_User_Id = entity.Performer_User_Id,

                CaseResponsibleUser_Id = entity.CaseResponsibleUser_Id,

                Priority_Id = entity.Priority_Id,

                Status_Id = entity.Status_Id,

                StateSecondary_Id = entity.StateSecondary_Id,

                Project_Id = entity.Project_Id,

                Problem_Id = entity.Problem_Id,

                CausingPartId = entity.CausingPartId,

                Change_Id = entity.Change_Id,

                PlanDate = entity.PlanDate,

                WatchDate = entity.WatchDate,

                Verified = entity.Verified,

                VerifiedDescription = entity.VerifiedDescription,

                SolutionRate = entity.SolutionRate,

                AgreedDate = entity.AgreedDate,

                ApprovedDate = entity.ApprovedDate,

                FinishingDate = entity.FinishingDate,

                FinishingDescription = entity.FinishingDescription,                

                RegistrationSourceCustomer_Id = entity.RegistrationSourceCustomer_Id,

                ApprovedBy_User_Id = entity.ApprovedBy_User_Id,

                LockCaseToWorkingGroup_Id = entity.LockCaseToWorkingGroup_Id,

                FollowUpDate = entity.FollowUpDate,

                RelatedCaseNumber = entity.RelatedCaseNumber,

                LeadTime = entity.LeadTime,

                CaseCleanUp_Id = entity.CaseCleanUp_Id,

                DefaultOwnerWG_Id = entity.DefaultOwnerWG_Id,

                RegUserName = entity.RegUserName,

                LatestSLACountDate = entity.LatestSLACountDate,

                RegTime = entity.RegTime,

                ChangeTime = entity.ChangeTime,

                #endregion

                #region Etc

                CaseSolution_Id = entity.CaseSolution_Id,

                #endregion

                #region casefiles
                CaseFiles = entity.CaseFiles
                #endregion

                
            };

            return ret;
        }
    }
}
