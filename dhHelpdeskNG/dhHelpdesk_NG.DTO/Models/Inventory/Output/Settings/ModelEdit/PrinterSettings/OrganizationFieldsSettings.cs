namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.PrinterSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFieldsSettings
    {
        public OrganizationFieldsSettings(
            ModelEditFieldSetting departmentFieldSetting,
            ModelEditFieldSetting unitFieldSetting)
        {
            this.DepartmentFieldSetting = departmentFieldSetting;
            this.UnitFieldSetting = unitFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting DepartmentFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting UnitFieldSetting { get; set; }
    }
}