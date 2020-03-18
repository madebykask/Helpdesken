namespace DH.Helpdesk.Web.Areas.Inventory.Models
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Printer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Server;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.PrinterFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ServerFieldSettings;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport;
    using DH.Helpdesk.Services.DisplayValues;
    using DH.Helpdesk.Services.Response.Inventory;
    using DH.Helpdesk.Web.Enums.Inventory;
    using DH.Helpdesk.Web.Models.Shared;

    public sealed class InventoryGridModel
    {
        public InventoryGridModel(
            List<GridColumnHeaderModel> headers,
            List<InventoryOverviewModel> inventories,
            int currentMode,
            SortFieldModel sortField)
        {
            this.Headers = headers;
            this.Inventories = inventories;
            this.CurrentMode = currentMode;
            this.SortField = sortField;
        }

        [NotNull]
        public List<GridColumnHeaderModel> Headers { get; set; }

        [NotNull]
        public List<InventoryOverviewModel> Inventories { get; set; }

        public int CurrentMode { get; set; }

        public string CurrentModeName { get; set; }

        public SortFieldModel SortField { get; set; }

        public static InventoryGridModel BuildModel(
            List<ComputerOverview> modelList,
            ComputerFieldsSettingsOverview settings,
            SortFieldModel sortField)
        {
            List<InventoryOverviewModel> overviews = modelList.Select(c => CreateComputerOverview(c, settings)).ToList();
            var headers = GetComputerHeaders(settings);
            return new InventoryGridModel(headers, overviews, (int)CurrentModes.Workstations, sortField);
        }

        public static InventoryGridModel BuildModel(
            ServerOverview[] modelList,
            ServerFieldsSettingsOverview settings,
            SortFieldModel sortField)
        {
            var overviews = modelList.Select(c => CreateServerOverview(c, settings)).ToList();
            var headers = GetServerHeaders(settings);
            return new InventoryGridModel(headers, overviews, (int)CurrentModes.Servers, sortField);
        }

        public static InventoryGridModel BuildModel(
            List<PrinterOverview> modelList,
            PrinterFieldsSettingsOverview settings,
            SortFieldModel sortField)
        {
            List<InventoryOverviewModel> overviews = modelList.Select(c => CreatePrinterOverview(c, settings)).ToList();
            List<InventoryOverviewModel> sortedOverviews = SortGrid(sortField, overviews);

            var headers = GetPrinterHeaders(settings);

            return new InventoryGridModel(headers, sortedOverviews, (int)CurrentModes.Printers, sortField);
        }

        public static InventoryGridModel BuildModel(
            InventoriesOverviewResponse response,
            InventoryFieldSettingsOverviewResponse settings,
            int inventoryTypeId,
            SortFieldModel sortField)
        {
            List<InventoryOverviewModel> overviews =
                response.Overviews.Select(
                    c =>
                    CreateInventoryOverview(
                        c,
                        response.DynamicData,
                        settings.InventoryFieldSettingsOverview,
                        settings.InventoryDynamicFieldSettingOverviews)).ToList();
            List<InventoryOverviewModel> sortedOverviews = SortGrid(sortField, overviews);

            var headers = GetInventoryHeaders(
                settings.InventoryFieldSettingsOverview,
                settings.InventoryDynamicFieldSettingOverviews);

            return new InventoryGridModel(headers, sortedOverviews, inventoryTypeId, sortField);
        }

        public static List<InventoryGridModel> BuildModels(
            InventoryOverviewResponseWithType response,
            InventoriesFieldSettingsOverviewResponse settings)
        {
            var inventoryGridModels = new List<InventoryGridModel>();

            foreach (var item in response.Overviews)
            {
                var setting =
                    settings.InventoryFieldSettingsOverviews.Single(x => x.InventoryTypeId == item.InventoryTypeId);

                var dynamicSettings =
                    settings.InventoryDynamicFieldSettingOverviews.FirstOrDefault(
                        x => x.InventoryTypeId == item.InventoryTypeId);

                var overviews =
                    item.InventoryOverviews.Select(
                        c =>
                        CreateInventoryOverview(
                            c,
                            response.DynamicData,
                            setting.InventoryFieldSettingsOverview,
                            dynamicSettings != null
                                ? dynamicSettings.InventoryDynamicFieldSettingOverviews
                                : new List<InventoryDynamicFieldSettingOverview>())).ToList();

                var headers = GetInventoryHeaders(
                    setting.InventoryFieldSettingsOverview,
                    dynamicSettings != null
                        ? dynamicSettings.InventoryDynamicFieldSettingOverviews
                        : new List<InventoryDynamicFieldSettingOverview>());

                var inventoryGridModel = new InventoryGridModel(headers, overviews, item.InventoryTypeId, null)
                                             {
                                                 CurrentModeName
                                                     =
                                                     setting
                                                     .InventoryTypeName
                                             };

                inventoryGridModels.Add(inventoryGridModel);
            }

            return inventoryGridModels;
        }

        private static InventoryOverviewModel CreateComputerOverview(
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
                (StringDisplayValue)overview.ContractFields.ContractStatusName,
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
                settings.OrganizationFieldsSettings.RegionFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.OrganizationFields.Region,
                (StringDisplayValue)overview.OrganizationFields.RegionName,
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
                (StringDisplayValue)overview.StateFields.StateName,
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

            return new InventoryOverviewModel(overview.Id, values);
        }

        private static List<GridColumnHeaderModel> GetComputerHeaders(ComputerFieldsSettingsOverview settings)
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
                settings.OrganizationFieldsSettings.RegionFieldSetting,
                BusinessData.Enums.Inventory.Fields.Computer.OrganizationFields.Region,
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
            return headers;
        }

        private static InventoryOverviewModel CreateServerOverview(
            ServerOverview overview,
            ServerFieldsSettingsOverview settings)
        {
            var values = new List<NewGridRowCellValueModel>();

            CreateValueIfNeeded(
                settings.GeneralFieldsSettings.NameFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.GeneralFields.Name,
                (StringDisplayValue)overview.GeneralFields.Name,
                values);
            CreateValueIfNeeded(
                settings.GeneralFieldsSettings.ManufacturerFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.GeneralFields.Manufacturer,
                (StringDisplayValue)overview.GeneralFields.Manufacturer,
                values);
            CreateValueIfNeeded(
                settings.GeneralFieldsSettings.DescriptionFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.GeneralFields.Description,
                (StringDisplayValue)overview.GeneralFields.Description,
                values);
            CreateValueIfNeeded(
                settings.GeneralFieldsSettings.ModelFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.GeneralFields.Model,
                (StringDisplayValue)overview.GeneralFields.Model,
                values);
            CreateValueIfNeeded(
                settings.GeneralFieldsSettings.SerialNumberFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.GeneralFields.SerialNumber,
                (StringDisplayValue)overview.GeneralFields.SerialNumber,
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
                settings.StorageFieldsSettings.CapasityFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.StorageFields.Capasity,
                (StringDisplayValue)overview.StorageFields.Capasity,
                values);

            CreateValueIfNeeded(
                settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.CommunicationFields.NetworkAdapter,
                (StringDisplayValue)overview.CommunicationFields.NetworkAdapterName,
                values);
            CreateValueIfNeeded(
                settings.CommunicationFieldsSettings.IPAddressFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.CommunicationFields.IPAddress,
                (StringDisplayValue)overview.CommunicationFields.IPAddress,
                values);
            CreateValueIfNeeded(
                settings.CommunicationFieldsSettings.MacAddressFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.CommunicationFields.MacAddress,
                (StringDisplayValue)overview.CommunicationFields.MacAddress,
                values);

            CreateValueIfNeeded(
                settings.OtherFieldsSettings.InfoFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.OtherFields.Info,
                (StringDisplayValue)overview.OtherFields.Info,
                values);
            CreateValueIfNeeded(
                settings.OtherFieldsSettings.OtherFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.OtherFields.Other,
                (StringDisplayValue)overview.OtherFields.Other,
                values);
            CreateValueIfNeeded(
                settings.OtherFieldsSettings.URLFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.OtherFields.URL,
                (StringDisplayValue)overview.OtherFields.URL,
                values);
            CreateValueIfNeeded(
                settings.OtherFieldsSettings.URL2FieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.OtherFields.URL2,
                (StringDisplayValue)overview.OtherFields.URL2,
                values);
            CreateValueIfNeeded(
                settings.OtherFieldsSettings.OwnerFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.OtherFields.Owner,
                (StringDisplayValue)overview.OtherFields.Owner,
                values);

            CreateValueIfNeeded(
                settings.PlaceFieldsSettings.RoomFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.PlaceFields.Room,
                (StringDisplayValue)overview.PlaceFields.RoomName,
                values);
            CreateValueIfNeeded(
                settings.PlaceFieldsSettings.LocationFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.PlaceFields.Location,
                (StringDisplayValue)overview.PlaceFields.Location,
                values);

            CreateValueIfNeeded(
                settings.StateFieldsSettings.CreatedDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.StateFields.CreatedDate,
                (DateTimeDisplayValue)overview.CreatedDate,
                values);
            CreateValueIfNeeded(
                settings.StateFieldsSettings.ChangedDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.StateFields.ChangedDate,
                (DateTimeDisplayValue)overview.ChangedDate,
                values);
            CreateValueIfNeeded(
                settings.StateFieldsSettings.SyncChangeDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.StateFields.SyncChangeDate,
                (DateTimeDisplayValue)overview.StateFields.SyncChangeDate,
                values);

            return new InventoryOverviewModel(overview.Id, values);
        }

        private static List<GridColumnHeaderModel> GetServerHeaders(ServerFieldsSettingsOverview settings)
        {
            var headers = new List<GridColumnHeaderModel>();

            CreateHeaderIfNeeded(
                settings.GeneralFieldsSettings.NameFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.GeneralFields.Name,
                headers);
            CreateHeaderIfNeeded(
                settings.GeneralFieldsSettings.ManufacturerFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.GeneralFields.Manufacturer,
                headers);
            CreateHeaderIfNeeded(
                settings.GeneralFieldsSettings.DescriptionFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.GeneralFields.Description,
                headers);
            CreateHeaderIfNeeded(
                settings.GeneralFieldsSettings.ModelFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.GeneralFields.Model,
                headers);
            CreateHeaderIfNeeded(
                settings.GeneralFieldsSettings.SerialNumberFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.GeneralFields.SerialNumber,
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
                settings.StorageFieldsSettings.CapasityFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.StorageFields.Capasity,
                headers);

            CreateHeaderIfNeeded(
                settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.CommunicationFields.NetworkAdapter,
                headers);
            CreateHeaderIfNeeded(
                settings.CommunicationFieldsSettings.IPAddressFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.CommunicationFields.IPAddress,
                headers);
            CreateHeaderIfNeeded(
                settings.CommunicationFieldsSettings.MacAddressFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.CommunicationFields.MacAddress,
                headers);

            CreateHeaderIfNeeded(
                settings.OtherFieldsSettings.InfoFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.OtherFields.Info,
                headers);
            CreateHeaderIfNeeded(
                settings.OtherFieldsSettings.OtherFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.OtherFields.Other,
                headers);
            CreateHeaderIfNeeded(
                settings.OtherFieldsSettings.URLFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.OtherFields.URL,
                headers);
            CreateHeaderIfNeeded(
                settings.OtherFieldsSettings.URL2FieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.OtherFields.URL2,
                headers);
            CreateHeaderIfNeeded(
                settings.OtherFieldsSettings.OwnerFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.OtherFields.Owner,
                headers);

            CreateHeaderIfNeeded(
                settings.PlaceFieldsSettings.RoomFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.PlaceFields.Room,
                headers);
            CreateHeaderIfNeeded(
                settings.PlaceFieldsSettings.LocationFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.PlaceFields.Location,
                headers);

            CreateHeaderIfNeeded(
                settings.StateFieldsSettings.CreatedDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.StateFields.CreatedDate,
                headers);
            CreateHeaderIfNeeded(
                settings.StateFieldsSettings.ChangedDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.StateFields.ChangedDate,
                headers);
            CreateHeaderIfNeeded(
                settings.StateFieldsSettings.SyncChangeDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.StateFields.SyncChangeDate,
                headers);

            return headers;
        }

        private static InventoryOverviewModel CreatePrinterOverview(
            PrinterOverview overview,
            PrinterFieldsSettingsOverview settings)
        {
            var values = new List<NewGridRowCellValueModel>();

            CreateValueIfNeeded(
                settings.GeneralFieldsSettings.NameFieldSetting,
                BusinessData.Enums.Inventory.Fields.Printer.GeneralFields.Name,
                (StringDisplayValue)overview.GeneralFields.Name,
                values);
            CreateValueIfNeeded(
                settings.GeneralFieldsSettings.ManufacturerFieldSetting,
                BusinessData.Enums.Inventory.Fields.Printer.GeneralFields.Manufacturer,
                (StringDisplayValue)overview.GeneralFields.Manufacturer,
                values);
            CreateValueIfNeeded(
                settings.GeneralFieldsSettings.ModelFieldSetting,
                BusinessData.Enums.Inventory.Fields.Printer.GeneralFields.Model,
                (StringDisplayValue)overview.GeneralFields.Model,
                values);
            CreateValueIfNeeded(
                settings.GeneralFieldsSettings.SerialNumberFieldSetting,
                BusinessData.Enums.Inventory.Fields.Printer.GeneralFields.SerialNumber,
                (StringDisplayValue)overview.GeneralFields.SerialNumber,
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
                settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting,
                BusinessData.Enums.Inventory.Fields.Printer.CommunicationFields.NetworkAdapter,
                (StringDisplayValue)overview.CommunicationFields.NetworkAdapterName,
                values);
            CreateValueIfNeeded(
                settings.CommunicationFieldsSettings.IPAddressFieldSetting,
                BusinessData.Enums.Inventory.Fields.Printer.CommunicationFields.IPAddress,
                (StringDisplayValue)overview.CommunicationFields.IPAddress,
                values);
            CreateValueIfNeeded(
                settings.CommunicationFieldsSettings.MacAddressFieldSetting,
                BusinessData.Enums.Inventory.Fields.Printer.CommunicationFields.MacAddress,
                (StringDisplayValue)overview.CommunicationFields.MacAddress,
                values);

            CreateValueIfNeeded(
                settings.OtherFieldsSettings.NumberOfTraysFieldSetting,
                BusinessData.Enums.Inventory.Fields.Printer.OtherFields.NumberOfTrays,
                (StringDisplayValue)overview.OtherFields.NumberOfTrays,
                values);
            CreateValueIfNeeded(
                settings.OtherFieldsSettings.DriverFieldSetting,
                BusinessData.Enums.Inventory.Fields.Printer.OtherFields.Driver,
                (StringDisplayValue)overview.OtherFields.Driver,
                values);
            CreateValueIfNeeded(
                settings.OtherFieldsSettings.InfoFieldSetting,
                BusinessData.Enums.Inventory.Fields.Printer.OtherFields.Info,
                (StringDisplayValue)overview.OtherFields.Info,
                values);
            CreateValueIfNeeded(
                settings.OtherFieldsSettings.URLFieldSetting,
                BusinessData.Enums.Inventory.Fields.Printer.OtherFields.URL,
                (StringDisplayValue)overview.OtherFields.URL,
                values);

            CreateValueIfNeeded(
                settings.PlaceFieldsSettings.RoomFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.PlaceFields.Room,
                (StringDisplayValue)overview.PlaceFields.RoomName,
                values);
            CreateValueIfNeeded(
                settings.PlaceFieldsSettings.LocationFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.PlaceFields.Location,
                (StringDisplayValue)overview.PlaceFields.Location,
                values);
            CreateValueIfNeeded(
                settings.StateFieldsSettings.CreatedDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.StateFields.CreatedDate,
                (DateTimeDisplayValue)overview.CreatedDate,
                values);
            CreateValueIfNeeded(
                settings.StateFieldsSettings.ChangedDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.StateFields.ChangedDate,
                (DateTimeDisplayValue)overview.ChangedDate,
                values);
            CreateValueIfNeeded(
                settings.StateFieldsSettings.SyncChangedDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.StateFields.SyncChangeDate,
                (DateTimeDisplayValue)overview.SyncDate,
                values);
            
            return new InventoryOverviewModel(overview.Id, values);
        }

        private static List<GridColumnHeaderModel> GetPrinterHeaders(PrinterFieldsSettingsOverview settings)
        {
            var headers = new List<GridColumnHeaderModel>();

            CreateHeaderIfNeeded(
                settings.GeneralFieldsSettings.NameFieldSetting,
                BusinessData.Enums.Inventory.Fields.Printer.GeneralFields.Name,
                headers);
            CreateHeaderIfNeeded(
                settings.GeneralFieldsSettings.ManufacturerFieldSetting,
                BusinessData.Enums.Inventory.Fields.Printer.GeneralFields.Manufacturer,
                headers);
            CreateHeaderIfNeeded(
                settings.GeneralFieldsSettings.ModelFieldSetting,
                BusinessData.Enums.Inventory.Fields.Printer.GeneralFields.Model,
                headers);
            CreateHeaderIfNeeded(
                settings.GeneralFieldsSettings.SerialNumberFieldSetting,
                BusinessData.Enums.Inventory.Fields.Printer.GeneralFields.SerialNumber,
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
                settings.CommunicationFieldsSettings.NetworkAdapterFieldSetting,
                BusinessData.Enums.Inventory.Fields.Printer.CommunicationFields.NetworkAdapter,
                headers);
            CreateHeaderIfNeeded(
                settings.CommunicationFieldsSettings.IPAddressFieldSetting,
                BusinessData.Enums.Inventory.Fields.Printer.CommunicationFields.IPAddress,
                headers);
            CreateHeaderIfNeeded(
                settings.CommunicationFieldsSettings.MacAddressFieldSetting,
                BusinessData.Enums.Inventory.Fields.Printer.CommunicationFields.MacAddress,
                headers);

            CreateHeaderIfNeeded(
                settings.OtherFieldsSettings.NumberOfTraysFieldSetting,
                BusinessData.Enums.Inventory.Fields.Printer.OtherFields.NumberOfTrays,
                headers);
            CreateHeaderIfNeeded(
                settings.OtherFieldsSettings.DriverFieldSetting,
                BusinessData.Enums.Inventory.Fields.Printer.OtherFields.Driver,
                headers);
            CreateHeaderIfNeeded(
                settings.OtherFieldsSettings.InfoFieldSetting,
                BusinessData.Enums.Inventory.Fields.Printer.OtherFields.Info,
                headers);
            CreateHeaderIfNeeded(
                settings.OtherFieldsSettings.URLFieldSetting,
                BusinessData.Enums.Inventory.Fields.Printer.OtherFields.URL,
                headers);

            CreateHeaderIfNeeded(
                settings.PlaceFieldsSettings.RoomFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.PlaceFields.Room,
                headers);
            CreateHeaderIfNeeded(
                settings.PlaceFieldsSettings.LocationFieldSetting,
                BusinessData.Enums.Inventory.Fields.Shared.PlaceFields.Location,
                headers);

            CreateHeaderIfNeeded(
                settings.StateFieldsSettings.CreatedDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.StateFields.CreatedDate,
                headers);

            CreateHeaderIfNeeded(
                settings.StateFieldsSettings.ChangedDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.StateFields.ChangedDate,
                headers);

            CreateHeaderIfNeeded(
                settings.StateFieldsSettings.SyncChangedDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Server.StateFields.SyncChangeDate,
                headers);

            return headers;
        }

        private static InventoryOverviewModel CreateInventoryOverview(
            InventoryOverview overview,
            List<InventoryValue> dynamicData,
            InventoryFieldSettingsOverview settings,
            List<InventoryDynamicFieldSettingOverview> dynamicFieldSettings)
        {
            var values = new List<NewGridRowCellValueModel>();

            CreateValueIfNeeded(
                settings.DefaultSettings.DepartmentFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.Department,
                (StringDisplayValue)overview.DepartmentName,
                values);
            CreateValueIfNeeded(
                settings.DefaultSettings.NameFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.Name,
                (StringDisplayValue)overview.Name,
                values);
            CreateValueIfNeeded(
                settings.DefaultSettings.ModelFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.Model,
                (StringDisplayValue)overview.Model,
                values);
            CreateValueIfNeeded(
                settings.DefaultSettings.ManufacturerFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.Manufacturer,
                (StringDisplayValue)overview.Manufacturer,
                values);
            CreateValueIfNeeded(
                settings.DefaultSettings.SerialNumberFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.SerialNumber,
                (StringDisplayValue)overview.SerialNumber,
                values);
            CreateValueIfNeeded(
                settings.DefaultSettings.TheftMarkFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.TheftMark,
                (StringDisplayValue)overview.TheftMark,
                values);
            CreateValueIfNeeded(
                settings.DefaultSettings.BarCodeFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.BarCode,
                (StringDisplayValue)overview.BarCode,
                values);
            CreateValueIfNeeded(
                settings.DefaultSettings.PurchaseDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.PurchaseDate,
                (DateTimeDisplayValue)overview.PurchaseDate,
                values);
            CreateValueIfNeeded(
                settings.DefaultSettings.PlaceFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.Place,
                (StringDisplayValue)overview.RoomName,
                values);
            CreateValueIfNeeded(
                settings.DefaultSettings.WorkstationFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.Workstation,
                (StringDisplayValue)overview.WorkstationName,
                values);
            CreateValueIfNeeded(
                settings.DefaultSettings.InfoFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.Info,
                (StringDisplayValue)overview.Info,
                values);
            CreateValueIfNeeded(
                settings.DefaultSettings.CreatedDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.CreatedDate,
                (DateTimeDisplayValue)overview.CreatedDate,
                values);
            CreateValueIfNeeded(
                settings.DefaultSettings.ChangedDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.ChangedDate,
                (DateTimeDisplayValue)overview.ChangedDate,
                values);

            CreateValueIfNeeded(
                settings.DefaultSettings.SyncDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.SyncChangedDate,
                (DateTimeDisplayValue)overview.SyncChangedDate,
                values);

            var dynamicValues = dynamicData.Where(x => x.InventoryId == overview.Id).ToList();

            if (dynamicValues.Any())
            {
                values.AddRange(
                    dynamicValues.Select(
                        item =>
                        item.FieldType == FieldTypes.Bool
                            ? new NewGridRowCellValueModel(
                                  item.InventoryTypePropertyId.ToString(CultureInfo.InvariantCulture),
                                  (BooleanDisplayValue)(item.Value == "1"))
                            : new NewGridRowCellValueModel(
                                  item.InventoryTypePropertyId.ToString(CultureInfo.InvariantCulture),
                                  (StringDisplayValue)item.Value)));
            }

            // todo need to find best solution, maybe, put this logic in the view
            if (dynamicFieldSettings.Any())
            {
                var existedColumns = dynamicValues.Select(x => x.InventoryTypePropertyId).ToList();
                var dynamicColumns =
                    dynamicFieldSettings.Where(x => !existedColumns.Contains(x.Id)).Select(x => x.Id).Distinct();
                values.AddRange(
                    dynamicColumns.Select(
                        x =>
                        new NewGridRowCellValueModel(
                            x.ToString(CultureInfo.InvariantCulture),
                            new StringDisplayValue(string.Empty))));
            }

            return new InventoryOverviewModel(overview.Id, values);
        }

        private static List<GridColumnHeaderModel> GetInventoryHeaders(
            InventoryFieldSettingsOverview settings,
            List<InventoryDynamicFieldSettingOverview> dynamicFieldSettings)
        {
            var headers = new List<GridColumnHeaderModel>();

            CreateHeaderIfNeeded(
                settings.DefaultSettings.DepartmentFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.Department,
                headers);
            CreateHeaderIfNeeded(
                settings.DefaultSettings.NameFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.Name,
                headers);
            CreateHeaderIfNeeded(
                settings.DefaultSettings.ModelFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.Model,
                headers);
            CreateHeaderIfNeeded(
                settings.DefaultSettings.ManufacturerFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.Manufacturer,
                headers);
            CreateHeaderIfNeeded(
                settings.DefaultSettings.SerialNumberFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.SerialNumber,
                headers);
            CreateHeaderIfNeeded(
                settings.DefaultSettings.TheftMarkFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.TheftMark,
                headers);
            CreateHeaderIfNeeded(
                settings.DefaultSettings.BarCodeFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.BarCode,
                headers);
            CreateHeaderIfNeeded(
                settings.DefaultSettings.PurchaseDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.PurchaseDate,
                headers);
            CreateHeaderIfNeeded(
                settings.DefaultSettings.PlaceFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.Place,
                headers);
            CreateHeaderIfNeeded(
                settings.DefaultSettings.WorkstationFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.Workstation,
                headers);
            CreateHeaderIfNeeded(
                settings.DefaultSettings.InfoFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.Info,
                headers);
            CreateHeaderIfNeeded(
                settings.DefaultSettings.CreatedDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.CreatedDate,
                headers);
            CreateHeaderIfNeeded(
                settings.DefaultSettings.ChangedDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.ChangedDate,
                headers);
            CreateHeaderIfNeeded(
                settings.DefaultSettings.SyncDateFieldSetting,
                BusinessData.Enums.Inventory.Fields.Inventory.InventoryFields.SyncChangedDate,
                headers);

            headers.AddRange(
                dynamicFieldSettings.Select(
                    item => new GridColumnHeaderModel(item.Id.ToString(CultureInfo.InvariantCulture), item.Caption)));

            return headers;
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

        private static List<InventoryOverviewModel> SortGrid(
            SortFieldModel sortFieldModel,
            List<InventoryOverviewModel> models)
        {
            if (sortFieldModel != null && sortFieldModel.SortBy.HasValue)
            {
                var displayValueComparer = new RowCellModelComparer(sortFieldModel.Name);
                var sortedModels = new List<InventoryOverviewModel>();

                switch (sortFieldModel.SortBy)
                {
                    case SortBy.Ascending:
                        sortedModels = models.OrderBy(x => x, displayValueComparer).ToList();
                        break;
                    case SortBy.Descending:
                        sortedModels = models.OrderByDescending(x => x, displayValueComparer).ToList();
                        break;
                }

                return sortedModels;
            }

            return models;
        }

        private class RowCellModelComparer : IComparer<IRow<ICell>>
        {
            private readonly string fieldName;

            public RowCellModelComparer(string fieldName)
            {
                this.fieldName = fieldName;
            }

            public int Compare(IRow<ICell> x, IRow<ICell> y)
            {
                ICell valueX = x.Fields.SingleOrDefault(m => m.FieldName.Equals(this.fieldName));
                ICell valueY = y.Fields.SingleOrDefault(m => m.FieldName.Equals(this.fieldName));

                if (valueX == null && valueY == null)
                {
                    return 0;
                }

                if (valueX != null && valueY == null)
                {
                    return 1;
                }

                if (valueX == null)
                {
                    return -1;
                }

                DisplayValue displayValueX = valueX.Value;
                DisplayValue displayValueY = valueY.Value;

                var displayValueStringX = displayValueX as DisplayValue<string>;
                if (displayValueStringX != null)
                {
                    var displayValueStringY = displayValueY as DisplayValue<string>;

                    if (displayValueStringY != null)
                    {
                        return string.Compare(
                            displayValueStringX.Value,
                            displayValueStringY.Value,
                            StringComparison.InvariantCultureIgnoreCase);
                    }
                }

                var displayValueIntX = displayValueX as DisplayValue<int?>;
                if (displayValueIntX != null)
                {
                    var displayValueIntY = displayValueY as DisplayValue<int?>;

                    if (displayValueIntY != null)
                    {
                        if (!displayValueIntX.Value.HasValue && !displayValueIntY.Value.HasValue)
                        {
                            return 0;
                        }

                        if (displayValueIntX.Value.HasValue && !displayValueIntY.Value.HasValue)
                        {
                            return 1;
                        }

                        if (!displayValueIntX.Value.HasValue)
                        {
                            return -1;
                        }

                        return displayValueIntX.Value.Value.CompareTo(displayValueIntY.Value.Value);
                    }
                }

                var displayValueDateX = displayValueX as DisplayValue<DateTime?>;
                if (displayValueDateX != null)
                {
                    var displayValueDateY = displayValueY as DisplayValue<DateTime?>;

                    if (displayValueDateY != null)
                    {
                        if (!displayValueDateX.Value.HasValue && !displayValueDateY.Value.HasValue)
                        {
                            return 0;
                        }

                        if (displayValueDateX.Value.HasValue && !displayValueDateY.Value.HasValue)
                        {
                            return 1;
                        }

                        if (!displayValueDateX.Value.HasValue)
                        {
                            return -1;
                        }

                        return displayValueDateX.Value.Value.CompareTo(displayValueDateY.Value.Value);
                    }
                }

                var displayValueBoolX = displayValueX as DisplayValue<bool>;
                if (displayValueBoolX != null)
                {
                    var displayValueBoolY = displayValueY as DisplayValue<bool>;

                    if (displayValueBoolY != null)
                    {
                        return displayValueBoolX.Value.CompareTo(displayValueBoolY.Value);
                    }
                }

                return string.Compare(
                    displayValueX.GetDisplayValue(),
                    displayValueY.GetDisplayValue(),
                    StringComparison.InvariantCultureIgnoreCase);
            }
        }
    }
}