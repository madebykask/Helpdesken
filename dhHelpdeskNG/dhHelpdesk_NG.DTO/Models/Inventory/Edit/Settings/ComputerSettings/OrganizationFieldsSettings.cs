namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFieldsSettings
    {
        public OrganizationFieldsSettings(
            FieldSetting departmentFieldSetting,
            FieldSetting domainFieldSetting,
            FieldSetting unitFieldSetting)
        {
            this.DepartmentFieldSetting = departmentFieldSetting;
            this.DomainFieldSetting = domainFieldSetting;
            this.UnitFieldSetting = unitFieldSetting;
        }

        [NotNull]
        public FieldSetting DepartmentFieldSetting { get; set; }

        [NotNull]
        public FieldSetting DomainFieldSetting { get; set; }

        [NotNull]
        public FieldSetting UnitFieldSetting { get; set; }
    }
}