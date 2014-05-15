namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.PrinterSettings
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.SharedSettings;
    using DH.Helpdesk.Common.ValidationAttributes;

    using CommunicationFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.SharedSettings.CommunicationFieldsSettings;
    using PlaceFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.SharedSettings.PlaceFieldsSettings;

    public class PrinterFieldsSettingsForModelEdit
    {
        public PrinterFieldsSettingsForModelEdit(
            GeneralFieldsSettings generalFieldsSettings,
            InventoryFieldsSettings inventoryFieldsSettings,
            CommunicationFieldsSettings communicationFieldsSettings,
            OtherFieldsSettings otherFieldsSettings,
            OrganizationFieldsSettings organizationFieldsSettings,
            PlaceFieldsSettings placeFieldsSettings,
            StateFieldsSettings stateFieldsSettings)
        {
            this.GeneralFieldsSettings = generalFieldsSettings;
            this.InventoryFieldsSettings = inventoryFieldsSettings;
            this.CommunicationFieldsSettings = communicationFieldsSettings;
            this.OtherFieldsSettings = otherFieldsSettings;
            this.OrganizationFieldsSettings = organizationFieldsSettings;
            this.PlaceFieldsSettings = placeFieldsSettings;
            this.StateFieldsSettings = stateFieldsSettings;
        }

        [NotNull]
        public GeneralFieldsSettings GeneralFieldsSettings { get; private set; }

        [NotNull]
        public InventoryFieldsSettings InventoryFieldsSettings { get; private set; }

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