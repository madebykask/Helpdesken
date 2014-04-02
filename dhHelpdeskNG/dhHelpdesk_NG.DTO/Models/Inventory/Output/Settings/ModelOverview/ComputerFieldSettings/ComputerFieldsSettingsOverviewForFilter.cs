namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ComputerFieldsSettingsOverviewForFilter
    {
        public ComputerFieldsSettingsOverviewForFilter(
            FieldSettingOverview departmnetFieldSetting,
            FieldSettingOverview computerTypeFieldSetting,
            FieldSettingOverview contractStatusFieldSetting,
            FieldSettingOverview contractStartDateFieldSetting,
            FieldSettingOverview contractEndDateFieldSetting,
            FieldSettingOverview scanDateFieldSetting,
            FieldSettingOverview scrapDateFieldSetting)
        {
            this.DepartmnetFieldSetting = departmnetFieldSetting;
            this.ComputerTypeFieldSetting = computerTypeFieldSetting;
            this.ContractStatusFieldSetting = contractStatusFieldSetting;
            this.ContractStartDateFieldSetting = contractStartDateFieldSetting;
            this.ContractEndDateFieldSetting = contractEndDateFieldSetting;
            this.ScanDateFieldSetting = scanDateFieldSetting;
            this.ScrapDateFieldSetting = scrapDateFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview DepartmnetFieldSetting { get; private set; }

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