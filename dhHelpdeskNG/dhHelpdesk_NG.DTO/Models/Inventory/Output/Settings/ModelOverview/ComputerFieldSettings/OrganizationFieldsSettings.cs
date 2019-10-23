namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFieldsSettings
    {
        public OrganizationFieldsSettings(
            FieldSettingOverview regionFieldSetting,
            FieldSettingOverview departmentFieldSetting,
            FieldSettingOverview domainFieldSetting,
            FieldSettingOverview unitFieldSetting)
        {
            RegionFieldSetting = regionFieldSetting;
            this.DepartmentFieldSetting = departmentFieldSetting;
            this.DomainFieldSetting = domainFieldSetting;
            this.UnitFieldSetting = unitFieldSetting;
        }

        
        [NotNull]
        public FieldSettingOverview RegionFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview DepartmentFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview DomainFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview UnitFieldSetting { get; set; }
    }
}