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
            FieldSettingOverview scrapDateFieldSetting,
            FieldSettingOverview contactUserIdFieldSetting)
        {
            DepartmnetFieldSetting = departmnetFieldSetting;
            ComputerTypeFieldSetting = computerTypeFieldSetting;
            ContractStatusFieldSetting = contractStatusFieldSetting;
            ContractStartDateFieldSetting = contractStartDateFieldSetting;
            ContractEndDateFieldSetting = contractEndDateFieldSetting;
            ScanDateFieldSetting = scanDateFieldSetting;
            ScrapDateFieldSetting = scrapDateFieldSetting;
            ContactUserIdFieldSetting = contactUserIdFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview DepartmnetFieldSetting { get; private set; }

        [NotNull]
        public FieldSettingOverview ContactUserIdFieldSetting { get; private set; }

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