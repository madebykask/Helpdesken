namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFieldsSettings
    {
        public OrganizationFieldsSettings(
            ProcessingFieldSetting departmentFieldSetting,
            ProcessingFieldSetting domainFieldSetting,
            ProcessingFieldSetting unitFieldSetting)
        {
            this.DepartmentFieldSetting = departmentFieldSetting;
            this.DomainFieldSetting = domainFieldSetting;
            this.UnitFieldSetting = unitFieldSetting;
        }

        [NotNull]
        public ProcessingFieldSetting DepartmentFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting DomainFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting UnitFieldSetting { get; set; }
    }
}