namespace DH.Helpdesk.Web.Models.Inventory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Printer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Server;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.PrinterFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ServerFieldSettings;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.DisplayValues;
    using DH.Helpdesk.Services.DisplayValues.Inventory;
    using DH.Helpdesk.Services.Response.Inventory;
    using DH.Helpdesk.Web.Models.Common;

    public sealed class InventoryGridModel
    {
        public InventoryGridModel(
            int inventoriesFound,
            List<GridColumnHeaderModel> headers,
            List<InventoryOverviewModel> inventories)
        {
            this.InventoriesFound = inventoriesFound;
            this.Headers = headers;
            this.Inventories = inventories;
        }

        [NotNull]
        public List<GridColumnHeaderModel> Headers { get; set; }

        [NotNull]
        public List<InventoryOverviewModel> Inventories { get; set; }

        [MinValue(0)]
        public int InventoriesFound { get; set; }

        public CurrentModes CurrentMode { get; set; }

        public static InventoryGridModel BuildModel(List<ComputerOverview> modelList, ComputerFieldsSettingsOverview settings)
        {
            var headers = new List<GridColumnHeaderModel>();

            CreateHeaderIfNeeded(
                settings.WorkstationFieldsSettings.ComputerNameFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.Name,
                headers);
            CreateHeaderIfNeeded(
                settings.WorkstationFieldsSettings.ManufacturerFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.Manufacturer,
                headers);
            CreateHeaderIfNeeded(
                settings.WorkstationFieldsSettings.ComputerModelFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.Model,
                headers);
            CreateHeaderIfNeeded(
                settings.WorkstationFieldsSettings.SerialNumberFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.SerialNumber,
                headers);
            CreateHeaderIfNeeded(
                settings.WorkstationFieldsSettings.BIOSVersionFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.BIOSVersion,
                headers);
            CreateHeaderIfNeeded(
                settings.WorkstationFieldsSettings.BIOSDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.BIOSDate,
                headers);
            CreateHeaderIfNeeded(
                settings.WorkstationFieldsSettings.TheftmarkFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.Theftmark,
                headers);
            CreateHeaderIfNeeded(
                settings.WorkstationFieldsSettings.CarePackNumberFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.CarePackNumber,
                headers);
            CreateHeaderIfNeeded(
                settings.WorkstationFieldsSettings.ComputerTypeFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.ComputerType,
                headers);
            CreateHeaderIfNeeded(
                settings.WorkstationFieldsSettings.LocationFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.Location,
                headers);

            CreateHeaderIfNeeded(
                settings.ChassisFieldsSettings.ChassisFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.ChassisFields.Chassis,
                headers);

            CreateHeaderIfNeeded(
                settings.InventoryFieldsSettings.BarCodeFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.InventoryFields.BarCode,
                headers);
            CreateHeaderIfNeeded(
                settings.InventoryFieldsSettings.PurchaseDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.InventoryFields.PurchaseDate,
                headers);

            CreateHeaderIfNeeded(
                settings.OperatingSystemFieldsSettings.OperatingSystemFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.OperatingSystemFields.OperatingSystem,
                headers);
            CreateHeaderIfNeeded(
                settings.OperatingSystemFieldsSettings.VersionFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.OperatingSystemFields.Version,
                headers);
            CreateHeaderIfNeeded(
                settings.OperatingSystemFieldsSettings.ServicePackSystemFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.OperatingSystemFields.ServicePack,
                headers);
            CreateHeaderIfNeeded(
                settings.OperatingSystemFieldsSettings.RegistrationCodeSystemFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.OperatingSystemFields.RegistrationCode,
                headers);
            CreateHeaderIfNeeded(
                settings.OperatingSystemFieldsSettings.ProductKeyFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.OperatingSystemFields.ProductKey,
                headers);

            CreateHeaderIfNeeded(
                settings.ProccesorFieldsSettings.ProccesorFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.ProcessorFields.ProccesorName,
                headers);

            CreateHeaderIfNeeded(
                settings.MemoryFieldsSettings.RAMFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.MemoryFields.RAM,
                headers);

            CreateHeaderIfNeeded(
                settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.CommunicationFields.NetworkAdapter,
                headers);
            CreateHeaderIfNeeded(
                settings.CommunicationFieldsSettings.IPAddressFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.CommunicationFields.IPAddress,
                headers);
            CreateHeaderIfNeeded(
                settings.CommunicationFieldsSettings.MacAddressFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.CommunicationFields.MacAddress,
                headers);
            CreateHeaderIfNeeded(
                settings.CommunicationFieldsSettings.RASFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.CommunicationFields.RAS,
                headers);
            CreateHeaderIfNeeded(
                settings.CommunicationFieldsSettings.NovellClientFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.CommunicationFields.NovellClient,
                headers);

            CreateHeaderIfNeeded(
                settings.GraphicsFieldsSettings.VideoCardFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.GraphicsFields.VideoCard,
                headers);

            CreateHeaderIfNeeded(
                settings.SoundFieldsSettings.SoundCardFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.SoundFields.SoundCard,
                headers);

            CreateHeaderIfNeeded(
                settings.ContractFieldsSettings.ContractStatusFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.ContractStatusName,
                headers);
            CreateHeaderIfNeeded(
                settings.ContractFieldsSettings.ContractNumberFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.ContractNumber,
                headers);
            CreateHeaderIfNeeded(
                settings.ContractFieldsSettings.ContractStartDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.ContractStartDate,
                headers);
            CreateHeaderIfNeeded(
                settings.ContractFieldsSettings.ContractEndDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.ContractEndDate,
                headers);
            CreateHeaderIfNeeded(
                settings.ContractFieldsSettings.PurchasePriceFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.PurchasePrice,
                headers);
            CreateHeaderIfNeeded(
                settings.ContractFieldsSettings.AccountingDimension1FieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.AccountingDimension1,
                headers);
            CreateHeaderIfNeeded(
                settings.ContractFieldsSettings.AccountingDimension2FieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.AccountingDimension2,
                headers);
            CreateHeaderIfNeeded(
                settings.ContractFieldsSettings.AccountingDimension3FieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.AccountingDimension3,
                headers);
            CreateHeaderIfNeeded(
                settings.ContractFieldsSettings.AccountingDimension4FieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.AccountingDimension4,
                headers);
            CreateHeaderIfNeeded(
                settings.ContractFieldsSettings.AccountingDimension5FieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.AccountingDimension5,
                headers);

            CreateHeaderIfNeeded(
                settings.OtherFieldsSettings.InfoFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.OtherFields.Info,
                headers);

            CreateHeaderIfNeeded(
                settings.ContactInformationFieldsSettings.UserIdFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContactInformationFields.UserId,
                headers);

            CreateHeaderIfNeeded(
                settings.OrganizationFieldsSettings.DepartmentFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.OrganizationFields.Department,
                headers);
            CreateHeaderIfNeeded(
                settings.OrganizationFieldsSettings.DomainFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.OrganizationFields.Domain,
                headers);
            CreateHeaderIfNeeded(
                settings.OrganizationFieldsSettings.UnitFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.OrganizationFields.Unit,
                headers);

            CreateHeaderIfNeeded(
                settings.PlaceFieldsSettings.RoomFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.PlaceFields.Room,
                headers);
            CreateHeaderIfNeeded(
                settings.PlaceFieldsSettings.AddressFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.PlaceFields.Address,
                headers);
            CreateHeaderIfNeeded(
                settings.PlaceFieldsSettings.PostalCodeFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.PlaceFields.PostalCode,
                headers);
            CreateHeaderIfNeeded(
                settings.PlaceFieldsSettings.PostalAddressFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.PlaceFields.PostalAddress,
                headers);
            CreateHeaderIfNeeded(
                settings.PlaceFieldsSettings.PlaceFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.PlaceFields.Location,
                headers);
            CreateHeaderIfNeeded(
                settings.PlaceFieldsSettings.Place2FieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.PlaceFields.Location2,
                headers);

            CreateHeaderIfNeeded(
                settings.ContactFieldsSettings.NameFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContactFields.Name,
                headers);
            CreateHeaderIfNeeded(
                settings.ContactFieldsSettings.PhoneFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContactFields.Phone,
                headers);
            CreateHeaderIfNeeded(
                settings.ContactFieldsSettings.EmailFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContactFields.Email,
                headers);

            CreateHeaderIfNeeded(
                settings.StateFieldsSettings.StateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.StateFields.State,
                headers);
            CreateHeaderIfNeeded(
                settings.StateFieldsSettings.StolenFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.StateFields.Stolen,
                headers);
            CreateHeaderIfNeeded(
                settings.StateFieldsSettings.ReplacedWithFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.StateFields.Replaced,
                headers);
            CreateHeaderIfNeeded(
                settings.StateFieldsSettings.SendBackFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.StateFields.SendBack,
                headers);
            CreateHeaderIfNeeded(
                settings.StateFieldsSettings.ScrapDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.StateFields.ScrapDate,
                headers);

            CreateHeaderIfNeeded(
                settings.DateFieldsSettings.CreatedDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.DateFields.CreatedDate,
                headers);
            CreateHeaderIfNeeded(
                settings.DateFieldsSettings.ChangedDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.DateFields.ChangedDate,
                headers);
            CreateHeaderIfNeeded(
                settings.DateFieldsSettings.SyncChangedDateSetting,
                BusinessData.Enums.Inventory.Fields.Computer.DateFields.SynchronizeDate,
                headers);
            CreateHeaderIfNeeded(
                settings.DateFieldsSettings.ScanDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.DateFields.ScanDate,
                headers);
            CreateHeaderIfNeeded(
                settings.DateFieldsSettings.PathDirectoryFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.DateFields.PathDirectory,
                headers);

            var overviews = modelList.Select(c => CreateComputerOverview(c, settings)).ToList();

            throw new NotImplementedException();
        }

        public static InventoryGridModel BuildModel(List<ServerOverview> modelList, ServerFieldsSettingsOverview settings)
        {
            throw new NotImplementedException();
        }

        public static InventoryGridModel BuildModel(List<PrinterOverview> modelList, PrinterFieldsSettingsOverview settings)
        {
            throw new NotImplementedException();
        }

        public static InventoryGridModel BuildModel(InventoryOverviewResponse modelList, InventoryFieldSettingsOverviewResponse settings)
        {
            throw new NotImplementedException();
        }

        private static List<NewGridRowCellValueModel> CreateComputerOverview(
                        ComputerOverview overview,
                        ComputerFieldsSettingsOverview settings)
        {
            var values = new List<NewGridRowCellValueModel>();

            CreateValueIfNeeded(
                settings.WorkstationFieldsSettings.ComputerNameFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.Name,
                (StringDisplayValue)overview.WorkstationFields.ComputerName,
                values);
            CreateValueIfNeeded(
                settings.WorkstationFieldsSettings.ManufacturerFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.Manufacturer,
                (StringDisplayValue)overview.WorkstationFields.Manufacturer,
                values);
            CreateValueIfNeeded(
                settings.WorkstationFieldsSettings.ComputerModelFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.Model,
                (StringDisplayValue)overview.WorkstationFields.ComputerModelName,
                values);
            CreateValueIfNeeded(
                settings.WorkstationFieldsSettings.SerialNumberFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.SerialNumber,
                (StringDisplayValue)overview.WorkstationFields.SerialNumber,
                values);
            CreateValueIfNeeded(
                settings.WorkstationFieldsSettings.BIOSVersionFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.BIOSVersion,
                (StringDisplayValue)overview.WorkstationFields.BIOSVersion,
                values);
            CreateValueIfNeeded(
                settings.WorkstationFieldsSettings.BIOSDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.BIOSDate,
                (StringDisplayValue)overview.WorkstationFields.BIOSVersion,
                values);
            CreateValueIfNeeded(
                settings.WorkstationFieldsSettings.TheftmarkFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.Theftmark,
                (StringDisplayValue)overview.WorkstationFields.Theftmark,
                values);
            CreateValueIfNeeded(
                settings.WorkstationFieldsSettings.CarePackNumberFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.CarePackNumber,
                (StringDisplayValue)overview.WorkstationFields.CarePackNumber,
                values);
            CreateValueIfNeeded(
                settings.WorkstationFieldsSettings.ComputerTypeFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.ComputerType,
                (StringDisplayValue)overview.WorkstationFields.ComputerTypeName,
                values);
            CreateValueIfNeeded(
                settings.WorkstationFieldsSettings.LocationFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.Location,
                (StringDisplayValue)overview.WorkstationFields.Location,
                values);

            CreateValueIfNeeded(
                settings.ChassisFieldsSettings.ChassisFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.ChassisFields.Chassis,
                (StringDisplayValue)overview.ChassisFields.Chassis,
                values);

            CreateValueIfNeeded(
                settings.InventoryFieldsSettings.BarCodeFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.InventoryFields.BarCode,
                (StringDisplayValue)overview.InventoryFields.BarCode,
                values);
            CreateValueIfNeeded(
                settings.InventoryFieldsSettings.PurchaseDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.InventoryFields.PurchaseDate,
                (DateTimeDisplayValue)overview.InventoryFields.PurchaseDate,
                values);

            CreateValueIfNeeded(
                settings.OperatingSystemFieldsSettings.OperatingSystemFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.OperatingSystemFields.OperatingSystem,
                (StringDisplayValue)overview.OperatingSystemFields.OperatingSystemName,
                values);
            CreateValueIfNeeded(
                settings.OperatingSystemFieldsSettings.VersionFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.OperatingSystemFields.Version,
                (StringDisplayValue)overview.OperatingSystemFields.Version,
                values);
            CreateValueIfNeeded(
                settings.OperatingSystemFieldsSettings.ServicePackSystemFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.OperatingSystemFields.ServicePack,
                (StringDisplayValue)overview.OperatingSystemFields.ServicePack,
                values);
            CreateValueIfNeeded(
                settings.OperatingSystemFieldsSettings.RegistrationCodeSystemFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.OperatingSystemFields.RegistrationCode,
                (StringDisplayValue)overview.OperatingSystemFields.RegistrationCode,
                values);
            CreateValueIfNeeded(
                settings.OperatingSystemFieldsSettings.ProductKeyFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.OperatingSystemFields.ProductKey,
                (StringDisplayValue)overview.OperatingSystemFields.ProductKey,
                values);

            CreateValueIfNeeded(
                settings.ProccesorFieldsSettings.ProccesorFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.ProcessorFields.ProccesorName,
                (StringDisplayValue)overview.ProccesorFields.ProccesorName,
                values);

            CreateValueIfNeeded(
                settings.MemoryFieldsSettings.RAMFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.MemoryFields.RAM,
                (StringDisplayValue)overview.MemoryFields.RAMName,
                values);

            CreateValueIfNeeded(
                settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.CommunicationFields.NetworkAdapter,
                (StringDisplayValue)overview.CommunicationFields.NetworkAdapterName,
                values);
            CreateValueIfNeeded(
                settings.CommunicationFieldsSettings.IPAddressFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.CommunicationFields.IPAddress,
                (StringDisplayValue)overview.CommunicationFields.IPAddress,
                values);
            CreateValueIfNeeded(
                settings.CommunicationFieldsSettings.MacAddressFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.CommunicationFields.MacAddress,
                (StringDisplayValue)overview.CommunicationFields.MacAddress,
                values);
            CreateValueIfNeeded(
                settings.CommunicationFieldsSettings.RASFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.CommunicationFields.RAS,
                (BooleanDisplayValue)overview.CommunicationFields.RAS,
                values);
            CreateValueIfNeeded(
                settings.CommunicationFieldsSettings.NovellClientFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.CommunicationFields.NovellClient,
                (StringDisplayValue)overview.CommunicationFields.NovellClient,
                values);

            CreateValueIfNeeded(
                settings.GraphicsFieldsSettings.VideoCardFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.GraphicsFields.VideoCard,
                (StringDisplayValue)overview.GraphicsFields.VideoCard,
                values);

            CreateValueIfNeeded(
                settings.SoundFieldsSettings.SoundCardFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.SoundFields.SoundCard,
                (StringDisplayValue)overview.SoundFields.SoundCard,
                values);

            CreateValueIfNeeded(
                settings.ContractFieldsSettings.ContractStatusFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.ContractStatusName,
                (ContractStatusDisplayValue)overview.ContractFields.ContractStatusId,
                values);
            CreateValueIfNeeded(
                settings.ContractFieldsSettings.ContractNumberFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.ContractNumber,
                (StringDisplayValue)overview.ContractFields.ContractNumber,
                values);
            CreateValueIfNeeded(
                settings.ContractFieldsSettings.ContractStartDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.ContractStartDate,
                (DateTimeDisplayValue)overview.ContractFields.ContractStartDate,
                values);
            CreateValueIfNeeded(
                settings.ContractFieldsSettings.ContractEndDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.ContractEndDate,
                (DateTimeDisplayValue)overview.ContractFields.ContractEndDate,
                values);
            CreateValueIfNeeded(
                settings.ContractFieldsSettings.PurchasePriceFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.PurchasePrice,
                (IntegerDisplayValue)overview.ContractFields.PurchasePrice,
                values);
            CreateValueIfNeeded(
                settings.ContractFieldsSettings.AccountingDimension1FieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.AccountingDimension1,
                (StringDisplayValue)overview.ContractFields.AccountingDimension1,
                values);
            CreateValueIfNeeded(
                settings.ContractFieldsSettings.AccountingDimension2FieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.AccountingDimension2,
                (StringDisplayValue)overview.ContractFields.AccountingDimension2,
                values);
            CreateValueIfNeeded(
                settings.ContractFieldsSettings.AccountingDimension3FieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.AccountingDimension3,
                (StringDisplayValue)overview.ContractFields.AccountingDimension3,
                values);
            CreateValueIfNeeded(
                settings.ContractFieldsSettings.AccountingDimension4FieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.AccountingDimension4,
                (StringDisplayValue)overview.ContractFields.AccountingDimension4,
                values);
            CreateValueIfNeeded(
                settings.ContractFieldsSettings.AccountingDimension5FieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContractFields.AccountingDimension5,
                (StringDisplayValue)overview.ContractFields.AccountingDimension5,
                values);

            CreateValueIfNeeded(
                settings.OtherFieldsSettings.InfoFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.OtherFields.Info,
                (StringDisplayValue)overview.OtherFields.Info,
                values);

            CreateValueIfNeeded(
                settings.ContactInformationFieldsSettings.UserIdFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContactInformationFields.UserId,
                (StringDisplayValue)overview.ContactInformationFields.UserId,
                values);

            CreateValueIfNeeded(
                settings.OrganizationFieldsSettings.DepartmentFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.OrganizationFields.Department,
                (StringDisplayValue)overview.OrganizationFields.DepartmentName,
                values);
            CreateValueIfNeeded(
                settings.OrganizationFieldsSettings.DomainFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.OrganizationFields.Domain,
                (StringDisplayValue)overview.OrganizationFields.DomainName,
                values);
            CreateValueIfNeeded(
                settings.OrganizationFieldsSettings.UnitFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.OrganizationFields.Unit,
                (StringDisplayValue)overview.OrganizationFields.UnitName,
                values);

            CreateValueIfNeeded(
                settings.PlaceFieldsSettings.RoomFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.PlaceFields.Room,
                (StringDisplayValue)overview.PlaceFields.RoomName,
                values);
            CreateValueIfNeeded(
                settings.PlaceFieldsSettings.AddressFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.PlaceFields.Address,
                (StringDisplayValue)overview.PlaceFields.Address,
                values);
            CreateValueIfNeeded(
                settings.PlaceFieldsSettings.PostalCodeFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.PlaceFields.PostalCode,
                (StringDisplayValue)overview.PlaceFields.PostalCode,
                values);
            CreateValueIfNeeded(
                settings.PlaceFieldsSettings.PostalAddressFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.PlaceFields.PostalAddress,
                (StringDisplayValue)overview.PlaceFields.PostalAddress,
                values);
            CreateValueIfNeeded(
                settings.PlaceFieldsSettings.PlaceFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.PlaceFields.Location,
                (StringDisplayValue)overview.PlaceFields.Location,
                values);
            CreateValueIfNeeded(
                settings.PlaceFieldsSettings.Place2FieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.PlaceFields.Location2,
                (StringDisplayValue)overview.PlaceFields.Location2,
                values);

            CreateValueIfNeeded(
                settings.ContactFieldsSettings.NameFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContactFields.Name,
                (StringDisplayValue)overview.ContactFields.Name,
                values);
            CreateValueIfNeeded(
                settings.ContactFieldsSettings.PhoneFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContactFields.Phone,
                (StringDisplayValue)overview.ContactFields.Phone,
                values);
            CreateValueIfNeeded(
                settings.ContactFieldsSettings.EmailFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.ContactFields.Email,
                (StringDisplayValue)overview.ContactFields.Email,
                values);

            CreateValueIfNeeded(
                settings.StateFieldsSettings.StateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.StateFields.State,
                (ComputerStatusesDisplayValue)overview.StateFields.State,
                values);
            CreateValueIfNeeded(
                settings.StateFieldsSettings.StolenFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.StateFields.Stolen,
                (BooleanDisplayValue)overview.StateFields.Stolen,
                values);
            CreateValueIfNeeded(
                settings.StateFieldsSettings.ReplacedWithFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.StateFields.Replaced,
                (StringDisplayValue)overview.StateFields.Replaced,
                values);
            CreateValueIfNeeded(
                settings.StateFieldsSettings.SendBackFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.StateFields.SendBack,
                (BooleanDisplayValue)overview.StateFields.SendBack,
                values);
            CreateValueIfNeeded(
                settings.StateFieldsSettings.ScrapDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.StateFields.ScrapDate,
                (DateTimeDisplayValue)overview.StateFields.ScrapDate,
                values);

            CreateValueIfNeeded(
                settings.DateFieldsSettings.CreatedDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.DateFields.CreatedDate,
                (DateTimeDisplayValue)overview.CreatedDate,
                values);
            CreateValueIfNeeded(
                settings.DateFieldsSettings.ChangedDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.DateFields.ChangedDate,
                (DateTimeDisplayValue)overview.ChangedDate,
                values);
            CreateValueIfNeeded(
                settings.DateFieldsSettings.SyncChangedDateSetting,
                BusinessData.Enums.Inventory.Fields.Computer.DateFields.SynchronizeDate,
                (DateTimeDisplayValue)overview.DateFields.SynchronizeDate,
                values);
            CreateValueIfNeeded(
                settings.DateFieldsSettings.ScanDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.DateFields.ScanDate,
                (DateTimeDisplayValue)overview.DateFields.ScanDate,
                values);
            CreateValueIfNeeded(
                settings.DateFieldsSettings.PathDirectoryFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.DateFields.PathDirectory,
                (StringDisplayValue)overview.DateFields.PathDirectory,
                values);

            return values;
        }

        private static void CreateHeaderIfNeeded(
            FieldSettingOverview setting,
            string fieldName,
            List<GridColumnHeaderModel> headers)
        {
            if (!setting.IsShow)
            {
                return;
            }

            var header = new GridColumnHeaderModel(fieldName, setting.Caption);
            headers.Add(header);
        }

        private static void CreateValueIfNeeded(
            FieldSettingOverview setting,
            string fieldName,
            DisplayValue value,
            List<NewGridRowCellValueModel> values)
        {
            if (!setting.IsShow)
            {
                return;
            }

            var fieldValue = new NewGridRowCellValueModel(fieldName, value);
            values.Add(fieldValue);
        }
    }
}