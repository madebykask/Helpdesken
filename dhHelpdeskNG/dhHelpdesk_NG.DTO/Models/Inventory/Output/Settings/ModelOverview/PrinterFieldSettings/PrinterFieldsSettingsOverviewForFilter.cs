namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.PrinterFieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class PrinterFieldsSettingsOverviewForFilter
    {
        public PrinterFieldsSettingsOverviewForFilter(FieldSettingOverview departmentFieldSetting)
        {
            this.DepartmentFieldSetting = departmentFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview DepartmentFieldSetting { get; private set; }
    }
}