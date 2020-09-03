namespace DH.Helpdesk.Dal.Repositories.Servers.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ServerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.SharedSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ServerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ServerFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ServerSettings;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Dal.Attributes.Inventory;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Enums.Inventory.Server;
    using DH.Helpdesk.Dal.Enums.Inventory.Shared;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.MapperData.Inventory;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.Servers;

    using GeneralFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ServerSettings.GeneralFieldsSettings;
    using OtherFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ServerSettings.OtherFieldsSettings;
    using StateFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ServerSettings.StateFieldsSettings;
    using StorageFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ServerSettings.StorageFieldsSettings;

    public class ServerFieldSettingsRepository : Repository<ServerFieldSettings>, IServerFieldSettingsRepository
    {
        private readonly IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, ServerFieldsSettingsOverview> entityToBusinessModelMapperForOverview;

        private readonly IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperDataForModelEdit>, ServerFieldsSettingsForModelEdit> entityToBusinessModelMapperForModelEdit;

        private readonly IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperData>, ServerFieldsSettings> entityToBusinessModelMapperForEdit;

        private readonly IEntityToBusinessModelMapper<NamedObjectCollection<FieldProcessingSettingMapperData>, ServerFieldsSettingsProcessing> entityToBusinessModelMapperForProcessing;

        public ServerFieldSettingsRepository(
            IDatabaseFactory databaseFactory,
            IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, ServerFieldsSettingsOverview> entityToBusinessModelMapperForOverview,
            IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperDataForModelEdit>, ServerFieldsSettingsForModelEdit> entityToBusinessModelMapperForModelEdit,
            IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperData>, ServerFieldsSettings> entityToBusinessModelMapperForEdit,
            IEntityToBusinessModelMapper<NamedObjectCollection<FieldProcessingSettingMapperData>, ServerFieldsSettingsProcessing> entityToBusinessModelMapperForProcessing)
            : base(databaseFactory)
        {
            this.entityToBusinessModelMapperForOverview = entityToBusinessModelMapperForOverview;
            this.entityToBusinessModelMapperForModelEdit = entityToBusinessModelMapperForModelEdit;
            this.entityToBusinessModelMapperForEdit = entityToBusinessModelMapperForEdit;
            this.entityToBusinessModelMapperForProcessing = entityToBusinessModelMapperForProcessing;
        }

        public void Update(ServerFieldsSettings businessModel)
        {
            var languageTextId = this.GetLanguageTextId(businessModel.LanguageId);
            var fieldSettings = this.GetSettings(businessModel.CustomerId).ToList();
            var fieldSettingCollection = new NamedObjectCollection<ServerFieldSettings>(fieldSettings);
            MapGeneralFieldsSettings(businessModel.GeneralFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapChassisFieldsSettings(businessModel.ChassisFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapInventoringFieldsSettings(businessModel.InventoryFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapOperatingSystemFieldsSettings(businessModel.OperatingSystemFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapProcessorFieldsSettings(businessModel.ProccesorFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapMemoryFieldsSettings(businessModel.MemoryFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapStorageFieldsSettings(businessModel.StorageFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapCommunicationFieldsSettings(businessModel.CommunicationFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapOtherFieldsSettings(businessModel.OtherFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapDocumentFieldsSettings(businessModel.DocumentFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapPlaceFieldsSettings(businessModel.PlaceFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapStateFieldsSettings(businessModel.StateFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
        }

        [CreateMissingServerSettings("customerId")]
        public ServerFieldsSettings GetFieldSettingsForEdit(int customerId, int languageId)
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
                                FieldName = s.ServerField,
                                ShowInList = s.ShowInList,
                                ShowInDetails = s.Show,
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
                                FieldName = s.ServerField,
                                ShowInList = s.ShowInList,
                                ShowInDetails = s.Show,
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
                                FieldName = s.ServerField,
                                ShowInList = s.ShowInList,
                                ShowInDetails = s.Show,
                                Required = s.Required,
                            }).ToList();
                    break;
            }

            var settingCollection = new NamedObjectCollection<FieldSettingMapperData>(mapperData);
            return this.entityToBusinessModelMapperForEdit.Map(settingCollection);
        }

        [CreateMissingServerSettings("customerId")]
        public ServerFieldsSettingsForModelEdit GetFieldSettingsForModelEdit(int customerId, int languageId, bool isReadonly = false)
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
                                FieldName = s.ServerField,
                                Show = s.Show,
                                ReadOnly = isReadonly ? 1 : 0,
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
                                FieldName = s.ServerField,
                                Show = s.Show,
                                ReadOnly = isReadonly ? 1 : 0,
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
                                FieldName = s.ServerField,
                                Show = s.Show,
                                ReadOnly = isReadonly ? 1 : 0,
                                Required = s.Required,
                            }).ToList();
                    break;
            }

            var settingCollection = new NamedObjectCollection<FieldSettingMapperDataForModelEdit>(mapperData);
            return this.entityToBusinessModelMapperForModelEdit.Map(settingCollection);
        }

        [CreateMissingServerSettings("customerId")]
        public ServerFieldsSettingsOverview GetFieldSettingsOverview(int customerId, int languageId)
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
                                FieldName = s.ServerField,
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
                                FieldName = s.ServerField,
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
                                FieldName = s.ServerField,
                                Show = s.ShowInList
                            }).ToList();
                    break;
            }

            var settingCollection = new NamedObjectCollection<FieldOverviewSettingMapperData>(mapperData);
            return this.entityToBusinessModelMapperForOverview.Map(settingCollection);
        }

        [CreateMissingComputerSettings("customerId")]
        public ServerFieldsSettingsProcessing GetFieldSettingsProcessing(int customerId)
        {
            var settings = this.GetSettings(customerId);

            List<FieldProcessingSettingMapperData> mapperData = settings.Select(
                s =>
                new FieldProcessingSettingMapperData
                {
                    FieldName = s.ServerField,
                    Show = s.Show,
                    Required = s.Required
                }).ToList();

            var settingCollection = new NamedObjectCollection<FieldProcessingSettingMapperData>(mapperData);
            return this.entityToBusinessModelMapperForProcessing.Map(settingCollection);
        }

        private static void MapGeneralFieldsSettings(
            GeneralFieldsSettings updatedSettings,
            NamedObjectCollection<ServerFieldSettings> entity,
            string languageTextId,
            DateTime changedDate)
        {
            MapFieldSetting(updatedSettings.NameFieldSetting, entity.FindByName(GeneralFields.Name), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.ManufacturerFieldSetting, entity.FindByName(GeneralFields.Manufacturer), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.DescriptionFieldSetting, entity.FindByName(GeneralFields.Description), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.ModelFieldSetting, entity.FindByName(GeneralFields.Model), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.SerialNumberFieldSetting, entity.FindByName(GeneralFields.SerialNumber), languageTextId, changedDate);
        }

        private static void MapChassisFieldsSettings(
            ChassisFieldsSettings updatedSettings,
            NamedObjectCollection<ServerFieldSettings> entity,
            string languageTextId,
            DateTime changedDate)
        {
            MapFieldSetting(updatedSettings.ChassisFieldSetting, entity.FindByName(ChassisFields.Chassis), languageTextId, changedDate);
        }

        private static void MapInventoringFieldsSettings(
            InventoryFieldsSettings updatedSettings,
            NamedObjectCollection<ServerFieldSettings> entity,
            string languageTextId,
            DateTime changedDate)
        {
            MapFieldSetting(updatedSettings.BarCodeFieldSetting, entity.FindByName(InventoryFields.BarCode), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.PurchaseDateFieldSetting, entity.FindByName(InventoryFields.PurchaseDate), languageTextId, changedDate);
        }

        private static void MapOperatingSystemFieldsSettings(
             OperatingSystemFieldsSettings updatedSettings,
             NamedObjectCollection<ServerFieldSettings> entity,
             string languageTextId,
            DateTime changedDate)
        {
            MapFieldSetting(updatedSettings.OperatingSystemFieldSetting, entity.FindByName(OperatingSystemFields.OperatingSystem), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.VersionFieldSetting, entity.FindByName(OperatingSystemFields.Version), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.ServicePackSystemFieldSetting, entity.FindByName(OperatingSystemFields.ServicePack), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.RegistrationCodeSystemFieldSetting, entity.FindByName(OperatingSystemFields.RegistrationCode), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.ProductKeyFieldSetting, entity.FindByName(OperatingSystemFields.ProductKey), languageTextId, changedDate);
        }

        private static void MapProcessorFieldsSettings(
            ProcessorFieldsSettings updatedSettings,
            NamedObjectCollection<ServerFieldSettings> entity,
            string languageTextId,
            DateTime changedDate)
        {
            MapFieldSetting(updatedSettings.ProccesorFieldSetting, entity.FindByName(ProcessorFields.ProccesorName), languageTextId, changedDate);
        }

        private static void MapMemoryFieldsSettings(
            MemoryFieldsSettings updatedSettings,
            NamedObjectCollection<ServerFieldSettings> entity,
            string languageTextId,
            DateTime changedDate)
        {
            MapFieldSetting(updatedSettings.RAMFieldSetting, entity.FindByName(MemoryFields.RAM), languageTextId, changedDate);
        }

        private static void MapStorageFieldsSettings(
            StorageFieldsSettings updatedSettings,
            NamedObjectCollection<ServerFieldSettings> entity,
            string languageTextId,
            DateTime changedDate)
        {
            MapFieldSetting(updatedSettings.CapasityFieldSetting, entity.FindByName(StorageFields.Capasity), languageTextId, changedDate);
        }

        private static void MapCommunicationFieldsSettings(
            CommunicationFieldsSettings updatedSettings,
            NamedObjectCollection<ServerFieldSettings> entity,
            string languageTextId,
            DateTime changedDate)
        {
            MapFieldSetting(updatedSettings.NetworkAdapterFieldSetting, entity.FindByName(CommunicationFields.NetworkAdapter), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.IPAddressFieldSetting, entity.FindByName(CommunicationFields.IPAddress), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.MacAddressFieldSetting, entity.FindByName(CommunicationFields.MacAddress), languageTextId, changedDate);
        }

        private static void MapOtherFieldsSettings(
            OtherFieldsSettings updatedSettings,
            NamedObjectCollection<ServerFieldSettings> entity,
            string languageTextId,
            DateTime changedDate)
        {
            MapFieldSetting(updatedSettings.InfoFieldSetting, entity.FindByName(OtherFields.Info), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.OtherFieldSetting, entity.FindByName(OtherFields.Other), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.URLFieldSetting, entity.FindByName(OtherFields.URL), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.URL2FieldSetting, entity.FindByName(OtherFields.URL2), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.OwnerFieldSetting, entity.FindByName(OtherFields.Owner), languageTextId, changedDate);
        }

        private static void MapDocumentFieldsSettings(
            BusinessData.Models.Inventory.Edit.Settings.ServerSettings.DocumentFieldsSettings updatedSettings,
            NamedObjectCollection<ServerFieldSettings> entity,
            string languageTextId,
            DateTime changedDate)
        {
            MapFieldSetting(updatedSettings.DocuemntFieldSetting, entity.FindByName(DocumentFields.Document), languageTextId, changedDate);            
        }

        private static void MapPlaceFieldsSettings(
            PlaceFieldsSettings updatedSettings,
            NamedObjectCollection<ServerFieldSettings> entity,
            string languageTextId,
            DateTime changedDate)
        {
            MapFieldSetting(updatedSettings.RoomFieldSetting, entity.FindByName(PlaceFields.Room), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.LocationFieldSetting, entity.FindByName(PlaceFields.Location), languageTextId, changedDate);
        }

        private static void MapStateFieldsSettings(
            StateFieldsSettings updatedSettings,
            NamedObjectCollection<ServerFieldSettings> entity,
            string languageTextId,
            DateTime changedDate)
        {
            MapFieldSetting(updatedSettings.CreatedDateFieldSetting, entity.FindByName(StateFields.CreatedDate), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.ChangedDateFieldSetting, entity.FindByName(StateFields.ChangedDate), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.SyncChangeDateFieldSetting, entity.FindByName(StateFields.SyncChangeDate), languageTextId, changedDate);
        }

        private static void MapFieldSetting(
            FieldSetting updatedSetting,
            ServerFieldSettings fieldSetting,
            string languageTextId,
            DateTime changedDate)
        {
            fieldSetting.ChangedDate = changedDate;
            fieldSetting.Required = updatedSetting.IsRequired.ToInt();
            fieldSetting.Show = updatedSetting.ShowInDetails.ToInt();
            fieldSetting.ShowInList = updatedSetting.ShowInList.ToInt();

            switch (languageTextId)
            {
                case LanguageTextId.Swedish:
                    fieldSetting.Label = updatedSetting.Caption;
                    break;
                case LanguageTextId.English:
                    fieldSetting.Label_ENG = updatedSetting.Caption;
                    break;
                default:
                    fieldSetting.Label_ENG = updatedSetting.Caption;
                    break;
            }
        }

        private IQueryable<ServerFieldSettings> GetSettings(int customerId)
        {
            return this.DbSet.Where(x => x.Customer_Id == customerId);
        }

        private string GetLanguageTextId(int languageId)
        {
            return this.DbContext.Languages.Find(languageId).LanguageID;
        }
    }
}