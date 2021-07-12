using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer;
using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings;

namespace DH.Helpdesk.Services.Services.Concrete
{
    public interface IComputerCopyService
    {
        void ApplyCopySettings(ComputerForInsert businessModel, ComputerFieldsSettingsProcessing settings);
    }

    public class ComputerCopyService : IComputerCopyService
    {
        public void ApplyCopySettings(ComputerForInsert businessModel, ComputerFieldsSettingsProcessing settings)
        {
            if (!settings.ContactFieldsSettings.EmailFieldSetting.IsCopy)
                businessModel.ContactFields.Email = null;
            if (!settings.ContactFieldsSettings.NameFieldSetting.IsCopy)
                businessModel.ContactFields.Name = null;
            if (!settings.ContactFieldsSettings.PhoneFieldSetting.IsCopy)
                businessModel.ContactFields.Phone = null;

            if (!settings.ContactInformationFieldsSettings.UserIdFieldSetting.IsCopy)
                businessModel.ContactInformationFields.UserId = null;

            if (!settings.ChassisFieldsSettings.ChassisFieldSetting.IsCopy)
                businessModel.ChassisFields.Chassis = null;

            if (!settings.CommunicationFieldsSettings.IPAddressFieldSetting.IsCopy)
                businessModel.CommunicationFields.IPAddress = null;
            if (!settings.CommunicationFieldsSettings.MacAddressFieldSetting.IsCopy)
                businessModel.CommunicationFields.MacAddress = null;
            if (!settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting.IsCopy)
                businessModel.CommunicationFields.NetworkAdapterId = null;
            if (!settings.CommunicationFieldsSettings.NovellClientFieldSetting.IsCopy)
                businessModel.CommunicationFields.NovellClient = null;
            if (!settings.CommunicationFieldsSettings.RASFieldSetting.IsCopy)
                businessModel.CommunicationFields.IsRAS = false;

            if (!settings.ContractFieldsSettings.AccountingDimension1FieldSetting.IsCopy)
                businessModel.ContractFields.AccountingDimension1 = null;
            if (!settings.ContractFieldsSettings.AccountingDimension2FieldSetting.IsCopy)
                businessModel.ContractFields.AccountingDimension2 = null;
            if (!settings.ContractFieldsSettings.AccountingDimension3FieldSetting.IsCopy)
                businessModel.ContractFields.AccountingDimension3 = null;
            if (!settings.ContractFieldsSettings.AccountingDimension4FieldSetting.IsCopy)
                businessModel.ContractFields.AccountingDimension4 = null;
            if (!settings.ContractFieldsSettings.AccountingDimension5FieldSetting.IsCopy)
                businessModel.ContractFields.AccountingDimension5 = null;
            if (!settings.ContractFieldsSettings.ContractEndDateFieldSetting.IsCopy)
                businessModel.ContractFields.ContractEndDate = null;
            if (!settings.ContractFieldsSettings.ContractNumberFieldSetting.IsCopy)
                businessModel.ContractFields.ContractNumber = null;
            if (!settings.ContractFieldsSettings.ContractStartDateFieldSetting.IsCopy)
                businessModel.ContractFields.ContractStartDate = null;
            if (!settings.ContractFieldsSettings.ContractStatusFieldSetting.IsCopy)
                businessModel.ContractFields.ContractStatusId = null;
            if (!settings.ContractFieldsSettings.PurchaseDateFieldSetting.IsCopy)
                businessModel.InventoryFields.PurchaseDate = null; // purchase date was moved from Inventory to Contract on UI.
            if (!settings.ContractFieldsSettings.PurchasePriceFieldSetting.IsCopy)
                businessModel.ContractFields.PurchasePrice = 0;

            // businessModel dont' have DateFields so just ignor it

            if (!settings.GraphicsFieldsSettings.VideoCardFieldSetting.IsCopy)
                businessModel.GraphicsFields.VideoCard = null;

            if (!settings.InventoryFieldsSettings.BarCodeFieldSetting.IsCopy)
                businessModel.InventoryFields.BarCode = null;
            if (!settings.InventoryFieldsSettings.PurchaseDateFieldSetting.IsCopy)
                businessModel.InventoryFields.PurchaseDate = null;

            if (!settings.MemoryFieldsSettings.RAMFieldSetting.IsCopy)
                businessModel.MemoryFields.RAMId = null;

            if (!settings.OperatingSystemFieldsSettings.OperatingSystemFieldSetting.IsCopy)
                businessModel.OperatingSystemFields.OperatingSystemId = null;
            if (!settings.OperatingSystemFieldsSettings.ProductKeyFieldSetting.IsCopy)
                businessModel.OperatingSystemFields.ProductKey = null;
            if (!settings.OperatingSystemFieldsSettings.RegistrationCodeSystemFieldSetting.IsCopy)
                businessModel.OperatingSystemFields.RegistrationCode = null;
            if (!settings.OperatingSystemFieldsSettings.ServicePackSystemFieldSetting.IsCopy)
                businessModel.OperatingSystemFields.ServicePack = null;
            if (!settings.OperatingSystemFieldsSettings.VersionFieldSetting.IsCopy)
                businessModel.OperatingSystemFields.Version = null;

            if (!settings.OrganizationFieldsSettings.DepartmentFieldSetting.IsCopy)
                businessModel.OrganizationFields.DepartmentId = null;
            if (!settings.OrganizationFieldsSettings.DomainFieldSetting.IsCopy)
                businessModel.OrganizationFields.DomainId = null;
            if (!settings.OrganizationFieldsSettings.RegionFieldSetting.IsCopy)
                businessModel.OrganizationFields.RegionId = null;
            if (!settings.OrganizationFieldsSettings.UnitFieldSetting.IsCopy)
                businessModel.OrganizationFields.UnitId = null;

            if (!settings.OtherFieldsSettings.InfoFieldSetting.IsCopy)
                businessModel.OtherFields.Info = null;

            if (!settings.PlaceFieldsSettings.AddressFieldSetting.IsCopy)
                businessModel.PlaceFields.Address = null;
            if (!settings.PlaceFieldsSettings.Place2FieldSetting.IsCopy)
                businessModel.PlaceFields.Location2 = null;
            if (!settings.PlaceFieldsSettings.PlaceFieldSetting.IsCopy)
                businessModel.PlaceFields.Location = null;
            if (!settings.PlaceFieldsSettings.PostalAddressFieldSetting.IsCopy)
                businessModel.PlaceFields.PostalAddress = null;
            if (!settings.PlaceFieldsSettings.PostalCodeFieldSetting.IsCopy)
                businessModel.PlaceFields.PostalCode = null;
            if (!settings.PlaceFieldsSettings.RoomFieldSetting.IsCopy)
                businessModel.PlaceFields.RoomId = null;

            if (!settings.ProccesorFieldsSettings.ProccesorFieldSetting.IsCopy)
                businessModel.ProccesorFields.ProccesorId = null;

            if (!settings.SoundFieldsSettings.SoundCardFieldSetting.IsCopy)
                businessModel.SoundFields.SoundCard = null;

            if (!settings.StateFieldsSettings.ReplacedWithFieldSetting.IsCopy)
                businessModel.StateFields.Replaced = null;
            if (!settings.StateFieldsSettings.ScrapDateFieldSetting.IsCopy)
                businessModel.StateFields.ScrapDate = null;
            if (!settings.StateFieldsSettings.SendBackFieldSetting.IsCopy)
                businessModel.StateFields.IsSendBack = false;
            if (!settings.StateFieldsSettings.StateFieldSetting.IsCopy)
                businessModel.StateFields.State = 0;
            if (!settings.StateFieldsSettings.StolenFieldSetting.IsCopy)
                businessModel.StateFields.IsStolen = false;

            if (!settings.WorkstationFieldsSettings.BIOSDateFieldSetting.IsCopy)
                businessModel.WorkstationFields.BIOSDate = null;
            if (!settings.WorkstationFieldsSettings.BIOSVersionFieldSetting.IsCopy)
                businessModel.WorkstationFields.BIOSVersion = null;
            if (!settings.WorkstationFieldsSettings.CarePackNumberFieldSetting.IsCopy)
                businessModel.WorkstationFields.CarePackNumber = null;
            if (!settings.WorkstationFieldsSettings.ComputerModelFieldSetting.IsCopy)
                businessModel.WorkstationFields.ComputerModelId = null;
            if (!settings.WorkstationFieldsSettings.ComputerNameFieldSetting.IsCopy)
                businessModel.WorkstationFields.ComputerName = null;
            if (!settings.WorkstationFieldsSettings.ComputerTypeFieldSetting.IsCopy)
            {
                businessModel.WorkstationFields.ComputerTypeId = null;
                businessModel.WorkstationFields.ComputerTypeName = null;
            }

            if (!settings.WorkstationFieldsSettings.LocationFieldSetting.IsCopy)
                businessModel.WorkstationFields.Location = null;
            if (!settings.WorkstationFieldsSettings.ManufacturerFieldSetting.IsCopy)
                businessModel.WorkstationFields.Manufacturer = null;
            if (!settings.WorkstationFieldsSettings.SerialNumberFieldSetting.IsCopy)
                businessModel.WorkstationFields.SerialNumber = null;
            if (!settings.WorkstationFieldsSettings.TheftmarkFieldSetting.IsCopy)
                businessModel.WorkstationFields.Theftmark = null;
        }
    }
}