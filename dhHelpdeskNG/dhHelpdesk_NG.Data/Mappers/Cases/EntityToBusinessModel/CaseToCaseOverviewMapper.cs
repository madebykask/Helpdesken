namespace DH.Helpdesk.Dal.Mappers.Cases.EntityToBusinessModel
{
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.Domain;

    public class CaseToCaseOverviewMapper
    {
        public static CaseOverview Map(Case entity)
        {
            return new CaseOverview
            {
                Id = entity.Id,
                CustomerId = entity.Customer_Id,
                Deleted = entity.Deleted,
                FinishingDate = entity.FinishingDate,
                StatusId = entity.Status_Id,
                UserId = entity.User_Id,
                CaseNumber = entity.CaseNumber,
                Department = entity.Department,
                PersonsCellphone = entity.PersonsCellphone,
                PersonsName = entity.PersonsName,
                PersonsPhone = entity.PersonsPhone,
                Region = entity.Region,
                RegionId = entity.Region_Id,
                RegistrationDate = entity.RegTime,
                ReportedBy = entity.ReportedBy,
                OuId = entity.OU_Id,
                Place = entity.Place,
                UserCode = entity.UserCode,
                PersonsEmail = entity.PersonsEmail,
                InventoryNumber = entity.InventoryNumber,
                InventoryLocation = entity.InventoryLocation,
                InventoryType = entity.InventoryType,
                IpAddress = entity.IpAddress,
                CaseTypeId = entity.CaseType_Id,
                SystemId = entity.System_Id,
                Urgency = entity.Urgency,
                ImpactId = entity.Impact_Id,
                SupplierId = entity.Supplier_Id,
                InvoiceNumber = entity.InvoiceNumber,
                ReferenceNumber = entity.ReferenceNumber,
                Caption = entity.Caption,
                Description = entity.Description,
                CategoryId = entity.Category_Id,
                Miscellaneous = entity.Miscellaneous,
                ProductAreaId = entity.ProductArea_Id,
                ContactBeforeAction = entity.ContactBeforeAction,
                Sms = entity.SMS,
                AgreedDate = entity.AgreedDate,
                Available = entity.Available,
                Cost = entity.Cost,
                OtherCost = entity.OtherCost,
                Currency = entity.Currency,
                WorkingGroupId = entity.WorkingGroup_Id,
                WorkingGroup = entity.Workinggroup,
                CaseResponsibleUserId = entity.CaseResponsibleUser_Id,
                PerformerUserId = entity.Performer_User_Id,
                Priority = entity.Priority,
                StateSecondaryId = entity.StateSecondary_Id,
                StateSecondary = entity.StateSecondary,
                ProjectId = entity.Project_Id,
                ProblemId = entity.Problem_Id,
                ChangeId = entity.Change_Id,
                WatchDate = entity.WatchDate,
                Verified = entity.Verified,
                VerifiedDescription = entity.VerifiedDescription,
                SolutionRate = entity.SolutionRate,
                CaseHistories = entity.CaseHistories,
                FinishingDescription = entity.FinishingDescription,
                CausingTypeId = entity.CausingPartId
            };
        }
    }
}
