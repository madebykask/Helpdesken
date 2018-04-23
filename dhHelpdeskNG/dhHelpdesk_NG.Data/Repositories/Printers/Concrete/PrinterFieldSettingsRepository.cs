namespace DH.Helpdesk.Dal.Repositories.Printers.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.SharedSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.PrinterSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.PrinterFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.PrinterSettings;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Dal.Attributes.Inventory;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Enums.Inventory.Printer;
    using DH.Helpdesk.Dal.Enums.Inventory.Shared;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.MapperData.Inventory;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.Printers;

    using GeneralFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings.GeneralFieldsSettings;
    using OrganizationFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings.OrganizationFieldsSettings;
    using OtherFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings.OtherFieldsSettings;
    using StateFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings.StateFieldsSettings;

    public class PrinterFieldSettingsRepository : Repository<PrinterFieldSettings>, IPrinterFieldSettingsRepository
    {
        private readonly IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, PrinterFieldsSettingsOverview> entityToBusinessModelMapperForOverview;

        private readonly IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperDataForModelEdit>, PrinterFieldsSettingsForModelEdit> entityToBusinessModelMapperForModelEdit;

        private readonly IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperData>, PrinterFieldsSettings> entityToBusinessModelMapperForEdit;
        
        private readonly IEntityToBusinessModelMapper<NamedObjectCollection<FieldProcessingSettingMapperData>, PrinterFieldsSettingsProcessing> entityToBusinessModelMapperForProcessing;

        public PrinterFieldSettingsRepository(
            IDatabaseFactory databaseFactory,
            IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, PrinterFieldsSettingsOverview> entityToBusinessModelMapperForOverview,
            IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperDataForModelEdit>, PrinterFieldsSettingsForModelEdit> entityToBusinessModelMapperForModelEdit,
            IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperData>, PrinterFieldsSettings> entityToBusinessModelMapperForEdit,
            IEntityToBusinessModelMapper<NamedObjectCollection<FieldProcessingSettingMapperData>, PrinterFieldsSettingsProcessing> entityToBusinessModelMapperForProcessing)
            : base(databaseFactory)
        {
            this.entityToBusinessModelMapperForOverview = entityToBusinessModelMapperForOverview;
            this.entityToBusinessModelMapperForModelEdit = entityToBusinessModelMapperForModelEdit;
            this.entityToBusinessModelMapperForEdit = entityToBusinessModelMapperForEdit;
            this.entityToBusinessModelMapperForProcessing = entityToBusinessModelMapperForProcessing;
        }

        public void Update(PrinterFieldsSettings businessModel)
        {
            var languageTextId = this.GetLanguageTextId(businessModel.LanguageId);
            var fieldSettings = this.GetSettings(businessModel.CustomerId).ToList();
            var fieldSettingCollection = new NamedObjectCollection<PrinterFieldSettings>(fieldSettings);
            MapGeneralFieldsSettings(businessModel.GeneralFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapInventoringFieldsSettings(businessModel.InventoryFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapCommunicationFieldsSettings(businessModel.CommunicationFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapOtherFieldsSettings(businessModel.OtherFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapOrganizationFieldsSettings(businessModel.OrganizationFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapPlaceFieldsSettings(businessModel.PlaceFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
            MapStateFieldsSettings(businessModel.StateFieldsSettings, fieldSettingCollection, languageTextId, businessModel.ChangedDate);
        }

        [CreateMissingPrinterSettings("customerId")]
        public PrinterFieldsSettings GetFieldSettingsForEdit(int customerId, int languageId)
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
                                FieldName = s.PrinterField,
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
                                FieldName = s.PrinterField,
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
                                FieldName = s.PrinterField,
                                ShowInList = s.ShowInList,
                                ShowInDetails = s.Show,
                                Required = s.Required,
                            }).ToList();
                    break;
            }

            var settingCollection = new NamedObjectCollection<FieldSettingMapperData>(mapperData);
            return this.entityToBusinessModelMapperForEdit.Map(settingCollection);
        }

        [CreateMissingPrinterSettings("customerId")]
        public PrinterFieldsSettingsForModelEdit GetFieldSettingsForModelEdit(int customerId, int languageId, bool isReadonly = false)
        {
            var languageTextId = this.GetLanguageTextId(languageId);
            var settings = this.GetSettings(customerId);
            List<FieldSettingMapperDataForModelEdit> mapperData;

            switch (languageTextId)
            {
                case LanguageTextId.Swedish:
                    mapperData = settings.Select(s =>
                            new FieldSettingMapperDataForModelEdit
                            {
                                Caption = s.Label,
                                FieldName = s.PrinterField,
                                Show = s.Show,
                                ReadOnly = isReadonly ? 1 : 0,
                                Required = s.Required
                            }).ToList();
                    break;
                case LanguageTextId.English:
                    mapperData = settings.Select(s =>
                            new FieldSettingMapperDataForModelEdit
                            {
                                Caption = s.Label_ENG,
                                FieldName = s.PrinterField,
                                Show = s.Show,
                                ReadOnly = isReadonly ? 1 : 0,
                                Required = s.Required,
                            }).ToList();
                    break;
                default:
                    mapperData = settings.Select(s =>
                             new FieldSettingMapperDataForModelEdit
                             {
                                 Caption = s.Label_ENG,
                                 FieldName = s.PrinterField,
                                 Show = s.Show,
                                 ReadOnly = isReadonly ? 1 : 0,
                                 Required = s.Required,
                             }).ToList();
                    break;
            }

            var settingCollection = new NamedObjectCollection<FieldSettingMapperDataForModelEdit>(mapperData);
            return this.entityToBusinessModelMapperForModelEdit.Map(settingCollection);
        }

        [CreateMissingPrinterSettings("customerId")]
        public PrinterFieldsSettingsOverview GetFieldSettingsOverview(int customerId, int languageId)
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
                                FieldName = s.PrinterField,
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
                                FieldName = s.PrinterField,
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
                                FieldName = s.PrinterField,
                                Show = s.ShowInList
                            }).ToList();
                    break;
            }

            var settingCollection = new NamedObjectCollection<FieldOverviewSettingMapperData>(mapperData);
            return this.entityToBusinessModelMapperForOverview.Map(settingCollection);
        }

        [CreateMissingPrinterSettings("customerId")]
        public PrinterFieldsSettingsOverviewForFilter GetFieldSettingsOverviewForFilter(int customerId, int languageId)
        {
            var languageTextId = this.GetLanguageTextId(languageId);
            var settings = this.GetSettings(customerId).Where(x => x.PrinterField == OrganizationFields.Department);
            dynamic mapperData;

            switch (languageTextId)
            {
                case LanguageTextId.Swedish:
                    mapperData =
                        settings.Select(
                            s =>
                            new
                                {
                                    Caption = s.Label,
                                    s.Show
                                }).Single();
                    break;
                case LanguageTextId.English:
                    mapperData =
                        settings.Select(
                            s =>
                            new
                                {
                                    Caption = s.Label_ENG,
                                    s.Show
                                }).Single();
                    break;
                default:
                    mapperData =
                        settings.Select(
                            s =>
                            new
                                {
                                    Caption = s.Label_ENG,
                                    s.Show
                                }).Single();
                    break;
            }

            var setting = new FieldSettingOverview(mapperData.Show == 1, mapperData.Caption);
            var overview = new PrinterFieldsSettingsOverviewForFilter(setting);

            return overview;
        }

        [CreateMissingComputerSettings("customerId")]
        public PrinterFieldsSettingsProcessing GetFieldSettingsProcessing(int customerId)
        {
            var settings = this.GetSettings(customerId);

            List<FieldProcessingSettingMapperData> mapperData = settings.Select(
                s =>
                new FieldProcessingSettingMapperData
                {
                    FieldName = s.PrinterField,
                    Show = s.Show,
                    Required = s.Required
                }).ToList();

            var settingCollection = new NamedObjectCollection<FieldProcessingSettingMapperData>(mapperData);
            return this.entityToBusinessModelMapperForProcessing.Map(settingCollection);
        }

        private static void MapGeneralFieldsSettings(
            GeneralFieldsSettings updatedSettings,
            NamedObjectCollection<PrinterFieldSettings> entity,
            string languageTextId,
            DateTime changedDate)
        {
            MapFieldSetting(updatedSettings.NameFieldSetting, entity.FindByName(GeneralFields.Name), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.ManufacturerFieldSetting, entity.FindByName(GeneralFields.Manufacturer), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.ModelFieldSetting, entity.FindByName(GeneralFields.Model), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.SerialNumberFieldSetting, entity.FindByName(GeneralFields.SerialNumber), languageTextId, changedDate);
        }

        private static void MapInventoringFieldsSettings(
            InventoryFieldsSettings updatedSettings,
            NamedObjectCollection<PrinterFieldSettings> entity,
            string languageTextId,
            DateTime changedDate)
        {
            MapFieldSetting(updatedSettings.BarCodeFieldSetting, entity.FindByName(InventoryFields.BarCode), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.PurchaseDateFieldSetting, entity.FindByName(InventoryFields.PurchaseDate), languageTextId, changedDate);
        }

        private static void MapCommunicationFieldsSettings(
            CommunicationFieldsSettings updatedSettings,
            NamedObjectCollection<PrinterFieldSettings> entity,
            string languageTextId,
            DateTime changedDate)
        {
            MapFieldSetting(updatedSettings.NetworkAdapterFieldSetting, entity.FindByName(CommunicationFields.NetworkAdapter), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.IPAddressFieldSetting, entity.FindByName(CommunicationFields.IPAddress), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.MacAddressFieldSetting, entity.FindByName(CommunicationFields.MacAddress), languageTextId, changedDate);
        }

        private static void MapOtherFieldsSettings(
            OtherFieldsSettings updatedSettings,
            NamedObjectCollection<PrinterFieldSettings> entity,
            string languageTextId,
            DateTime changedDate)
        {
            MapFieldSetting(updatedSettings.NumberOfTraysFieldSetting, entity.FindByName(OtherFields.NumberOfTrays), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.DriverFieldSetting, entity.FindByName(OtherFields.Driver), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.InfoFieldSetting, entity.FindByName(OtherFields.Info), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.URLFieldSetting, entity.FindByName(OtherFields.URL), languageTextId, changedDate);
        }

        private static void MapOrganizationFieldsSettings(
            OrganizationFieldsSettings updatedSettings,
            NamedObjectCollection<PrinterFieldSettings> entity,
            string languageTextId,
            DateTime changedDate)
        {
            MapFieldSetting(updatedSettings.DepartmentFieldSetting, entity.FindByName(OrganizationFields.Department), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.UnitFieldSetting, entity.FindByName(OrganizationFields.Unit), languageTextId, changedDate);
        }

        private static void MapPlaceFieldsSettings(
            PlaceFieldsSettings updatedSettings,
            NamedObjectCollection<PrinterFieldSettings> entity,
            string languageTextId,
            DateTime changedDate)
        {
            MapFieldSetting(updatedSettings.RoomFieldSetting, entity.FindByName(PlaceFields.Room), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.LocationFieldSetting, entity.FindByName(PlaceFields.Location), languageTextId, changedDate);
        }

        private static void MapStateFieldsSettings(
            StateFieldsSettings updatedSettings,
            NamedObjectCollection<PrinterFieldSettings> entity,
            string languageTextId,
            DateTime changedDate)
        {
            MapFieldSetting(updatedSettings.CreatedDateFieldSetting, entity.FindByName(StateFields.CreatedDate), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.ChangedDateFieldSetting, entity.FindByName(StateFields.ChangedDate), languageTextId, changedDate);
            MapFieldSetting(updatedSettings.SyncDateFieldSetting, entity.FindByName(StateFields.SyncDate), languageTextId, changedDate);
        }

        private static void MapFieldSetting(
            FieldSetting updatedSetting,
            PrinterFieldSettings fieldSetting,
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

        private IQueryable<PrinterFieldSettings> GetSettings(int customerId)
        {
            return this.DbSet.Where(x => x.Customer_Id == customerId);
        }

        private string GetLanguageTextId(int languageId)
        {
            return this.DbContext.Languages.Find(languageId).LanguageID;
        }
    }
}