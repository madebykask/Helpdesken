namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.SharedSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFieldsSettings
    {
        public OrganizationFieldsSettings(FieldSettingOverview domainFieldSetting, FieldSettingOverview unitFieldSetting)
        {
            this.DomainFieldSetting = domainFieldSetting;
            this.UnitFieldSetting = unitFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview DomainFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview UnitFieldSetting { get; set; }
    }
}