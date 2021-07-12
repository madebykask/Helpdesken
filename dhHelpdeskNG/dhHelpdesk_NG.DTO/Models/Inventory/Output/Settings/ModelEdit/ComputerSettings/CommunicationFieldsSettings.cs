namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class CommunicationFieldsSettings
    {
        public CommunicationFieldsSettings(ModelEditFieldSetting networkAdapterFieldSetting, ModelEditFieldSetting ipAddressFieldSetting, ModelEditFieldSetting macAddressFieldSetting, ModelEditFieldSetting rasFieldSetting, ModelEditFieldSetting novellClientFieldSetting)
        {
            this.NetworkAdapterFieldSetting = networkAdapterFieldSetting;
            this.IPAddressFieldSetting = ipAddressFieldSetting;
            this.MacAddressFieldSetting = macAddressFieldSetting;
            this.RASFieldSetting = rasFieldSetting;
            this.NovellClientFieldSetting = novellClientFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting NetworkAdapterFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting IPAddressFieldSetting { get; set; }
        
        [NotNull]
        public ModelEditFieldSetting MacAddressFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting RASFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting NovellClientFieldSetting { get; set; }
    }
}