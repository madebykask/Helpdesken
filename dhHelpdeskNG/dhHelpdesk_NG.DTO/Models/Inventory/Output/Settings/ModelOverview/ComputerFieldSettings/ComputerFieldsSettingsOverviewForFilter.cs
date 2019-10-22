namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ComputerFieldsSettingsOverviewForFilter
    {
        public ComputerFieldsSettingsOverviewForFilter(
            FieldSettingOverview domainFieldSetting,
            FieldSettingOverview regionFieldSetting,
            FieldSettingOverview departmentFieldSetting,
            FieldSettingOverview computerTypeFieldSetting,
            FieldSettingOverview contractStatusFieldSetting,
            FieldSettingOverview contractStartDateFieldSetting,
            FieldSettingOverview contractEndDateFieldSetting,
            FieldSettingOverview scanDateFieldSetting,
            FieldSettingOverview scrapDateFieldSetting)
        {
            DomainFieldSetting = domainFieldSetting;
            RegionFieldSetting = regionFieldSetting;
            DepartmentFieldSetting = departmentFieldSetting;
            ComputerTypeFieldSetting = computerTypeFieldSetting;
            ContractStatusFieldSetting = contractStatusFieldSetting;
            ContractStartDateFieldSetting = contractStartDateFieldSetting;
            ContractEndDateFieldSetting = contractEndDateFieldSetting;
            ScanDateFieldSetting = scanDateFieldSetting;
            ScrapDateFieldSetting = scrapDateFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview DomainFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview RegionFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview DepartmentFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview ComputerTypeFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview ContractStatusFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview ContractStartDateFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview ContractEndDateFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview ScanDateFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview ScrapDateFieldSetting { get; private set; }
    }
}