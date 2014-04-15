namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings
{
    using System;

    using DH.Helpdesk.BusinessData.Attributes;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.SharedSettings;
    using DH.Helpdesk.Common.ValidationAttributes;

    using CommunicationFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.SharedSettings.CommunicationFieldsSettings;
    using PlaceFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.SharedSettings.PlaceFieldsSettings;

    public class PrinterFieldsSettings
    {
        private PrinterFieldsSettings(
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

        [IsId]
        [AllowRead(ModelStates.Updated)]
        public int CustomerId { get; private set; }

        [IsId]
        [AllowRead(ModelStates.Updated)]
        public int LanguageId { get; private set; }

        [AllowRead(ModelStates.Updated)]
        public DateTime ChangedDate { get; private set; }

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

        public static PrinterFieldsSettings CreateUpdated(
            int customerId,
            int langaugeId,
            DateTime changedDate,
            GeneralFieldsSettings generalFieldsSettings,
            InventoryFieldsSettings inventoryFieldsSettings,
            CommunicationFieldsSettings communicationFieldsSettings,
            OtherFieldsSettings otherFieldsSettings,
            OrganizationFieldsSettings organizationFieldsSettings,
            PlaceFieldsSettings placeFieldsSettings,
            StateFieldsSettings stateFieldsSettings)
        {
            var businessModel = new PrinterFieldsSettings(
                generalFieldsSettings,
                inventoryFieldsSettings,
                communicationFieldsSettings,
                otherFieldsSettings,
                organizationFieldsSettings,
                placeFieldsSettings,
                stateFieldsSettings) { ChangedDate = changedDate, CustomerId = customerId, LanguageId = langaugeId };

            return businessModel;
        }

        public static PrinterFieldsSettings CreateForEdit(
            GeneralFieldsSettings generalFieldsSettings,
            InventoryFieldsSettings inventoryFieldsSettings,
            CommunicationFieldsSettings communicationFieldsSettings,
            OtherFieldsSettings otherFieldsSettings,
            OrganizationFieldsSettings organizationFieldsSettings,
            PlaceFieldsSettings placeFieldsSettings,
            StateFieldsSettings stateFieldsSettings)
        {
            var businessModel = new PrinterFieldsSettings(
                generalFieldsSettings,
                inventoryFieldsSettings,
                communicationFieldsSettings,
                otherFieldsSettings,
                organizationFieldsSettings,
                placeFieldsSettings,
                stateFieldsSettings);

            return businessModel;
        }
    }
}
