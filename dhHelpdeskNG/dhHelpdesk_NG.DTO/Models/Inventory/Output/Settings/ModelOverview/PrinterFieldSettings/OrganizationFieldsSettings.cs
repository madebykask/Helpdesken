namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.PrinterFieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFieldsSettings
    {
        public OrganizationFieldsSettings(
            FieldSettingOverview departmentieFieldSetting,
            FieldSettingOverview unitFieldSetting)
        {
            this.DepartmentFieldSetting = departmentieFieldSetting;
            this.UnitFieldSetting = unitFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview DepartmentFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview UnitFieldSetting { get; set; }
    }
}