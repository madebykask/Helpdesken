using DH.Helpdesk.BusinessData.Models.Case.CaseHistory;
using DH.Helpdesk.Dal.MapperData.CaseHistory;

namespace DH.Helpdesk.Dal.Mappers.Cases.EntityToBusinessModel
{
    public class CaseHistoryToCaseHistoryOverviewMapper : IEntityToBusinessModelMapper<CaseHistoryMapperData, CaseHistoryOverview>
    {
        public CaseHistoryOverview Map(CaseHistoryMapperData data)
        {
            var entity = data.CaseHistory;

            return new CaseHistoryOverview
            {
                Id = entity.Id,
                CaseHistoryGUID = entity.CaseHistoryGUID,
                Case_Id = entity.Case_Id,
                ReportedBy = entity.ReportedBy,
                PersonsName = entity.PersonsName,
                PersonsEmail = entity.PersonsEmail,
                PersonsPhone = entity.PersonsPhone,
                PersonsCellphone = entity.PersonsCellphone,
                Customer_Id = entity.Customer_Id,
                Region_Id = entity.Region_Id,
                Department_Id = entity.Department_Id,
                OU_Id = entity.OU_Id,
                Place = entity.Place,
                UserCode = entity.UserCode,
                InventoryNumber = entity.InventoryNumber,
                InventoryType = entity.InventoryType,
                InventoryLocation = entity.InventoryLocation,
                CaseNumber = entity.CaseNumber,
                User_Id = entity.User_Id,
                IpAddress = entity.IpAddress,
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
                AgreedDate = entity.AgreedDate,
                Available = entity.Available,
                Cost = entity.Cost,
                OtherCost = entity.OtherCost,
                Currency = entity.Currency,
                Performer_User_Id = entity.Performer_User_Id,
                CaseResponsibleUser_Id = entity.CaseResponsibleUser_Id,
                Priority_Id = entity.Priority_Id,
                Status_Id = entity.Status_Id,
                StateSecondary_Id = entity.StateSecondary_Id,
                ExternalTime = entity.ExternalTime,
                Project_Id = entity.Project_Id,
                Verified = entity.Verified,
                VerifiedDescription = entity.VerifiedDescription,
                SolutionRate = entity.SolutionRate,
                PlanDate = entity.PlanDate,
                ApprovedDate = entity.ApprovedDate,
                ApprovedBy_User_Id = entity.ApprovedBy_User_Id,
                WatchDate = entity.WatchDate,
                LockCaseToWorkingGroup_Id = entity.LockCaseToWorkingGroup_Id,
                WorkingGroup_Id = entity.WorkingGroup_Id,
                FinishingDate = entity.FinishingDate,
                FinishingDescription = entity.FinishingDescription,
                FollowUpDate = entity.FollowUpDate,
                RegistrationSource = entity.RegistrationSource,
                RelatedCaseNumber = entity.RelatedCaseNumber,
                Problem_Id = entity.Problem_Id,
                Change_Id = entity.Change_Id,
                Unread = entity.Unread,
                RegLanguage_Id = entity.RegLanguage_Id,
                RegUserId = entity.RegUserId,
                RegUserDomain = entity.RegUserDomain,
                ProductAreaQuestionVersion_Id = entity.ProductAreaQuestionVersion_Id,
                LeadTime = entity.LeadTime,
                CreatedDate = entity.CreatedDate,
                CreatedByUser = entity.CreatedByUser,
                Deleted = entity.Deleted,
                CausingPartId = entity.CausingPartId,
                DefaultOwnerWG_Id = entity.DefaultOwnerWG_Id,
                CaseFile = entity.CaseFile,
                LogFile = entity.LogFile,
                CaseLog = entity.CaseLog,
                ClosingReason = entity.ClosingReason,
                RegistrationSourceCustomer_Id = entity.RegistrationSourceCustomer_Id,
                IsAbout_Persons_Name = entity.IsAbout_Persons_Name,
                IsAbout_ReportedBy = entity.IsAbout_ReportedBy,
                IsAbout_Persons_Phone = entity.IsAbout_Persons_Phone,
                IsAbout_UserCode = entity.IsAbout_UserCode,
                IsAbout_Department_Id = entity.IsAbout_Department_Id,
                CreatedByApp = entity.CreatedByApp,
                LatestSLACountDate = entity.LatestSLACountDate,
                ActionLeadTime = entity.ActionLeadTime,
                ActionExternalTime = entity.ActionExternalTime,
                CaseExtraFollowers = entity.CaseExtraFollowers,

                Department = data.Department,

                RegistrationSourceCustomer = data.RegistrationSourceCustomer,

                CaseType = data.CaseType,

                ProductArea = data.ProductArea,

                Category = data.Category,

                Problem = data.Problem,

                Project = data.Project,

                UserPerformer = data.UserPerformer,

                UserResponsible = data.UserResponsible,

                Priority = data.Priority,

                WorkingGroup = data.WorkingGroup,

                StateSecondary = data.StateSecondary,

                Status = data.Status,

                IsAbout_Department = data.IsAbout_Department,

                Emaillogs = data.EmailLogs
            };
        }
    }
}