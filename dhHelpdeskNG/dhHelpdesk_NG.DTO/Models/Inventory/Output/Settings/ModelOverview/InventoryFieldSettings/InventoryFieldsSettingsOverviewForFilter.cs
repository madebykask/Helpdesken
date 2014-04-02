namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryFieldsSettingsOverviewForFilter
    {
        public InventoryFieldsSettingsOverviewForFilter(FieldSettingOverview departmentFieldSetting)
        {
            this.DepartmentFieldSetting = departmentFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview DepartmentFieldSetting { get; private set; }
    }
}