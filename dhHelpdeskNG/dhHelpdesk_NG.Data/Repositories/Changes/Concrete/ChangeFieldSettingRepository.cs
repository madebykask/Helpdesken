﻿namespace DH.Helpdesk.Dal.Repositories.Changes.Concrete
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeProcessing;
    using DH.Helpdesk.BusinessData.Models.Changes.Settings.SettingsEdit;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Dal.Attributes.Changes;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.MapperData.Changes;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeFieldSettingRepository : Repository, IChangeFieldSettingRepository
    {
        #region Fields

        private readonly
            IEntityToBusinessModelMapper<NamedObjectCollection<FieldEditSettingMapperData>, ChangeEditSettings>
            changeFieldSettingsToChangeEditSettingsMapper;

        private readonly
            IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, ChangeOverviewSettings>
            changeFieldSettingsToChangeOverviewSettingsMapper;

        private readonly
            IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperData>, ChangeFieldSettings>
            changeFieldSettingsToFieldSettingsMapper;

        private readonly IBusinessModelToEntityMapper<ChangeFieldSettings, NamedObjectCollection<ChangeFieldSettingsEntity>>
            updatedFieldSettingsToChangeFieldSettingsMapper;

        private readonly
            IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, SearchSettings>
            changeFieldSettingsToSearchSettingsMapper;

        private readonly
            IEntityToBusinessModelMapper
                <NamedObjectCollection<FieldProcessingSettingMapperData>, ChangeProcessingSettings>
            changeFieldSettingsToChangeProcessingSettingsMapper;

        #endregion

        #region Constructors and Destructors

        public ChangeFieldSettingRepository(
            IDatabaseFactory databaseFactory,
            IEntityToBusinessModelMapper<NamedObjectCollection<FieldEditSettingMapperData>, ChangeEditSettings>
                changeFieldSettingsToChangeEditSettingsMapper,
            IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, ChangeOverviewSettings>
                changeFieldSettingsToChangeOverviewSettingsMapper,
            IEntityToBusinessModelMapper<NamedObjectCollection<FieldSettingMapperData>, ChangeFieldSettings>
                changeFieldSettingsToFieldSettingsMapper,
            IBusinessModelToEntityMapper<ChangeFieldSettings, NamedObjectCollection<ChangeFieldSettingsEntity>>
                updatedFieldSettingsToChangeFieldSettingsMapper,
            IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, SearchSettings>
                changeFieldSettingsToSearchSettingsMapper,
            IEntityToBusinessModelMapper
                <NamedObjectCollection<FieldProcessingSettingMapperData>, ChangeProcessingSettings>
                changeFieldSettingsToChangeProcessingSettingsMapper)
            : base(databaseFactory)
        {
            this.changeFieldSettingsToChangeEditSettingsMapper = changeFieldSettingsToChangeEditSettingsMapper;
            this.changeFieldSettingsToChangeOverviewSettingsMapper = changeFieldSettingsToChangeOverviewSettingsMapper;
            this.changeFieldSettingsToFieldSettingsMapper = changeFieldSettingsToFieldSettingsMapper;
            this.updatedFieldSettingsToChangeFieldSettingsMapper = updatedFieldSettingsToChangeFieldSettingsMapper;
            this.changeFieldSettingsToSearchSettingsMapper = changeFieldSettingsToSearchSettingsMapper;
            this.changeFieldSettingsToChangeProcessingSettingsMapper =
                changeFieldSettingsToChangeProcessingSettingsMapper;
        }

        #endregion

        #region Public Methods and Operators

        [CreateMissingSettings("customerId")]
        public ChangeEditSettings GetEnglishEditSettings(int customerId)
        {
            var fieldSettings = this.FindByCustomerIdCore(customerId);

            var mapperData =
                fieldSettings.Select(
                    s =>
                    new FieldEditSettingMapperData
                        {
                            Bookmark = s.Bookmark,
                            Caption = s.Label_ENG,
                            InitialValue = s.InitialValue,
                            ChangeField = s.ChangeField,
                            Required = s.Required,
                            Show = s.Show
                        }).ToList();

            var fieldSettingCollection = new NamedObjectCollection<FieldEditSettingMapperData>(mapperData);
            return this.changeFieldSettingsToChangeEditSettingsMapper.Map(fieldSettingCollection);
        }

        [CreateMissingSettings("customerId")]
        public ChangeFieldSettings GetEnglishFieldSettings(int customerId)
        {
            var fieldSettings = this.FindByCustomerIdCore(customerId);

            var mapperData =
                fieldSettings.Select(
                    s =>
                    new FieldSettingMapperData
                        {
                            Bookmark = s.Bookmark,
                            Caption = s.Label_ENG,
                            InitialValue = s.InitialValue,
                            ChangeField = s.ChangeField,
                            Required = s.Required,
                            Show = s.Show,
                            ShowExternal = s.ShowExternal,
                            ShowInList = s.ShowInList
                        }).ToList();

            var fieldSettingCollection = new NamedObjectCollection<FieldSettingMapperData>(mapperData);
            var settings = this.changeFieldSettingsToFieldSettingsMapper.Map(fieldSettingCollection);
            settings.LanguageId = 2;
            return settings;
        }

        [CreateMissingSettings("customerId")]
        public ChangeOverviewSettings GetEnglishOverviewSettings(int customerId, bool onlyListSettings)
        {
            var fieldSettings = this.FindByCustomerIdCore(customerId);

            var mapperData =
                fieldSettings.Select(
                    s =>
                    new FieldOverviewSettingMapperData
                        {
                            Caption = s.Label_ENG,
                            ChangeField = s.ChangeField,
                            Show = onlyListSettings ? s.ShowInList : s.Show
                        }).ToList();

            var fieldSettingCollection = new NamedObjectCollection<FieldOverviewSettingMapperData>(mapperData);
            return this.changeFieldSettingsToChangeOverviewSettingsMapper.Map(fieldSettingCollection);
        }

        [CreateMissingSettings("customerId")]
        public ChangeEditSettings GetSwedishEditSettings(int customerId)
        {
            var fieldSettings = this.FindByCustomerIdCore(customerId);

            var mapperData =
                fieldSettings.Select(
                    s =>
                    new FieldEditSettingMapperData
                        {
                            Bookmark = s.Bookmark,
                            Caption = s.Label,
                            InitialValue = s.InitialValue,
                            ChangeField = s.ChangeField,
                            Required = s.Required,
                            Show = s.Show
                        }).ToList();

            var fieldSettingCollection = new NamedObjectCollection<FieldEditSettingMapperData>(mapperData);
            return this.changeFieldSettingsToChangeEditSettingsMapper.Map(fieldSettingCollection);
        }

        [CreateMissingSettings("customerId")]
        public ChangeFieldSettings GetSwedishFieldSettings(int customerId)
        {
            var fieldSettings = this.FindByCustomerIdCore(customerId);

            var mapperData =
                fieldSettings.Select(
                    s =>
                    new FieldSettingMapperData
                        {
                            Bookmark = s.Bookmark,
                            Caption = s.Label,
                            InitialValue = s.InitialValue,
                            ChangeField = s.ChangeField,
                            Required = s.Required,
                            Show = s.Show,
                            ShowExternal = s.ShowExternal,
                            ShowInList = s.ShowInList
                        }).ToList();

            var fieldSettingCollection = new NamedObjectCollection<FieldSettingMapperData>(mapperData);
            var settings = this.changeFieldSettingsToFieldSettingsMapper.Map(fieldSettingCollection);
            settings.LanguageId = 1;
            return settings;
        }

        [CreateMissingSettings("customerId")]
        public SearchSettings GetEnglishSearchSettings(int customerId)
        {
            var settings = this.FindByCustomerIdCore(customerId);

            var mapperData =
                settings.Select(
                    s =>
                    new FieldOverviewSettingMapperData
                        {
                            Caption = s.Label_ENG,
                            ChangeField = s.ChangeField,
                            Show = s.ShowInList
                        }).ToList();

            var settingCollection = new NamedObjectCollection<FieldOverviewSettingMapperData>(mapperData);
            return this.changeFieldSettingsToSearchSettingsMapper.Map(settingCollection);
        }

        [CreateMissingSettings("customerId")]
        public SearchSettings GetSwedishSearchSettings(int customerId)
        {
            var settings = this.FindByCustomerIdCore(customerId);

            var mapperData =
                settings.Select(
                    s =>
                    new FieldOverviewSettingMapperData
                        {
                            Caption = s.Label,
                            ChangeField = s.ChangeField,
                            Show = s.ShowInList
                        }).ToList();

            var settingCollection = new NamedObjectCollection<FieldOverviewSettingMapperData>(mapperData);
            return this.changeFieldSettingsToSearchSettingsMapper.Map(settingCollection);
        }

        [CreateMissingSettings("customerId")]
        public ChangeProcessingSettings GetProcessingSettings(int customerId)
        {
            var settings = this.FindByCustomerIdCore(customerId);

            var mapperData =
                settings.Select(
                    s =>
                    new FieldProcessingSettingMapperData
                        {
                            ChangeField = s.ChangeField,
                            Required = s.Required,
                            Show = s.Show
                        }).ToList();

            var settingCollection = new NamedObjectCollection<FieldProcessingSettingMapperData>(mapperData);
            return this.changeFieldSettingsToChangeProcessingSettingsMapper.Map(settingCollection);
        }

        [CreateMissingSettings("customerId")]
        public ChangeOverviewSettings GetSwedishOverviewSettings(int customerId, bool onlyListSettings)
        {
            var fieldSettings = this.FindByCustomerIdCore(customerId);

            var mapperData =
                fieldSettings.Select(
                    s =>
                        new FieldOverviewSettingMapperData
                        {
                            Caption = s.Label,
                            ChangeField = s.ChangeField,
                            Show = onlyListSettings ? s.ShowInList : s.Show
                        }).ToList();

            var fieldSettingCollection = new NamedObjectCollection<FieldOverviewSettingMapperData>(mapperData);
            return this.changeFieldSettingsToChangeOverviewSettingsMapper.Map(fieldSettingCollection);
        }

        public void UpdateSettings(ChangeFieldSettings updatedSettings)
        {
            var fieldSettings = this.FindByCustomerIdCore(updatedSettings.CustomerId.Value).ToList();
            var fieldSettingCollection = new NamedObjectCollection<ChangeFieldSettingsEntity>(fieldSettings);
            this.updatedFieldSettingsToChangeFieldSettingsMapper.Map(updatedSettings, fieldSettingCollection);
        }

        #endregion

        #region Methods

        private IQueryable<ChangeFieldSettingsEntity> FindByCustomerIdCore(int customerId)
        {
            return this.DbContext.ChangeFieldSettings.Where(s => s.Customer_Id == customerId);
        }

        #endregion
    }
}