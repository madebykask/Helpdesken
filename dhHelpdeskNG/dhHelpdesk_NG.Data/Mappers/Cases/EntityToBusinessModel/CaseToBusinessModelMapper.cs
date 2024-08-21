namespace DH.Helpdesk.Dal.Mappers.Cases.EntityToBusinessModel
{
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.Domain;

    public sealed class CaseToBusinessModelMapper : IEntityToBusinessModelMapper<Case, CaseOverview>
    {
        public CaseOverview Map(Case entity)
        {
            return new CaseOverview
                       {
                           AgreedDate = entity.AgreedDate,
                           Available = entity.Available,
                           Caption = entity.Caption,
                           CaseHistories = entity.CaseHistories,
                           CaseNumber = entity.CaseNumber,
                           CaseResponsibleUserId = entity.CaseResponsibleUser_Id,
                           CaseTypeId = entity.CaseType_Id,
                           CategoryId = entity.Category_Id,
                           CausingTypeId = entity.CausingPartId,
                           ChangeId = entity.Change_Id,
                           ContactBeforeAction = entity.ContactBeforeAction,
                           Cost = entity.Cost,
                           Currency = entity.Currency,
                           CustomerId = entity.Customer_Id,
                           Deleted = entity.Deleted,
                           Department = entity.Department,
                           Description = entity.Description,
                           FinishingDate = entity.FinishingDate,
                           FinishingDescription = entity.FinishingDescription,
                           ImpactId = entity.Impact_Id,
                           InventoryLocation = entity.InventoryLocation,
                           InventoryNumber = entity.InventoryNumber,
                           IpAddress = entity.IpAddress,
                           InventoryType = entity.InventoryType,
                           InvoiceNumber = entity.InvoiceNumber,
                           Miscellaneous = entity.Miscellaneous,
                           OtherCost = entity.OtherCost,
                           OuId = entity.OU_Id,
                           PerformerUserId = entity.Performer_User_Id,
                           PersonsCellphone = entity.PersonsCellphone,
                           PersonsEmail = entity.PersonsEmail,
                           PersonsPhone = entity.PersonsPhone,
                           PersonsName = entity.PersonsName,
                           Place = entity.Place,
                           Priority = entity.Priority,
                           ProblemId = entity.Problem_Id,
                           ProductAreaId = entity.ProductArea_Id,
                           ProjectId = entity.Project_Id,
                           ReferenceNumber = entity.ReferenceNumber,
                           Region = entity.Region,
                           RegionId = entity.Region_Id,
                           RegistrationDate = entity.RegTime,
                           ReportedBy = entity.ReportedBy,
                           Sms = entity.SMS,
                           SolutionRate = entity.SolutionRate,
                           StateSecondary = entity.StateSecondary,
                           StateSecondaryId = entity.StateSecondary_Id,
                           StatusId = entity.Status_Id,
                           SupplierId = entity.Supplier_Id,
                           SystemId = entity.System_Id,
                           Urgency = entity.Urgency,
                           UserCode = entity.UserCode,
                           UserId = entity.User_Id,
                           Verified = entity.Verified,
                           VerifiedDescription = entity.VerifiedDescription,
                           WatchDate = entity.WatchDate,
                           WorkingGroup = entity.Workinggroup,
                           WorkingGroupId = entity.WorkingGroup_Id,
                           MovedFromCustomer_Id = entity.MovedFromCustomer_Id,
            };
        }
    }
}