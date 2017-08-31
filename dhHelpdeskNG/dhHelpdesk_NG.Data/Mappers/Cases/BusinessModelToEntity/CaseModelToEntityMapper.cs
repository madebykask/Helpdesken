namespace DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity
{
    using BusinessData.Models.Case;   
    using Domain;

    public sealed class CaseModelToEntityMapper : IBusinessModelToEntityMapper<CaseModel, Case>
    {
        public void Map(CaseModel businessModel, Case entity)
        {
            if (entity == null)
            {
                return;
            }

            #region Base

            entity.Id = businessModel.Id;
            entity.CaseNumber = businessModel.CaseNumber;
            entity.CaseGUID = businessModel.CaseGUID;
            entity.Customer_Id = businessModel.Customer_Id;
            entity.User_Id = businessModel.User_Id;
            entity.IpAddress = businessModel.IpAddress;
            entity.RegLanguage_Id = businessModel.RegLanguage_Id;
            entity.RegUserId = businessModel.RegUserId;
            entity.RegUserDomain = businessModel.RegUserDomain;
            entity.ChangeByUser_Id = businessModel.ChangeByUser_Id;
            entity.ExternalTime = businessModel.ExternalTime;
            entity.Deleted = businessModel.Deleted;             

            #endregion

            #region Initiator              

            entity.ReportedBy = businessModel.ReportedBy;
            entity.PersonsName = businessModel.PersonsName;
            entity.PersonsEmail = businessModel.PersonsEmail;

            //NoMailToNotifier = entity.ma

            entity.PersonsPhone = businessModel.PersonsPhone;
            entity.PersonsCellphone = businessModel.PersonsCellphone;
            entity.CostCentre = businessModel.CostCentre;
            entity.Place = businessModel.Place;
            entity.UserCode = businessModel.UserCode;
            //UpdateNotifierInformation = entity.

            entity.Region_Id = businessModel.Region_Id;
            entity.Department_Id = businessModel.Department_Id;
            entity.OU_Id = businessModel.OU_Id;

            #endregion                

            #region Computer Info

            entity.InventoryNumber = businessModel.InventoryNumber;
            entity.InventoryType = businessModel.InventoryType;
            entity.InventoryLocation = businessModel.InventoryLocation;

            #endregion

            #region Case Info

            entity.RegistrationSource = businessModel.RegistrationSource.Value;
            entity.CaseType_Id = businessModel.CaseType_Id;
            entity.ProductArea_Id = businessModel.ProductArea_Id;
            entity.ProductAreaSetDate = businessModel.ProductAreaSetDate;
            entity.ProductAreaQuestionVersion_Id = businessModel.ProductAreaQuestionVersion_Id;
            entity.System_Id = businessModel.System_Id;
            entity.Urgency_Id = businessModel.Urgency_Id;
            entity.Impact_Id = businessModel.Impact_Id;
            entity.Category_Id = businessModel.Category_Id;
            entity.Supplier_Id = businessModel.Supplier_Id;
            entity.InvoiceNumber = businessModel.InvoiceNumber;
            entity.ReferenceNumber = businessModel.ReferenceNumber;
            entity.Caption = businessModel.Caption;
            entity.Description = businessModel.Description;
            entity.Miscellaneous = businessModel.Miscellaneous;
            entity.ContactBeforeAction = businessModel.ContactBeforeAction;
            entity.SMS = businessModel.SMS;
            entity.Available = businessModel.Available;
            entity.Cost = businessModel.Cost;
            entity.OtherCost = businessModel.OtherCost;
            entity.Currency = businessModel.Currency;
            entity.ApprovedBy_User_Id = businessModel.ApprovedBy_User_Id;
            
            #endregion

            #region Other Info

            entity.WorkingGroup_Id = businessModel.WorkingGroup_Id;
            entity.Performer_User_Id = businessModel.Performer_User_Id;
            entity.CaseResponsibleUser_Id = businessModel.CaseResponsibleUser_Id;
            entity.Priority_Id = businessModel.Priority_Id;
            entity.Status_Id = businessModel.Status_Id;
            entity.StateSecondary_Id = businessModel.StateSecondary_Id;
            entity.Project_Id = businessModel.Project_Id;
            entity.Problem_Id = businessModel.Problem_Id;
            entity.CausingPartId = businessModel.CausingPartId;
            entity.Change_Id = businessModel.Change_Id;
            entity.PlanDate = businessModel.PlanDate;
            entity.WatchDate = businessModel.WatchDate;
            entity.Verified = businessModel.Verified.Value;
            entity.VerifiedDescription = businessModel.VerifiedDescription;
            entity.SolutionRate = businessModel.SolutionRate;
            entity.AgreedDate = businessModel.AgreedDate;
            entity.ApprovedDate = businessModel.ApprovedDate;
            entity.FinishingDate = businessModel.FinishingDate;
            entity.FinishingDescription = businessModel.FinishingDescription;        
            entity.RegistrationSourceCustomer_Id = businessModel.RegistrationSourceCustomer_Id;
            entity.LockCaseToWorkingGroup_Id = businessModel.LockCaseToWorkingGroup_Id;
            entity.FollowUpDate = businessModel.FollowUpDate;
            entity.RelatedCaseNumber = businessModel.RelatedCaseNumber.Value;
            entity.LeadTime = businessModel.LeadTime.Value;       
            entity.CaseCleanUp_Id = businessModel.CaseCleanUp_Id;
            entity.DefaultOwnerWG_Id = businessModel.DefaultOwnerWG_Id;
            entity.RegUserName = businessModel.RegUserName;            
            entity.LatestSLACountDate = businessModel.LatestSLACountDate;
            //entity.Moved = businessModel.Moved.Value;
            entity.RegTime = businessModel.RegTime;
            entity.ChangeTime = businessModel.ChangeTime;
            entity.CaseSolution_Id = businessModel.CaseSolution_Id;

            #endregion
        }
    }
}