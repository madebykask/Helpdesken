namespace DH.Helpdesk.Dal.Repositories.Computers.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.SharedSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Enums.Inventory.Computer;
    using DH.Helpdesk.Dal.Enums.Inventory.Shared;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.MapperData.Inventory;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.Computers;

    using CommunicationFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.CommunicationFieldsSettings;
    using ContactFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.ContactFieldsSettings;
    using ContactInformationFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.ContactInformationFieldsSettings;
    using ContractFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.ContractFieldsSettings;
    using DateFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.DateFieldsSettings;
    using GraphicsFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.GraphicsFieldsSettings;
    using OrganizationFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.OrganizationFieldsSettings;
    using OtherFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.OtherFieldsSettings;
    using PlaceFields = DH.Helpdesk.Dal.Enums.Inventory.Computer.PlaceFields;
    using PlaceFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.PlaceFieldsSettings;
    using SoundFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.SoundFieldsSettings;
    using StateFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.StateFieldsSettings;
    using WorkstationFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings.WorkstationFieldsSettings;

    public class ComputerFieldSettingsRepository : Repository<Domain.Computers.ComputerFieldSettings>, IComputerFieldSettingsRepository
    {
        private readonly IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, ComputerFieldsSettingsOverviewForFilter> entityToBusinessModelMapperForFilter;

        private readonly IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, ComputerFieldsSettingsOverview> entityToBusinessModelMapperForOverview;

        private readonly IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperDataForModelEdit>, ComputerFieldsSettingsForModelEdit> entityToBusinessModelMapperForModelEdit;

        private readonly IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperData>, ComputerFieldsSettings> entityToBusinessModelMapperForEdit;

        public ComputerFieldSettingsRepository(
            IDatabaseFactory databaseFactory,
            IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, ComputerFieldsSettingsOverviewForFilter> entityToBusinessModelMapperForFilter,
            IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, ComputerFieldsSettingsOverview> entityToBusinessModelMapperForOverview,
            IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperDataForModelEdit>, ComputerFieldsSettingsForModelEdit> entityToBusinessModelMapperForModelEdit,
            IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperData>, ComputerFieldsSettings> entityToBusinessModelMapperForEdit)
            : base(databaseFactory)
        {
            this.entityToBusinessModelMapperForFilter = entityToBusinessModelMapperForFilter;
            this.entityToBusinessModelMapperForOverview = entityToBusinessModelMapperForOverview;
            this.entityToBusinessModelMapperForModelEdit = entityToBusinessModelMapperForModelEdit;
            this.entityToBusinessModelMapperForEdit = entityToBusinessModelMapperForEdit;
        }

        public void Update(ComputerFieldsSettings businessModel)
        {
            var languageTextId = this.GetLanguageTextId(businessModel.LanguageId);
            var fieldSettings = this.GetSettings(businessModel.CustomerId).ToList();
            var fieldSettingCollection = new NamedObjectCollection<ComputerFieldSettings>(fieldSettings);
            MapWorkstationFieldsSettings(businessModel.WorkstationFieldsSettings, fieldSettingCollection, languageTextId);
            MapChassisFieldsSettings(businessModel.ChassisFieldsSettings, fieldSettingCollection, languageTextId);
            MapInventoringFieldsSettings(businessModel.InventoryFieldsSettings, fieldSettingCollection, languageTextId);
            MapOperatingSystemFieldsSettings(businessModel.OperatingSystemFieldsSettings, fieldSettingCollection, languageTextId);
            MapProcessorFieldsSettings(businessModel.ProccesorFieldsSettings, fieldSettingCollection, languageTextId);
            MapMemoryFieldsSettings(businessModel.MemoryFieldsSettings, fieldSettingCollection, languageTextId);
            MapCommunicationFieldsSettings(businessModel.CommunicationFieldsSettings, fieldSettingCollection, languageTextId);
            MapGraphicsFieldsSettings(businessModel.GraphicsFieldsSettings, fieldSettingCollection, languageTextId);
            MapSoundFieldsSettings(businessModel.SoundFieldsSettings, fieldSettingCollection, languageTextId);
            MapContractFieldsSettings(businessModel.ContractFieldsSettings, fieldSettingCollection, languageTextId);
            MapOtherFieldsSettings(businessModel.OtherFieldsSettings, fieldSettingCollection, languageTextId);
            MapContactInformationFieldsSettings(businessModel.ContactInformationFieldsSettings, fieldSettingCollection, languageTextId);
            MapOrganizationFieldsSettings(businessModel.OrganizationFieldsSettings, fieldSettingCollection, languageTextId);
            MapPlaceFieldsSettings(businessModel.PlaceFieldsSettings, fieldSettingCollection, languageTextId);
            MapContactFieldsSettings(businessModel.ContactFieldsSettings, fieldSettingCollection, languageTextId);
            MapStateFieldsSettings(businessModel.StateFieldsSettings, fieldSettingCollection, languageTextId);
            MapDateFieldsSettings(businessModel.DateFieldsSettings, fieldSettingCollection, languageTextId);
        }

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
                                Required = s.Required,
                            }).ToList();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("languageId");
            }

            var settingCollection = new NamedObjectCollection<FieldSettingMapperData>(mapperData);
            return this.entityToBusinessModelMapperForEdit.Map(settingCollection);
        }

        public ComputerFieldsSettingsForModelEdit GetFieldSettingsForModelEdit(int customerId, int languageId)
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
                                ReadOnly = s.ReadOnly,
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
                                ReadOnly = s.ReadOnly,
                                Required = s.Required,
                            }).ToList();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("languageId");
            }

            var settingCollection = new NamedObjectCollection<FieldSettingMapperDataForModelEdit>(mapperData);
            return this.entityToBusinessModelMapperForModelEdit.Map(settingCollection);
        }

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
                    throw new ArgumentOutOfRangeException("languageId");
            }

            var settingCollection = new NamedObjectCollection<FieldOverviewSettingMapperData>(mapperData);
            return this.entityToBusinessModelMapperForOverview.Map(settingCollection);
        }

        public ComputerFieldsSettingsOverviewForFilter GetFieldSettingsOverviewForFilter(int customerId, int languageId)
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
                    throw new ArgumentOutOfRangeException("languageId");
            }

            var settingCollection = new NamedObjectCollection<FieldOverviewSettingMapperData>(mapperData);
            return this.entityToBusinessModelMapperForFilter.Map(settingCollection);
        }

        private static void MapWorkstationFieldsSettings(
            WorkstationFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId)
        {
            MapFieldSetting(updatedSettings.ComputerNameFieldSetting, entity.FindByName(WorkstationFields.Name), languageTextId);
            MapFieldSetting(updatedSettings.ManufacturerFieldSetting, entity.FindByName(WorkstationFields.Manufacturer), languageTextId);
            MapFieldSetting(updatedSettings.ComputerModelFieldSetting, entity.FindByName(WorkstationFields.Model), languageTextId);
            MapFieldSetting(updatedSettings.SerialNumberFieldSetting, entity.FindByName(WorkstationFields.SerialNumber), languageTextId);
            MapFieldSetting(updatedSettings.BIOSDateFieldSetting, entity.FindByName(WorkstationFields.BIOSDate), languageTextId);
            MapFieldSetting(updatedSettings.BIOSVersionFieldSetting, entity.FindByName(WorkstationFields.BIOSVersion), languageTextId);
            MapFieldSetting(updatedSettings.TheftmarkFieldSetting, entity.FindByName(WorkstationFields.Theftmark), languageTextId);
            MapFieldSetting(updatedSettings.CarePackNumberFieldSetting, entity.FindByName(WorkstationFields.CarePackNumber), languageTextId);
            MapFieldSetting(updatedSettings.ComputerTypeFieldSetting, entity.FindByName(WorkstationFields.ComputerType), languageTextId);
            MapFieldSetting(updatedSettings.LocationFieldSetting, entity.FindByName(WorkstationFields.Location), languageTextId);
        }

        private static void MapChassisFieldsSettings(
            ChassisFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId)
        {
            MapFieldSetting(updatedSettings.ChassisFieldSetting, entity.FindByName(ChassisFields.Chassis), languageTextId);
        }

        private static void MapInventoringFieldsSettings(
            InventoryFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId)
        {
            MapFieldSetting(updatedSettings.BarCodeFieldSetting, entity.FindByName(InventoryFields.BarCode), languageTextId);
            MapFieldSetting(updatedSettings.PurchaseDateFieldSetting, entity.FindByName(InventoryFields.PurchaseDate), languageTextId);
        }

        private static void MapOperatingSystemFieldsSettings(
            OperatingSystemFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId)
        {
            MapFieldSetting(updatedSettings.OperatingSystemFieldSetting, entity.FindByName(OperatingSystemFields.OS), languageTextId);
            MapFieldSetting(updatedSettings.VersionFieldSetting, entity.FindByName(OperatingSystemFields.Version), languageTextId);
            MapFieldSetting(updatedSettings.ServicePackSystemFieldSetting, entity.FindByName(OperatingSystemFields.ServicePack), languageTextId);
            MapFieldSetting(updatedSettings.RegistrationCodeSystemFieldSetting, entity.FindByName(OperatingSystemFields.RegistrationCode), languageTextId);
            MapFieldSetting(updatedSettings.ProductKeyFieldSetting, entity.FindByName(OperatingSystemFields.ProductKey), languageTextId);
        }

        private static void MapProcessorFieldsSettings(
            ProcessorFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId)
        {
            MapFieldSetting(updatedSettings.ProccesorFieldSetting, entity.FindByName(ProcessorFields.ProccesorName), languageTextId);
        }

        private static void MapMemoryFieldsSettings(
            MemoryFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId)
        {
            MapFieldSetting(updatedSettings.RAMFieldSetting, entity.FindByName(MemoryFields.RAM), languageTextId);
        }

        private static void MapCommunicationFieldsSettings(
            CommunicationFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId)
        {
            MapFieldSetting(updatedSettings.NetworkAdapterFieldSetting, entity.FindByName(CommunicationFields.NetworkAdapter), languageTextId);
            MapFieldSetting(updatedSettings.IPAddressFieldSetting, entity.FindByName(CommunicationFields.IPAddress), languageTextId);
            MapFieldSetting(updatedSettings.MacAddressFieldSetting, entity.FindByName(CommunicationFields.MacAddress), languageTextId);
            MapFieldSetting(updatedSettings.RASFieldSetting, entity.FindByName(CommunicationFields.RAS), languageTextId);
            MapFieldSetting(updatedSettings.NovellClientFieldSetting, entity.FindByName(CommunicationFields.NovellClient), languageTextId);
        }

        private static void MapGraphicsFieldsSettings(
            GraphicsFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId)
        {
            MapFieldSetting(updatedSettings.VideoCardFieldSetting, entity.FindByName(GraphicsFields.VideoCard), languageTextId);
        }

        private static void MapSoundFieldsSettings(
            SoundFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId)
        {
            MapFieldSetting(updatedSettings.SoundCardFieldSetting, entity.FindByName(SoundFields.SoundCard), languageTextId);
        }

        private static void MapContractFieldsSettings(
            ContractFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId)
        {
            MapFieldSetting(updatedSettings.ContractStatusFieldSetting, entity.FindByName(ContractFields.ContractStatusName), languageTextId);
            MapFieldSetting(updatedSettings.ContractNumberFieldSetting, entity.FindByName(ContractFields.ContractNumber), languageTextId);
            MapFieldSetting(updatedSettings.ContractStartDateFieldSetting, entity.FindByName(ContractFields.ContractStartDate), languageTextId);
            MapFieldSetting(updatedSettings.ContractEndDateFieldSetting, entity.FindByName(ContractFields.ContractEndDate), languageTextId);
            MapFieldSetting(updatedSettings.PurchasePriceFieldSetting, entity.FindByName(ContractFields.PurchasePrice), languageTextId);
            MapFieldSetting(updatedSettings.PurchaseDateFieldSetting, entity.FindByName(InventoryFields.PurchaseDate), languageTextId); // todo should be removed from computer contract models
            MapFieldSetting(updatedSettings.AccountingDimension1FieldSetting, entity.FindByName(ContractFields.AccountingDimension1), languageTextId);
            MapFieldSetting(updatedSettings.AccountingDimension2FieldSetting, entity.FindByName(ContractFields.AccountingDimension2), languageTextId);
            MapFieldSetting(updatedSettings.AccountingDimension3FieldSetting, entity.FindByName(ContractFields.AccountingDimension3), languageTextId);
            MapFieldSetting(updatedSettings.AccountingDimension4FieldSetting, entity.FindByName(ContractFields.AccountingDimension4), languageTextId);
            MapFieldSetting(updatedSettings.AccountingDimension5FieldSetting, entity.FindByName(ContractFields.AccountingDimension5), languageTextId);
        }

        private static void MapOtherFieldsSettings(
            OtherFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId)
        {
            MapFieldSetting(updatedSettings.InfoFieldSetting, entity.FindByName(OtherFields.Info), languageTextId);
        }

        private static void MapContactInformationFieldsSettings(
            ContactInformationFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId)
        {
            MapFieldSetting(updatedSettings.UserIdFieldSetting, entity.FindByName(ContactInformationFields.UserId), languageTextId);
        }

        private static void MapOrganizationFieldsSettings(
            OrganizationFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId)
        {
            MapFieldSetting(updatedSettings.DepartmentFieldSetting, entity.FindByName(OrganizationFields.Department), languageTextId);
            MapFieldSetting(updatedSettings.DomainFieldSetting, entity.FindByName(OrganizationFields.Domain), languageTextId);
            MapFieldSetting(updatedSettings.UnitFieldSetting, entity.FindByName(OrganizationFields.Unit), languageTextId);
        }

        private static void MapPlaceFieldsSettings(
            PlaceFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId)
        {
            MapFieldSetting(updatedSettings.RoomFieldSetting, entity.FindByName(PlaceFields.Room), languageTextId);
            MapFieldSetting(updatedSettings.AddressFieldSetting, entity.FindByName(PlaceFields.Address), languageTextId);
            MapFieldSetting(updatedSettings.PostalCodeFieldSetting, entity.FindByName(PlaceFields.PostalCode), languageTextId);
            MapFieldSetting(updatedSettings.PostalAddressFieldSetting, entity.FindByName(PlaceFields.PostalAddress), languageTextId);
            MapFieldSetting(updatedSettings.PlaceFieldSetting, entity.FindByName(PlaceFields.Location), languageTextId);
            MapFieldSetting(updatedSettings.Place2FieldSetting, entity.FindByName(PlaceFields.Location2), languageTextId);
        }

        private static void MapContactFieldsSettings(
            ContactFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId)
        {
            MapFieldSetting(updatedSettings.NameFieldSetting, entity.FindByName(ContactFields.Name), languageTextId);
            MapFieldSetting(updatedSettings.PhoneFieldSetting, entity.FindByName(ContactFields.Phone), languageTextId);
            MapFieldSetting(updatedSettings.EmailFieldSetting, entity.FindByName(ContactFields.Email), languageTextId);
        }

        private static void MapStateFieldsSettings(
            StateFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId)
        {
            MapFieldSetting(updatedSettings.StateFieldSetting, entity.FindByName(StateFields.State), languageTextId);
            MapFieldSetting(updatedSettings.StolenFieldSetting, entity.FindByName(StateFields.Stolen), languageTextId);
            MapFieldSetting(updatedSettings.ReplacedWithFieldSetting, entity.FindByName(StateFields.Replaced), languageTextId);
            MapFieldSetting(updatedSettings.SendBackFieldSetting, entity.FindByName(StateFields.SendBack), languageTextId);
            MapFieldSetting(updatedSettings.ScrapDateFieldSetting, entity.FindByName(StateFields.ScrapDate), languageTextId);
        }

        private static void MapDateFieldsSettings(
            DateFieldsSettings updatedSettings,
            NamedObjectCollection<ComputerFieldSettings> entity,
            string languageTextId)
        {
            MapFieldSetting(updatedSettings.CreatedDateFieldSetting, entity.FindByName(DateFields.CreatedDate), languageTextId);
            MapFieldSetting(updatedSettings.ChangedDateFieldSetting, entity.FindByName(DateFields.ChangedDate), languageTextId);
            MapFieldSetting(updatedSettings.SyncChangedDateSetting, entity.FindByName(DateFields.SynchronizeDate), languageTextId);
            MapFieldSetting(updatedSettings.ScanDateFieldSetting, entity.FindByName(DateFields.ScanDate), languageTextId);
            MapFieldSetting(updatedSettings.PathDirectoryFieldSetting, entity.FindByName(DateFields.PathDirectory), languageTextId);
        }

        private static void MapFieldSetting(
            FieldSetting updatedSetting,
            ComputerFieldSettings fieldSetting,
            string languageTextId)
        {
            fieldSetting.ChangedDate = updatedSetting.ChangedDate;
            fieldSetting.Required = updatedSetting.IsRequired.ToInt();
            fieldSetting.ReadOnly = updatedSetting.IsReadOnly.ToInt();
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
                    throw new ArgumentOutOfRangeException("languageTextId");
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
    }
}