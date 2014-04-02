namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.PrinterSettings
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.SharedSettings;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class PrinterFieldsSettingsForModelEdit
    {
        public PrinterFieldsSettingsForModelEdit(int? customerId, int? languageId, GeneralFieldsSettings generalFieldsSettingsSettings, InventoryFieldsSettings inventoryFieldsSettingsSettings, CommunicationFieldsSettings communicationFieldsSettings, OtherFieldsSettings otherFieldsSettings, OrganizationFieldsSettings organizationFieldsSettings, PlaceFieldsSettings placeFieldsSettings, StateFieldsSettings stateFieldsSettings)
        {
            this.CustomerId = customerId;
            this.LanguageId = languageId;
            this.GeneralFieldsSettingsSettings = generalFieldsSettingsSettings;
            this.InventoryFieldsSettingsSettings = inventoryFieldsSettingsSettings;
            this.CommunicationFieldsSettings = communicationFieldsSettings;
            this.OtherFieldsSettings = otherFieldsSettings;
            this.OrganizationFieldsSettings = organizationFieldsSettings;
            this.PlaceFieldsSettings = placeFieldsSettings;
            this.StateFieldsSettings = stateFieldsSettings;
        }

        [IsId]
        public int? CustomerId { get; private set; }

        [IsId]
        public int? LanguageId { get; private set; }

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