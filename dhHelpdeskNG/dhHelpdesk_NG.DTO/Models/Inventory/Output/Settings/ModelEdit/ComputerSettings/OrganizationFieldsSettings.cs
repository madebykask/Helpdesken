namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFieldsSettings
    {
        public OrganizationFieldsSettings(
            ModelEditFieldSetting regionFieldSetting,
            ModelEditFieldSetting departmentFieldSetting,
            ModelEditFieldSetting domainFieldSetting,
            ModelEditFieldSetting unitFieldSetting)
        {
            this.RegionFieldSetting = regionFieldSetting;
            this.DepartmentFieldSetting = departmentFieldSetting;
            this.DomainFieldSetting = domainFieldSetting;
            this.UnitFieldSetting = unitFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting RegionFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting DepartmentFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting DomainFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting UnitFieldSetting { get; set; }
    }
}