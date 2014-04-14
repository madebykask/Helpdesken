namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFieldsSettings
    {
        public OrganizationFieldsSettings(
            FieldSetting departmentFieldSetting,
            FieldSetting unitFieldSetting)
        {
            this.DepartmentFieldSetting = departmentFieldSetting;
            this.UnitFieldSetting = unitFieldSetting;
        }

        [NotNull]
        public FieldSetting DepartmentFieldSetting { get; set; }

        [NotNull]
        public FieldSetting UnitFieldSetting { get; set; }
    }
}