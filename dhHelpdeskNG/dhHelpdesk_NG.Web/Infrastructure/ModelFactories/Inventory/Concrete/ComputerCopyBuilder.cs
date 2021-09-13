using System;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings;
using DH.Helpdesk.Dal.Repositories.Computers;
using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer;

namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory.Concrete
{
    public class ComputerCopyBuilder : IComputerCopyBuilder
    {
        private readonly IComputerFieldSettingsRepository _computerFieldSettingsRepository;

        public ComputerCopyBuilder(IComputerFieldSettingsRepository computerFieldSettingsRepository)
        {
            _computerFieldSettingsRepository = computerFieldSettingsRepository;
        }

        public ComputerViewModel CopyWorkstation(ComputerViewModel destModel, ComputerViewModel sourceModel, OperationContext context)
        {
            var settings = _computerFieldSettingsRepository.GetFieldSettingsProcessing(context.CustomerId);
            return ApplyCopySettings(destModel, sourceModel, settings);
        }

        private ComputerViewModel ApplyCopySettings(ComputerViewModel destModel, ComputerViewModel sourceModel, ComputerFieldsSettingsProcessing settings)
        {
            CopyField(destModel.ContactFieldsModel.Email, sourceModel.ContactFieldsModel.Email, settings.ContactFieldsSettings.EmailFieldSetting);
            CopyField(destModel.ContactFieldsModel.Name, sourceModel.ContactFieldsModel.Name, settings.ContactFieldsSettings.NameFieldSetting);
            CopyField(destModel.ContactFieldsModel.Phone, sourceModel.ContactFieldsModel.Phone, settings.ContactFieldsSettings.PhoneFieldSetting);

            if (settings.ContactInformationFieldsSettings.UserIdFieldSetting.IsCopy)
                destModel.ContactInformationFieldsModel.UserId = sourceModel.ContactInformationFieldsModel.UserId;

            CopyField(destModel.ChassisFieldsModel.Chassis, sourceModel.ChassisFieldsModel.Chassis, settings.ChassisFieldsSettings.ChassisFieldSetting);

            CopyField(destModel.CommunicationFieldsViewModel.CommunicationFieldsModel.IPAddress, 
                sourceModel.CommunicationFieldsViewModel.CommunicationFieldsModel.IPAddress, settings.CommunicationFieldsSettings.IPAddressFieldSetting);
            CopyField(destModel.CommunicationFieldsViewModel.CommunicationFieldsModel.MacAddress, 
                sourceModel.CommunicationFieldsViewModel.CommunicationFieldsModel.MacAddress, settings.CommunicationFieldsSettings.MacAddressFieldSetting);
            CopyField(destModel.CommunicationFieldsViewModel.CommunicationFieldsModel.NetworkAdapterId, 
                sourceModel.CommunicationFieldsViewModel.CommunicationFieldsModel.NetworkAdapterId, settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting);
            CopyField(destModel.CommunicationFieldsViewModel.CommunicationFieldsModel.NovellClient, 
                sourceModel.CommunicationFieldsViewModel.CommunicationFieldsModel.NovellClient, settings.CommunicationFieldsSettings.NovellClientFieldSetting);
            CopyField(destModel.CommunicationFieldsViewModel.CommunicationFieldsModel.IsRAS, 
                sourceModel.CommunicationFieldsViewModel.CommunicationFieldsModel.IsRAS, settings.CommunicationFieldsSettings.RASFieldSetting);

            CopyField(destModel.ContractFieldsViewModel.ContractFieldsModel.AccountingDimension1, 
                sourceModel.ContractFieldsViewModel.ContractFieldsModel.AccountingDimension1, settings.ContractFieldsSettings.AccountingDimension1FieldSetting);
            CopyField(destModel.ContractFieldsViewModel.ContractFieldsModel.AccountingDimension2, 
                sourceModel.ContractFieldsViewModel.ContractFieldsModel.AccountingDimension2, settings.ContractFieldsSettings.AccountingDimension2FieldSetting);
            CopyField(destModel.ContractFieldsViewModel.ContractFieldsModel.AccountingDimension3, 
                sourceModel.ContractFieldsViewModel.ContractFieldsModel.AccountingDimension3, settings.ContractFieldsSettings.AccountingDimension3FieldSetting);
            CopyField(destModel.ContractFieldsViewModel.ContractFieldsModel.AccountingDimension4, 
                sourceModel.ContractFieldsViewModel.ContractFieldsModel.AccountingDimension4, settings.ContractFieldsSettings.AccountingDimension4FieldSetting);
            CopyField(destModel.ContractFieldsViewModel.ContractFieldsModel.AccountingDimension5, 
                sourceModel.ContractFieldsViewModel.ContractFieldsModel.AccountingDimension5, settings.ContractFieldsSettings.AccountingDimension5FieldSetting);
            CopyField(destModel.ContractFieldsViewModel.ContractFieldsModel.ContractEndDate, 
                sourceModel.ContractFieldsViewModel.ContractFieldsModel.ContractEndDate, settings.ContractFieldsSettings.ContractEndDateFieldSetting);
            CopyField(destModel.ContractFieldsViewModel.ContractFieldsModel.ContractNumber, 
                sourceModel.ContractFieldsViewModel.ContractFieldsModel.ContractNumber, settings.ContractFieldsSettings.ContractNumberFieldSetting);
            CopyField(destModel.ContractFieldsViewModel.ContractFieldsModel.ContractStartDate, 
                sourceModel.ContractFieldsViewModel.ContractFieldsModel.ContractStartDate, settings.ContractFieldsSettings.ContractStartDateFieldSetting);
            CopyField(destModel.ContractFieldsViewModel.ContractFieldsModel.ContractStatusId, 
                sourceModel.ContractFieldsViewModel.ContractFieldsModel.ContractStatusId, settings.ContractFieldsSettings.ContractStatusFieldSetting);
            CopyField(destModel.ContractFieldsViewModel.ContractFieldsModel.PurchaseDate, // purchase date was moved from Inventory to Contract on UI.
                sourceModel.ContractFieldsViewModel.ContractFieldsModel.PurchaseDate, settings.ContractFieldsSettings.PurchaseDateFieldSetting);
            CopyField(destModel.ContractFieldsViewModel.ContractFieldsModel.PurchasePrice, 
                sourceModel.ContractFieldsViewModel.ContractFieldsModel.PurchasePrice, settings.ContractFieldsSettings.PurchasePriceFieldSetting);


            // destModel dont' have DateFields so just ignor it

            CopyField(destModel.GraphicsFieldsModel.VideoCard, 
                sourceModel.GraphicsFieldsModel.VideoCard, settings.GraphicsFieldsSettings.VideoCardFieldSetting);

            CopyField(destModel.InventoryFieldsModel.BarCode, 
                sourceModel.InventoryFieldsModel.BarCode, settings.InventoryFieldsSettings.BarCodeFieldSetting);
            CopyField(destModel.ContractFieldsViewModel.ContractFieldsModel.PurchaseDate, // purchase date was moved from Inventory to Contract on UI
                sourceModel.ContractFieldsViewModel.ContractFieldsModel.PurchaseDate, settings.InventoryFieldsSettings.PurchaseDateFieldSetting);

            CopyField(destModel.MemoryFieldsViewModel.MemoryFieldsModel.RAMId, 
                sourceModel.MemoryFieldsViewModel.MemoryFieldsModel.RAMId, settings.MemoryFieldsSettings.RAMFieldSetting);

            CopyField(destModel.OperatingSystemFieldsViewModel.OperatingSystemFieldsModel.OperatingSystemId, 
                sourceModel.OperatingSystemFieldsViewModel.OperatingSystemFieldsModel.OperatingSystemId, settings.OperatingSystemFieldsSettings.OperatingSystemFieldSetting);
            CopyField(destModel.OperatingSystemFieldsViewModel.OperatingSystemFieldsModel.ProductKey, 
                sourceModel.OperatingSystemFieldsViewModel.OperatingSystemFieldsModel.ProductKey, settings.OperatingSystemFieldsSettings.ProductKeyFieldSetting);
            CopyField(destModel.OperatingSystemFieldsViewModel.OperatingSystemFieldsModel.RegistrationCode, 
                sourceModel.OperatingSystemFieldsViewModel.OperatingSystemFieldsModel.RegistrationCode, settings.OperatingSystemFieldsSettings.RegistrationCodeSystemFieldSetting);
            CopyField(destModel.OperatingSystemFieldsViewModel.OperatingSystemFieldsModel.ServicePack, 
                sourceModel.OperatingSystemFieldsViewModel.OperatingSystemFieldsModel.ServicePack, settings.OperatingSystemFieldsSettings.ServicePackSystemFieldSetting);
            CopyField(destModel.OperatingSystemFieldsViewModel.OperatingSystemFieldsModel.Version, 
                sourceModel.OperatingSystemFieldsViewModel.OperatingSystemFieldsModel.Version, settings.OperatingSystemFieldsSettings.VersionFieldSetting);

            CopyField(destModel.OrganizationFieldsViewModel.OrganizationFieldsModel.DepartmentId, 
                sourceModel.OrganizationFieldsViewModel.OrganizationFieldsModel.DepartmentId, settings.OrganizationFieldsSettings.DepartmentFieldSetting);
            CopyField(destModel.OrganizationFieldsViewModel.OrganizationFieldsModel.DomainId, 
                sourceModel.OrganizationFieldsViewModel.OrganizationFieldsModel.DomainId, settings.OrganizationFieldsSettings.DomainFieldSetting);
            CopyField(destModel.OrganizationFieldsViewModel.OrganizationFieldsModel.RegionId, 
                sourceModel.OrganizationFieldsViewModel.OrganizationFieldsModel.RegionId, settings.OrganizationFieldsSettings.RegionFieldSetting);
            CopyField(destModel.OrganizationFieldsViewModel.OrganizationFieldsModel.UnitId, 
                sourceModel.OrganizationFieldsViewModel.OrganizationFieldsModel.UnitId, settings.OrganizationFieldsSettings.UnitFieldSetting);

            CopyField(destModel.OtherFieldsModel.Info, 
                sourceModel.OtherFieldsModel.Info, settings.OtherFieldsSettings.InfoFieldSetting);

            CopyField(destModel.PlaceFieldsViewModel.PlaceFieldsModel.Address, 
                sourceModel.PlaceFieldsViewModel.PlaceFieldsModel.Address, settings.PlaceFieldsSettings.AddressFieldSetting);
            CopyField(destModel.PlaceFieldsViewModel.PlaceFieldsModel.Location2, 
                sourceModel.PlaceFieldsViewModel.PlaceFieldsModel.Location2, settings.PlaceFieldsSettings.Place2FieldSetting);
            CopyField(destModel.PlaceFieldsViewModel.PlaceFieldsModel.Location, 
                sourceModel.PlaceFieldsViewModel.PlaceFieldsModel.Location, settings.PlaceFieldsSettings.PlaceFieldSetting);
            CopyField(destModel.PlaceFieldsViewModel.PlaceFieldsModel.PostalAddress, 
                sourceModel.PlaceFieldsViewModel.PlaceFieldsModel.PostalAddress, settings.PlaceFieldsSettings.PostalAddressFieldSetting);
            CopyField(destModel.PlaceFieldsViewModel.PlaceFieldsModel.PostalCode, 
                sourceModel.PlaceFieldsViewModel.PlaceFieldsModel.PostalCode, settings.PlaceFieldsSettings.PostalCodeFieldSetting);
            CopyField(destModel.PlaceFieldsViewModel.PlaceFieldsModel.RoomId, 
                sourceModel.PlaceFieldsViewModel.PlaceFieldsModel.RoomId, settings.PlaceFieldsSettings.RoomFieldSetting);
            CopyField(destModel.PlaceFieldsViewModel.PlaceFieldsModel.BuildingId, 
                sourceModel.PlaceFieldsViewModel.PlaceFieldsModel.BuildingId, settings.PlaceFieldsSettings.BuildingFieldSetting);
            CopyField(destModel.PlaceFieldsViewModel.PlaceFieldsModel.FloorId, 
                sourceModel.PlaceFieldsViewModel.PlaceFieldsModel.FloorId, settings.PlaceFieldsSettings.FloorFieldSetting);

            CopyField(destModel.ProccesorFieldsViewModel.ProccesorFieldsModel.ProccesorId, 
                sourceModel.ProccesorFieldsViewModel.ProccesorFieldsModel.ProccesorId, settings.ProccesorFieldsSettings.ProccesorFieldSetting);

            CopyField(destModel.SoundFieldsModel.SoundCard, 
                sourceModel.SoundFieldsModel.SoundCard, settings.SoundFieldsSettings.SoundCardFieldSetting);

            CopyField(destModel.StateFieldsViewModel.StateFieldsModel.Replaced, 
                sourceModel.StateFieldsViewModel.StateFieldsModel.Replaced, settings.StateFieldsSettings.ReplacedWithFieldSetting);
            CopyField(destModel.StateFieldsViewModel.StateFieldsModel.ScrapDate, 
                sourceModel.StateFieldsViewModel.StateFieldsModel.ScrapDate, settings.StateFieldsSettings.ScrapDateFieldSetting);
            CopyField(destModel.StateFieldsViewModel.StateFieldsModel.IsSendBack, 
                sourceModel.StateFieldsViewModel.StateFieldsModel.IsSendBack, settings.StateFieldsSettings.SendBackFieldSetting);
            CopyField(destModel.StateFieldsViewModel.StateFieldsModel.StateId, 
                sourceModel.StateFieldsViewModel.StateFieldsModel.StateId, settings.StateFieldsSettings.StateFieldSetting);
            CopyField(destModel.StateFieldsViewModel.StateFieldsModel.IsStolen, 
                sourceModel.StateFieldsViewModel.StateFieldsModel.IsStolen, settings.StateFieldsSettings.StolenFieldSetting);

            CopyField(destModel.WorkstationFieldsViewModel.WorkstationFieldsModel.BIOSDate, 
                sourceModel.WorkstationFieldsViewModel.WorkstationFieldsModel.BIOSDate, settings.WorkstationFieldsSettings.BIOSDateFieldSetting);
            CopyField(destModel.WorkstationFieldsViewModel.WorkstationFieldsModel.BIOSVersion, 
                sourceModel.WorkstationFieldsViewModel.WorkstationFieldsModel.BIOSVersion, settings.WorkstationFieldsSettings.BIOSVersionFieldSetting);
            CopyField(destModel.WorkstationFieldsViewModel.WorkstationFieldsModel.CarePackNumber, 
                sourceModel.WorkstationFieldsViewModel.WorkstationFieldsModel.CarePackNumber, settings.WorkstationFieldsSettings.CarePackNumberFieldSetting);
            CopyField(destModel.WorkstationFieldsViewModel.WorkstationFieldsModel.ComputerModelId, 
                sourceModel.WorkstationFieldsViewModel.WorkstationFieldsModel.ComputerModelId, settings.WorkstationFieldsSettings.ComputerModelFieldSetting);
            CopyField(destModel.WorkstationFieldsViewModel.WorkstationFieldsModel.Name, 
                sourceModel.WorkstationFieldsViewModel.WorkstationFieldsModel.Name, settings.WorkstationFieldsSettings.ComputerNameFieldSetting);
            CopyField(destModel.WorkstationFieldsViewModel.WorkstationFieldsModel.ComputerTypeId, 
                sourceModel.WorkstationFieldsViewModel.WorkstationFieldsModel.ComputerTypeId, settings.WorkstationFieldsSettings.ComputerTypeFieldSetting);

            CopyField(destModel.WorkstationFieldsViewModel.WorkstationFieldsModel.Location, 
                sourceModel.WorkstationFieldsViewModel.WorkstationFieldsModel.Location, settings.WorkstationFieldsSettings.LocationFieldSetting);
            CopyField(destModel.WorkstationFieldsViewModel.WorkstationFieldsModel.Manufacturer, 
                sourceModel.WorkstationFieldsViewModel.WorkstationFieldsModel.Manufacturer, settings.WorkstationFieldsSettings.ManufacturerFieldSetting);
            CopyField(destModel.WorkstationFieldsViewModel.WorkstationFieldsModel.SerialNumber, 
                sourceModel.WorkstationFieldsViewModel.WorkstationFieldsModel.SerialNumber, settings.WorkstationFieldsSettings.SerialNumberFieldSetting);
            CopyField(destModel.WorkstationFieldsViewModel.WorkstationFieldsModel.Theftmark, 
                sourceModel.WorkstationFieldsViewModel.WorkstationFieldsModel.Theftmark, settings.WorkstationFieldsSettings.TheftmarkFieldSetting);

            return destModel;
        }

        private void CopyField<T>(ConfigurableFieldModel<T> destValue, ConfigurableFieldModel<T> sourceValue, ProcessingFieldSetting setting)
        {
            if (destValue != null && sourceValue != null && setting.IsCopy)
                destValue.Value = sourceValue.Value;
        }
    }
}