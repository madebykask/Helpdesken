namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.SharedSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OrganizationFieldsSettings
    {
        public OrganizationFieldsSettings(ModelEditFieldSetting domainFieldSetting, ModelEditFieldSetting unitFieldSetting)
        {
            this.DomainFieldSetting = domainFieldSetting;
            this.UnitFieldSetting = unitFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting DomainFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting UnitFieldSetting { get; set; }
    }
}