namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.SettingsEdit.SharedSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFieldsSettings
    {
        public OrganizationFieldsSettings(FieldSetting domainFieldSetting, FieldSetting unitFieldSetting)
        {
            this.DomainFieldSetting = domainFieldSetting;
            this.UnitFieldSetting = unitFieldSetting;
        }

        [NotNull]
        public FieldSetting DomainFieldSetting { get; set; }

        [NotNull]
        public FieldSetting UnitFieldSetting { get; set; }
    }
}