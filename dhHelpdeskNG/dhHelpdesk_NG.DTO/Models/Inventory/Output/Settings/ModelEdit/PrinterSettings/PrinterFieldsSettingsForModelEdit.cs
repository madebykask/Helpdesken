namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.PrinterSettings
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.SharedSettings;
    using DH.Helpdesk.Common.ValidationAttributes;

    using CommunicationFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.SharedSettings.CommunicationFieldsSettings;
    using PlaceFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.SharedSettings.PlaceFieldsSettings;

    public class PrinterFieldsSettingsForModelEdit
    {
        public PrinterFieldsSettingsForModelEdit(
            GeneralFieldsSettings generalFieldsSettingsSettings,
            InventoryFieldsSettings inventoryFieldsSettingsSettings,
            CommunicationFieldsSettings communicationFieldsSettings,
            OtherFieldsSettings otherFieldsSettings,
            OrganizationFieldsSettings organizationFieldsSettings,
            PlaceFieldsSettings placeFieldsSettings,
            StateFieldsSettings stateFieldsSettings)
        {
            this.GeneralFieldsSettingsSettings = generalFieldsSettingsSettings;
            this.InventoryFieldsSettingsSettings = inventoryFieldsSettingsSettings;
            this.CommunicationFieldsSettings = communicationFieldsSettings;
            this.OtherFieldsSettings = otherFieldsSettings;
            this.OrganizationFieldsSettings = organizationFieldsSettings;
            this.PlaceFieldsSettings = placeFieldsSettings;
            this.StateFieldsSettings = stateFieldsSettings;
        }

        [NotNull]
        public GeneralFieldsSettings GeneralFieldsSettingsSettings { get; private set; }

        [NotNull]
        public InventoryFieldsSettings InventoryFieldsSettingsSettings { get; private set; }

        [NotNull]
        public CommunicationFieldsSettings CommunicationFieldsSettings { get; private set; }

        [NotNull]
        public OtherFieldsSettings OtherFieldsSettings { get; private set; }

        [NotNull]
        public OrganizationFieldsSettings OrganizationFieldsSettings { get; private set; }

        [NotNull]
        public PlaceFieldsSettings PlaceFieldsSettings { get; private set; }

        [NotNull]
        public StateFieldsSettings StateFieldsSettings { get; private set; }
    }
}