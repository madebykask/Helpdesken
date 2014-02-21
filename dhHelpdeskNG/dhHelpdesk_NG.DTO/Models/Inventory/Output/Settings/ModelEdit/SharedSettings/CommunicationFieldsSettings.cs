namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.SharedSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class CommunicationFieldsSettings
    {
        public CommunicationFieldsSettings(ModelEditFieldSetting networkAdapterFieldSetting, ModelEditFieldSetting ipAddressFieldSetting, ModelEditFieldSetting macAddressFieldSetting)
        {
            this.NetworkAdapterFieldSetting = networkAdapterFieldSetting;
            this.IPAddressFieldSetting = ipAddressFieldSetting;
            this.MacAddressFieldSetting = macAddressFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting NetworkAdapterFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting IPAddressFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting MacAddressFieldSetting { get; set; }
    }
}