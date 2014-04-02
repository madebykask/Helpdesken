namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.SharedSettings;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class PrinterFieldsSettings
    {
        public PrinterFieldsSettings(GeneralFieldsSettings generalFieldsSettingsSettings, InventoryFieldsSettings inventoryFieldsSettingsSettings, CommunicationFieldsSettings communicationFieldsSettings, OtherFieldsSettings otherFieldsSettings, OrganizationFieldsSettings organizationFieldsSettings, PlaceFieldsSettings placeFieldsSettings, StateFieldsSettings stateFieldsSettings)
        {
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

        public static PrinterFieldsSettings CreateUpdated(int? customerId, int? langaugeId, GeneralFieldsSettings generalFieldsSettings, InventoryFieldsSettings inventoryFieldsSettings, CommunicationFieldsSettings communicationFieldsSettings, OtherFieldsSettings otherFieldsSettings, OrganizationFieldsSettings organizationFieldsSettings, PlaceFieldsSettings placeFieldsSettings, StateFieldsSettings stateFieldsSettings)
        {
            var businessModel = new PrinterFieldsSettings(generalFieldsSettings, inventoryFieldsSettings, communicationFieldsSettings, otherFieldsSettings, organizationFieldsSettings, placeFieldsSettings, stateFieldsSettings) { CustomerId = customerId, LanguageId = langaugeId };

            return businessModel;
        }

        public static PrinterFieldsSettings CreateForEdit(GeneralFieldsSettings generalFieldsSettings, InventoryFieldsSettings inventoryFieldsSettings, CommunicationFieldsSettings communicationFieldsSettings, OtherFieldsSettings otherFieldsSettings, OrganizationFieldsSettings organizationFieldsSettings, PlaceFieldsSettings placeFieldsSettings, StateFieldsSettings stateFieldsSettings)
        {
            var businessModel = new PrinterFieldsSettings(generalFieldsSettings, inventoryFieldsSettings, communicationFieldsSettings, otherFieldsSettings, organizationFieldsSettings, placeFieldsSettings, stateFieldsSettings);

            return businessModel;
        }
    }
}
