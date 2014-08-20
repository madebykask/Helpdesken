namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.PrinterSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFieldsSettings
    {
        public OrganizationFieldsSettings(
            ProcessingFieldSetting departmentFieldSetting,
            ProcessingFieldSetting unitFieldSetting)
        {
            this.DepartmentFieldSetting = departmentFieldSetting;
            this.UnitFieldSetting = unitFieldSetting;
        }

        [NotNull]
        public ProcessingFieldSetting DepartmentFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting UnitFieldSetting { get; set; }
    }
}