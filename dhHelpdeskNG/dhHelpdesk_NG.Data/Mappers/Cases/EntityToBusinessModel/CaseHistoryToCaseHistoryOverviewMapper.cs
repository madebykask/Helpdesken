using System.Linq;
using DH.Helpdesk.BusinessData.Models.Accounts;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseHistory;
using DH.Helpdesk.BusinessData.Models.Case.Output;
using DH.Helpdesk.BusinessData.Models.Changes.Output;
using DH.Helpdesk.BusinessData.Models.Logs.Output;
using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
using DH.Helpdesk.Common.Tools;
using DH.Helpdesk.Dal.MapperData.CaseHistory;
using DH.Helpdesk.Domain;
using EmailLogsOverview = DH.Helpdesk.BusinessData.Models.Case.CaseHistory.EmailLogsOverview;

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

                Department = data.Department != null
                    ? new DepartmentOverview
                    {
                        DepartmentId = data.Department.DepartmentId,
                        DepartmentName = data.Department.DepartmentName,
                        SearchKey = data.Department.SearchKey,
                        CountryName = data.Department.Country != null ? data.Department.Country.Name : null
                    }
                    : null,

                RegistrationSourceCustomer =
                    data.RegistrationSourceCustomer != null
                        ? new RegistrationSourceCustomerOverview
                        {
                            Id = data.RegistrationSourceCustomer.Id,
                            SourceName = data.RegistrationSourceCustomer.SourceName
                        }
                        : null,

                CaseType = new CaseTypeOverview
                {
                    Id = data.CaseType.Id,
                    Name = data.CaseType.Name
                },

                ProductArea = data.ProductArea != null
                    ? new ProductAreaOverview
                    {
                        Id = data.ProductArea.Id,
                        Name = data.ProductArea.Name
                    }
                    : null,

                Category = data.Category != null
                    ? new CategoryOverview
                    {

                        Id = data.Category.Id,
                        Name = data.Category.Name,
                    }
                    : null,

                UserPerformer = data.UserPerformer != null
                    ? new UserBasicOvierview
                    {
                        Id = data.UserPerformer.Id ?? 0,
                        FirstName = data.UserPerformer.FirstName,
                        SurName = data.UserPerformer.SurName
                    }
                    : null,


                Priority = data.Priority != null
                    ? new PriorityOverview
                    {
                        Id = data.Priority.Id,
                        Name = data.Priority.Name
                    }
                    : null,

                WorkingGroup = data.WorkingGroup != null
                    ? new WorkingGroupOverview
                    {
                        Id = data.WorkingGroup.Id,
                        WorkingGroupName = data.WorkingGroup.WorkingGroupName
                    }
                    : null,

                StateSecondary = data.StateSecondary != null
                    ? new StateSecondaryOverview
                    {
                        Id = data.StateSecondary.Id,
                        Name = data.StateSecondary.Name
                    }
                    : null,

                Status = data.Status != null
                    ? new StatusOverview
                    {
                        Id = data.Status.Id,
                        Name = data.Status.Name
                    }
                    : null,

                IsAbout_Department = data.IsAbout_Department != null
                    ? new DepartmentOverview
                    {
                        DepartmentId = data.IsAbout_Department.DepartmentId,
                        DepartmentName = data.IsAbout_Department.DepartmentName,
                        SearchKey = data.IsAbout_Department.SearchKey,
                        CountryName = data.IsAbout_Department.Country?.Name
                    }
                    : null,

                Emaillogs = data.EmailLogs.Where(t => t.Id.HasValue)
                                          .Select(t => new EmailLogsOverview(t.Id.Value, t.MailId.Value, t.EmailAddress)).ToList()
            };
        }
    }
}