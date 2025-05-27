// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomerSettingsToBusinessModelMapper.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CustomerSettingsToBusinessModelMapper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Mappers.Customer.EntityToBusinessModel
{
    using DH.Helpdesk.BusinessData.Models.Customer;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The customer settings to business model mapper.
    /// </summary>
    public class CustomerSettingsToBusinessModelMapper : IEntityToBusinessModelMapper<Setting, CustomerSettings>
    {
        /// <summary>
        /// The map.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="CustomerSettings"/>.
        /// </returns>
        public CustomerSettings Map(Setting entity)
        {
            if (entity == null)
            {
                return null;
            }
            
            return new CustomerSettings
                       {
                           CustomerId = entity.Customer_Id,
                           ModuleAccount = entity.ModuleAccount.ToBool(),
                           ModuleAdSync = entity.ModuleADSync.ToBool(),
                           ModuleAsset = entity.ModuleAsset.ToBool(),   
                           ModuleBulletinBoard = entity.ModuleBulletinBoard.ToBool(),
                           ModuleCalendar = entity.ModuleCalendar.ToBool(),
                           ModuleCase = entity.ModuleCase.ToBool(),
                           ModuleChangeManagement = entity.ModuleChangeManagement.ToBool(),
                           ModuleChecklist = entity.ModuleChecklist.ToBool(),
                           ModuleComputerUser = entity.ModuleComputerUser.ToBool(),
                           ModuleContract = entity.ModuleContract.ToBool(),
                           ModuleDailyReport = entity.ModuleDailyReport.ToBool(),
                           ModuleDocument = entity.ModuleDocument.ToBool(),
                           ModuleFaq = entity.ModuleFAQ.ToBool(),
                           ModuleInventory = entity.ModuleInventory.ToBool(),
                           ModuleInventoryImport = entity.ModuleInventoryImport.ToBool(),
                           ModuleInvoice = entity.ModuleInvoice.ToBool(),
                           ModuleLicense = entity.ModuleLicense.ToBool(),
                           ModuleOperationLog = entity.ModuleOperationLog.ToBool(),
                           ModuleOrder = entity.ModuleOrder.ToBool(),
                           ModulePlanning = entity.ModulePlanning.ToBool(),
                           ModuleProblem = entity.ModuleProblem.ToBool(),
                           ModuleProject = entity.ModuleProject.ToBool(),
                           ModuleQuestion = entity.ModuleQuestion.ToBool(),
                           ModuleQuestionnaire = entity.ModuleQuestionnaire.ToBool(),
                           ModuleTimeRegistration = entity.ModuleTimeRegistration.ToBool(),
                           ModuleWatch = entity.ModuleWatch.ToBool(),
                           ModuleCaseInvoice = entity.ModuleCaseInvoice.ToBool(),
                           ShowCaseOverviewInfo = entity.ShowCaseOverviewInfo.ToBool(),
                           ShowStatusPanel = entity.ShowStatusPanel.ToBool(),
                           CreateCaseFromOrder = entity.CreateCaseFromOrder.ToBool(),
                           CreateComputerFromOrder = entity.CreateComputerFromOrder.ToBool(),
                           BulletinBoardWGRestriction = entity.BulletinBoardWGRestriction.ToBool(),
                           CalendarWGRestriction = entity.CalendarWGRestriction.ToBool(),
                           QuickLinkWGRestriction = entity.QuickLinkWGRestriction,
                           ModuleExtendedCase = entity.ModuleExtendedCase.ToBool(),
                           AttachmentPlacement = entity.AttachmentPlacement,
                           M2TNewCaseMailTo = entity.M2TNewCaseMailTo,
                           DontConnectUserToWorkingGroup = entity.DontConnectUserToWorkingGroup,
                           PhysicalFilePath = entity.PhysicalFilePath,
                           VirtualFilePath = entity.VirtualFilePath,
                           IsUserFirstLastNameRepresentation = entity.IsUserFirstLastNameRepresentation,
                           ComputerUserSearchRestriction = entity.ComputerUserSearchRestriction,
                           DepartmentFilterFormat = entity.DepartmentFilterFormat,
                           DefaultCaseTemplateId = entity.DefaultCaseTemplateId,
                           SetUserToAdministrator = entity.SetUserToAdministrator.ToBool(),
                           DefaultAdministratorId = entity.DefaultAdministrator,
                           TimeZoneOffset = entity.TimeZone_offset,
                           DisableCaseEndDate = entity.DisableCaseEndDate.ToBool(),
                           BlockedEmailRecipients = entity.BlockedEmailRecipients,
                           GraphTenantId = entity.GraphTenantId,
                           GraphClientId = entity.GraphClientId,
                           GraphClientSecret = entity.GraphClientSecret,
                           GraphUserName = entity.GraphUserName,
                           ErrorMailTo = entity.ErrorMailTo,
                           UseGraphSendingEmail = entity.UseGraphSendingEmail
            };
        }
    }
}