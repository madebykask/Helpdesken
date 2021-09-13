using DH.Helpdesk.Dal.EntityConfigurations.CaseDocument;
using DH.Helpdesk.Dal.Enums.Notifiers;

namespace DH.Helpdesk.Dal.Repositories.Computers.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Dal.Attributes.Inventory;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Enums.Inventory.Computer;
    using DH.Helpdesk.Dal.Enums.Inventory.Shared;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.MapperData.Inventory;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.Computers;

    using ChassisFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.ChassisFieldsSettings;
    using CommunicationFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.CommunicationFieldsSettings;
    using ContactFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.ContactFieldsSettings;
    using ContactInformationFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.ContactInformationFieldsSettings;
    using ContractFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.ContractFieldsSettings;
    using DateFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.DateFieldsSettings;
    using FieldSetting = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.FieldSetting;
    using GraphicsFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.GraphicsFieldsSettings;
    using InventoryFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.InventoryFieldsSettings;
    using MemoryFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.MemoryFieldsSettings;
    using OperatingSystemFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.OperatingSystemFieldsSettings;
    using OrganizationFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.OrganizationFieldsSettings;
    using OtherFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.OtherFieldsSettings;
    using PlaceFields = DH.Helpdesk.Dal.Enums.Inventory.Computer.PlaceFields;
    using PlaceFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.PlaceFieldsSettings;
    using ProcessorFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.ProcessorFieldsSettings;
    using SoundFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.SoundFieldsSettings;
    using StateFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.StateFieldsSettings;
    using WorkstationFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.WorkstationFieldsSettings;

    public class ComputerFieldSettingsRepository : Repository<ComputerFieldSettings>, IComputerFieldSettingsRepository
    {
        private readonly IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, ComputerFieldsSettingsOverviewForFilter> entityToBusinessModelMapperForFilter;

        private readonly IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, ComputerFieldsSettingsOverviewForShortInfo> entityToBusinessModelMapperForShortInfo;

        private readonly IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, ComputerFieldsSettingsOverview> entityToBusinessModelMapperForOverview;

        private readonly IEntityToBusinessModelMapper<NamedObjectCollection<FieldProcessingSettingMapperData>, ComputerFieldsSettingsProcessing> entityToBusinessModelMapperForProcessing;

        private readonly IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperDataForModelEdit>, ComputerFieldsSettingsForModelEdit> entityToBusinessModelMapperForModelEdit;

        private readonly IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperData>, ComputerFieldsSettings> entityToBusinessModelMapperForEdit;

        public ComputerFieldSettingsRepository(
            IDatabaseFactory databaseFactory,
            IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, ComputerFieldsSettingsOverviewForFilter> entityToBusinessModelMapperForFilter,
            IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, ComputerFieldsSettingsOverviewForShortInfo> entityToBusinessModelMapperForShortInfo,
            IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, ComputerFieldsSettingsOverview> entityToBusinessModelMapperForOverview,
            IEntityToBusinessModelMapper<NamedObjectCollection<FieldProcessingSettingMapperData>, ComputerFieldsSettingsProcessing> entityToBusinessModelMapperForProcessing,
            IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperDataForModelEdit>, ComputerFieldsSettingsForModelEdit> entityToBusinessModelMapperForModelEdit,
            IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperData>, ComputerFieldsSettings> entityToBusinessModelMapperForEdit)
            : base(databaseFactory)
        {
            this.entityToBusinessModelMapperForFilter = entityToBusinessModelMapperForFilter;
            this.entityToBusinessModelMapperForShortInfo = entityToBusinessModelMapperForShortInfo;
            this.entityToBusinessModelMapperForOverview = entityToBusinessModelMapperForOverview;
            this.entityToBusinessModelMapperForProcessing = entityToBusinessModelMapperForProcessing;
            this.entityToBusinessModelMapperForModelEdit = entityToBusinessModelMapperForModelEdit;
            this.entityToBusinessModelMapperForEdit = entityToBusinessModelMapperForEdit;
        }

        public void Update(ComputerFieldsSettings businessModel)
        {
            var languageTextId = this.GetLanguageTextId(businessModel.LanguageId);
            var fieldSettings = this.GetSettings(businessModel.CustomerId).ToList();
            var fieldSettingCollection = new NamedObjectCollection<ComputerFieldSettings>(fieldSettings);
            MapWorkstationFieldsSettings(businessModel.WorkstationFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapChassisFieldsSettings(businessModel.ChassisFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapInventoringFieldsSettings(businessModel.InventoryFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapOperatingSystemFieldsSettings(businessModel.OperatingSystemFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapProcessorFieldsSettings(businessModel.ProccesorFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapMemoryFieldsSettings(businessModel.MemoryFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapCommunicationFieldsSettings(businessModel.CommunicationFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapGraphicsFieldsSettings(businessModel.GraphicsFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapSoundFieldsSettings(businessModel.SoundFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapContractFieldsSettings(businessModel.ContractFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapOtherFieldsSettings(businessModel.OtherFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapContactInformationFieldsSettings(businessModel.ContactInformationFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapOrganizationFieldsSettings(businessModel.OrganizationFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapPlaceFieldsSettings(businessModel.PlaceFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapContactFieldsSettings(businessModel.ContactFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapStateFieldsSettings(businessModel.StateFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapDateFieldsSettings(businessModel.DateFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
        }

        [CreateMissingComputerSettings("customerId")]
        public ComputerFieldsSettings GetFieldSettingsForEdit(int customerId, int languageId)
        {
            var languageTextId = this.GetLanguageTextId(languageId);
            var settings = this.GetSettings(customerId);
            List<FieldSettingMapperData> mapperData;

            switch (languageTextId)
            {
                case LanguageTextId.Swedish:
                    mapperData =
                        settings.Select(
                            s =>
                            new FieldSettingMapperData
                            {
                                Caption = s.Label,
                                FieldName = s.ComputerField,
                                ShowInList = s.ShowInList,
                                ShowInDetails = s.Show,
                                ReadOnly = s.ReadOnly,
                                Copy = s.Copy,
                                Required = s.Required
                            }).ToList();
                    break;
                case LanguageTextId.English:
                    mapperData =
                        settings.Select(
                            s =>
                            new FieldSettingMapperData
                            {
                                Caption = s.Label_ENG,
                                FieldName = s.ComputerField,
                                ShowInList = s.ShowInList,
                                ShowInDetails = s.Show,
                                ReadOnly = s.ReadOnly,
                                Copy = s.Copy,
                                Required = s.Required,
                            }).ToList();
                    break;
                default:
                    mapperData =
                        settings.Select(
                            s =>
                            new FieldSettingMapperData
                            {
                                Caption = s.Label_ENG,
                                FieldName = s.ComputerField,
                                ShowInList = s.ShowInList,
                                ShowInDetails = s.Show,
                                ReadOnly = s.ReadOnly,
                                Copy = s.Copy,
                                Required = s.Required,
                            }).ToList();
                    break;
                    //throw new ArgumentOutOfRangeException("languageId");
            }

            var settingCollection = new NamedObjectCollection<FieldSettingMapperData>(mapperData);
            return this.entityToBusinessModelMapperForEdit.Map(settingCollection);
        }

        [CreateMissingComputerSettings("customerId")]
        public ComputerFieldsSettingsForModelEdit GetFieldSettingsForModelEdit(int customerId, int languageId, bool isReadonly = false, bool isCopy = false)
        {
            var languageTextId = this.GetLanguageTextId(languageId);
            var settings = this.GetSettings(customerId);
            List<FieldSettingMapperDataForModelEdit> mapperData;

            switch (languageTextId)
            {
                case LanguageTextId.Swedish:
                    mapperData =
                        settings.Select(
                            s =>
                            new FieldSettingMapperDataForModelEdit
                            {
                                Caption = s.Label,
                                FieldName = s.ComputerField,
                                Show = s.Show,
                                ReadOnly = isReadonly ? 1 : s.ReadOnly,
                                Copy = !isCopy ? 0 : s.Copy,
                                Required = s.Required
                            }).ToList();
                    break;
                case LanguageTextId.English:
                    mapperData =
                        settings.Select(
                            s =>
                            new FieldSettingMapperDataForModelEdit
                            {
                                Caption = s.Label_ENG,
                                FieldName = s.ComputerField,
                                Show = s.Show,
                                ReadOnly = isReadonly ? 1 : s.ReadOnly,
                                Copy = !isCopy ? 0 : s.Copy,
                                Required = s.Required,
                            }).ToList();
                    break;
                default:
                    mapperData =
                       settings.Select(
                           s =>
                           new FieldSettingMapperDataForModelEdit
                           {
                               Caption = s.Label_ENG,
                               FieldName = s.ComputerField,
                               Show = s.Show,
                               ReadOnly = isReadonly ? 1 : s.ReadOnly,
                               Copy = !isCopy ? 0 : s.Copy,
                               Required = s.Required,
                           }).ToList();
                    break;
            }

            var settingCollection = new NamedObjectCollection<FieldSettingMapperDataForModelEdit>(mapperData);

            //add notifiers field settings
            var notifierFieldSettings = GetNotifierFieldSettings(customerId, languageId);
            if (notifierFieldSettings.Any())
                settingCollection.AddRange(notifierFieldSettings);

            return entityToBusinessModelMapperForModelEdit.Map(settingCollection);
        }

        private IList<FieldSettingMapperDataForModelEdit> GetNotifierFieldSettings(int customerId, int langId)
        {
            var settings = DbContext.ComputerUserFieldSettings.Where(x => x.Customer_Id == customerId).ToList();

            var res = new List<FieldSettingMapperDataForModelEdit>()
            {
                MapNotifierFieldSettingsToEditSettings(ContactInformationFields.FirstName, GeneralField.FirstName, GeneralFieldLabel.UserId, settings, langId),
                MapNotifierFieldSettingsToEditSettings(ContactInformationFields.LastName, GeneralField.LastName, GeneralFieldLabel.LastName, settings, langId),
                MapNotifierFieldSettingsToEditSettings(ContactInformationFields.Region, OrganizationField.Region, OrganizationFieldLabel.Region, settings, langId),
                MapNotifierFieldSettingsToEditSettings(ContactInformationFields.Department, OrganizationField.Department, OrganizationFieldLabel.Department, settings, langId),
                MapNotifierFieldSettingsToEditSettings(ContactInformationFields.Unit, OrganizationField.Unit, OrganizationFieldLabel.Unit, settings, langId)
            };

            return res;
        }

        private FieldSettingMapperDataForModelEdit MapNotifierFieldSettingsToEditSettings(string contactField, string notifierField, string label, IList<ComputerUserFieldSettings> settings, int langId)
        {
            FieldSettingMapperDataForModelEdit editSettings;
            var fieldSettings = settings.SingleOrDefault(s => s.ComputerUserField.Equals(notifierField, StringComparison.OrdinalIgnoreCase));
            if (fieldSettings != null)
            {
                var translation = DbContext.ComputerUserFieldSettingsLanguages.SingleOrDefault(
                        t => t.ComputerUserFieldSettings_Id == fieldSettings.Id && t.Language_Id == langId);

                editSettings = new FieldSettingMapperDataForModelEdit
                {
                    Caption = translation == null ? label : translation.Label,
                    FieldName = contactField,
                    Show = fieldSettings.Show,
                    Required = fieldSettings.Required
                };
            }
            else
            {
                editSettings = new FieldSettingMapperDataForModelEdit()
                {
                    Caption = label,
                    FieldName = contactField,
                };
            }

            return editSettings;
        }

        [CreateMissingComputerSettings("customerId")]
        public ComputerFieldsSettingsOverview GetFieldSettingsOverview(int customerId, int languageId)
        {
            var languageTextId = this.GetLanguageTextId(languageId);
            var settings = this.GetSettings(customerId);
            List<FieldOverviewSettingMapperData> mapperData;

            switch (languageTextId)
            {
                case LanguageTextId.Swedish:
                    mapperData =
                        settings.Select(
                            s =>
                            new FieldOverviewSettingMapperData
                            {
                                Caption = s.Label,
                                FieldName = s.ComputerField,
                                Show = s.ShowInList
                            }).ToList();
                    break;
                case LanguageTextId.English:
                    mapperData =
                        settings.Select(
                            s =>
                            new FieldOverviewSettingMapperData
                            {
                                Caption = s.Label_ENG,
                                FieldName = s.ComputerField,
                                Show = s.ShowInList
                            }).ToList();
                    break;
                default:
                    mapperData =
                        settings.Select(
                            s =>
                            new FieldOverviewSettingMapperData
                            {
                                Caption = s.Label_ENG,
                                FieldName = s.ComputerField,
                                Show = s.ShowInList
                            }).ToList();
                    break;
            }

            var settingCollection = new NamedObjectCollection<FieldOverviewSettingMapperData>(mapperData);
            return this.entityToBusinessModelMapperForOverview.Map(settingCollection);
        }

        [CreateMissingComputerSettings("customerId")]
        public ComputerFieldsSettingsProcessing GetFieldSettingsProcessing(int customerId)
        {
            var settings = this.GetSettings(customerId);

            var mapperData = settings.Select(
                s =>
                new FieldProcessingSettingMapperData
                    {
                        FieldName = s.ComputerField,
                        Show = s.Show,
                        ReadOnly = s.ReadOnly,
                        Copy = s.Copy,
                        Required = s.Required
                    }).ToList();

            var settingCollection = new NamedObjectCollection<FieldProcessingSettingMapperData>(mapperData);
            return this.entityToBusinessModelMapperForProcessing.Map(settingCollection);
        }

        [CreateMissingComputerSettings("customerId")]
        public ComputerFieldsSettingsOverviewForFilter GetFieldSettingsOverviewForFilter(int customerId, int languageId)
        {
            var mapperData = this.GetFieldOverviewSettingMapperData(customerId, languageId);

            var settingCollection = new NamedObjectCollection<FieldOverviewSettingMapperData>(mapperData);
            return this.entityToBusinessModelMapperForFilter.Map(settingCollection);
        }

        [CreateMissingComputerSettings("customerId")]
        public ComputerFieldsSettingsOverviewForShortInfo GetFieldSettingsOverviewForShortInfo(int customerId, int languageId)
        {
            var mapperData = this.GetFieldOverviewSettingMapperData(customerId, languageId);

            var settingCollection = new NamedObjectCollection<FieldOverviewSettingMapperData>(mapperData);
            return this.entityToBusinessModelMapperForShortInfo.Map(settingCollection);
        }

        private static void MapWorkstationFieldsSettings(
            WorkstationFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId,
            DateTime changeDate)
        {
            MapFieldSetting(updatedSettings.ComputerNameFieldSetting, entity.FindByName(WorkstationFields.Name), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.ManufacturerFieldSetting, entity.FindByName(WorkstationFields.Manufacturer), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.ComputerModelFieldSetting, entity.FindByName(WorkstationFields.Model), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.SerialNumberFieldSetting, entity.FindByName(WorkstationFields.SerialNumber), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.BIOSDateFieldSetting, entity.FindByName(WorkstationFields.BIOSDate), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.BIOSVersionFieldSetting, entity.FindByName(WorkstationFields.BIOSVersion), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.TheftmarkFieldSetting, entity.FindByName(WorkstationFields.Theftmark), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.CarePackNumberFieldSetting, entity.FindByName(WorkstationFields.CarePackNumber), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.ComputerTypeFieldSetting, entity.FindByName(WorkstationFields.ComputerType), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.LocationFieldSetting, entity.FindByName(WorkstationFields.Location), languageTextId, changeDate);
        }

        private static void MapChassisFieldsSettings(
            ChassisFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId,
            DateTime changeDate)
        {
            MapFieldSetting(updatedSettings.ChassisFieldSetting, entity.FindByName(ChassisFields.Chassis), languageTextId, changeDate);
        }

        private static void MapInventoringFieldsSettings(
            InventoryFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId,
            DateTime changeDate)
        {
            MapFieldSetting(updatedSettings.BarCodeFieldSetting, entity.FindByName(InventoryFields.BarCode), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.PurchaseDateFieldSetting, entity.FindByName(InventoryFields.PurchaseDate), languageTextId, changeDate);
        }

        private static void MapOperatingSystemFieldsSettings(
            OperatingSystemFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId,
            DateTime changeDate)
        {
            MapFieldSetting(updatedSettings.OperatingSystemFieldSetting, entity.FindByName(OperatingSystemFields.OS), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.VersionFieldSetting, entity.FindByName(OperatingSystemFields.Version), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.ServicePackSystemFieldSetting, entity.FindByName(OperatingSystemFields.ServicePack), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.RegistrationCodeSystemFieldSetting, entity.FindByName(OperatingSystemFields.RegistrationCode), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.ProductKeyFieldSetting, entity.FindByName(OperatingSystemFields.ProductKey), languageTextId, changeDate);
        }

        private static void MapProcessorFieldsSettings(
            ProcessorFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId,
            DateTime changeDate)
        {
            MapFieldSetting(updatedSettings.ProccesorFieldSetting, entity.FindByName(ProcessorFields.ProccesorName), languageTextId, changeDate);
        }

        private static void MapMemoryFieldsSettings(
            MemoryFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId,
            DateTime changeDate)
        {
            MapFieldSetting(updatedSettings.RAMFieldSetting, entity.FindByName(MemoryFields.RAM), languageTextId, changeDate);
        }

        private static void MapCommunicationFieldsSettings(
            CommunicationFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId,
            DateTime changeDate)
        {
            MapFieldSetting(updatedSettings.NetworkAdapterFieldSetting, entity.FindByName(CommunicationFields.NetworkAdapter), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.IPAddressFieldSetting, entity.FindByName(CommunicationFields.IPAddress), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.MacAddressFieldSetting, entity.FindByName(CommunicationFields.MacAddress), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.RASFieldSetting, entity.FindByName(CommunicationFields.RAS), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.NovellClientFieldSetting, entity.FindByName(CommunicationFields.NovellClient), languageTextId, changeDate);
        }

        private static void MapGraphicsFieldsSettings(
            GraphicsFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId,
            DateTime changeDate)
        {
            MapFieldSetting(updatedSettings.VideoCardFieldSetting, entity.FindByName(GraphicsFields.VideoCard), languageTextId, changeDate);
        }

        private static void MapSoundFieldsSettings(
            SoundFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId,
            DateTime changeDate)
        {
            MapFieldSetting(updatedSettings.SoundCardFieldSetting, entity.FindByName(SoundFields.SoundCard), languageTextId, changeDate);
        }

        private static void MapContractFieldsSettings(
            ContractFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId,
            DateTime changeDate)
        {
            MapFieldSetting(updatedSettings.ContractStatusFieldSetting, entity.FindByName(ContractFields.ContractStatusName), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.ContractNumberFieldSetting, entity.FindByName(ContractFields.ContractNumber), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.ContractStartDateFieldSetting, entity.FindByName(ContractFields.ContractStartDate), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.ContractEndDateFieldSetting, entity.FindByName(ContractFields.ContractEndDate), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.PurchasePriceFieldSetting, entity.FindByName(ContractFields.PurchasePrice), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.AccountingDimension1FieldSetting, entity.FindByName(ContractFields.AccountingDimension1), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.AccountingDimension2FieldSetting, entity.FindByName(ContractFields.AccountingDimension2), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.AccountingDimension3FieldSetting, entity.FindByName(ContractFields.AccountingDimension3), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.AccountingDimension4FieldSetting, entity.FindByName(ContractFields.AccountingDimension4), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.AccountingDimension5FieldSetting, entity.FindByName(ContractFields.AccountingDimension5), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.DocumentFieldSetting, entity.FindByName(ContractFields.Document), languageTextId, changeDate);
        }

        private static void MapOtherFieldsSettings(
            OtherFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId,
            DateTime changeDate)
        {
            MapFieldSetting(updatedSettings.InfoFieldSetting, entity.FindByName(OtherFields.Info), languageTextId, changeDate);
        }

        private static void MapContactInformationFieldsSettings(
            ContactInformationFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId,
            DateTime changeDate)
        {
            MapFieldSetting(updatedSettings.UserIdFieldSetting, entity.FindByName(ContactInformationFields.UserId), languageTextId, changeDate);
        }

        private static void MapOrganizationFieldsSettings(
            OrganizationFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId,
            DateTime changeDate)
        {
            MapFieldSetting(updatedSettings.RegionFieldSetting, entity.FindByName(OrganizationFields.Region), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.DepartmentFieldSetting, entity.FindByName(OrganizationFields.Department), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.DomainFieldSetting, entity.FindByName(OrganizationFields.Domain), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.UnitFieldSetting, entity.FindByName(OrganizationFields.Unit), languageTextId, changeDate);
        }

        private static void MapPlaceFieldsSettings(
            PlaceFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId,
            DateTime changeDate)
        {
            MapFieldSetting(updatedSettings.RoomFieldSetting, entity.FindByName(PlaceFields.Room), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.BuildingFieldSetting, entity.FindByName(PlaceFields.Building), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.FloorFieldSetting, entity.FindByName(PlaceFields.Floor), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.AddressFieldSetting, entity.FindByName(PlaceFields.Address), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.PostalCodeFieldSetting, entity.FindByName(PlaceFields.PostalCode), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.PostalAddressFieldSetting, entity.FindByName(PlaceFields.PostalAddress), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.PlaceFieldSetting, entity.FindByName(PlaceFields.Location), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.Place2FieldSetting, entity.FindByName(PlaceFields.Location2), languageTextId, changeDate);
        }

        private static void MapContactFieldsSettings(
            ContactFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId,
            DateTime changeDate)
        {
            MapFieldSetting(updatedSettings.NameFieldSetting, entity.FindByName(ContactFields.Name), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.PhoneFieldSetting, entity.FindByName(ContactFields.Phone), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.EmailFieldSetting, entity.FindByName(ContactFields.Email), languageTextId, changeDate);
        }

        private static void MapStateFieldsSettings(
            StateFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId,
            DateTime changeDate)
        {
            MapFieldSetting(updatedSettings.StateFieldSetting, entity.FindByName(StateFields.State), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.StolenFieldSetting, entity.FindByName(StateFields.Stolen), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.ReplacedWithFieldSetting, entity.FindByName(StateFields.Replaced), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.SendBackFieldSetting, entity.FindByName(StateFields.SendBack), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.ScrapDateFieldSetting, entity.FindByName(StateFields.ScrapDate), languageTextId, changeDate);
        }

        private static void MapDateFieldsSettings(
            DateFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId,
            DateTime changeDate)
        {
            MapFieldSetting(updatedSettings.CreatedDateFieldSetting, entity.FindByName(DateFields.CreatedDate), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.ChangedDateFieldSetting, entity.FindByName(DateFields.ChangedDate), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.SyncChangedDateSetting, entity.FindByName(DateFields.SynchronizeDate), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.ScanDateFieldSetting, entity.FindByName(DateFields.ScanDate), languageTextId, changeDate);
            MapFieldSetting(updatedSettings.PathDirectoryFieldSetting, entity.FindByName(DateFields.PathDirectory), languageTextId, changeDate);
        }

        private static void MapFieldSetting(
            FieldSetting updatedSetting,
            ComputerFieldSettings fieldSetting,
            string languageTextId,
            DateTime changeDate)
        {
            fieldSetting.ChangedDate = changeDate;
            fieldSetting.Required = updatedSetting.IsRequired.ToInt();
            fieldSetting.ReadOnly = updatedSetting.IsReadOnly.ToInt();
            fieldSetting.Copy = updatedSetting.IsCopy.ToInt();
            fieldSetting.Show = updatedSetting.ShowInDetails.ToInt();
            fieldSetting.ShowInList = updatedSetting.ShowInList.ToInt();

            switch (languageTextId)
            {
                case LanguageTextId.Swedish:
                    fieldSetting.Label = updatedSetting.Caption;
                    break;                
                default:
                    fieldSetting.Label_ENG = updatedSetting.Caption;
                    break;
                    
            }
        }

        private IQueryable<ComputerFieldSettings> GetSettings(int customerId)
        {
            return this.DbSet.Where(x => x.Customer_Id == customerId);
        }

        private string GetLanguageTextId(int languageId)
        {
            return this.DbContext.Languages.Find(languageId).LanguageID;
        }

        private List<FieldOverviewSettingMapperData> GetFieldOverviewSettingMapperData(int customerId, int languageId)
        {
            var languageTextId = this.GetLanguageTextId(languageId);
            var settings = this.GetSettings(customerId);
            List<FieldOverviewSettingMapperData> mapperData;

            switch (languageTextId)
            {
                case LanguageTextId.Swedish:
                    mapperData =
                        settings.Select(
                            s =>
                            new FieldOverviewSettingMapperData
                            {
                                Caption = s.Label,
                                FieldName = s.ComputerField,
                                Show = s.Show
                            }).ToList();
                    break;
                case LanguageTextId.English:
                    mapperData =
                        settings.Select(
                            s =>
                            new FieldOverviewSettingMapperData
                            {
                                Caption = s.Label_ENG,
                                FieldName = s.ComputerField,
                                Show = s.Show
                            }).ToList();
                    break;
                default:
                    mapperData =
                        settings.Select(
                            s =>
                            new FieldOverviewSettingMapperData
                            {
                                Caption = s.Label_ENG,
                                FieldName = s.ComputerField,
                                Show = s.Show
                            }).ToList();
                    break;
            }

            return mapperData;
        }
    }
}