namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Printer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Shared;

    public class PrinterFieldsSettingsViewModel
    {
        public PrinterFieldsSettingsViewModel()
        {
        }

        public PrinterFieldsSettingsViewModel(
            int? customerId,
            int? languageId,
            GeneralFieldsSettingsModel generalFieldsSettingsSettingsModel,
            InventoryFieldsSettingsModel inventoryFieldsSettingsSettingsModel,
            CommunicationFieldsSettingsModel communicationFieldsSettingsModel,
            OtherFieldsSettingsModel otherFieldsSettingsModel,
            OrganizationFieldsSettingsModel organizationFieldsSettingsModel,
            PlaceFieldsSettingsModel placeFieldsSettingsModel,
            StateFieldsSettingsModel stateFieldsSettingsModel)
        {
            this.CustomerId = customerId;
            this.LanguageId = languageId;
            this.GeneralFieldsSettingsModel = generalFieldsSettingsSettingsModel;
            this.InventoryFieldsSettingsModel = inventoryFieldsSettingsSettingsModel;
            this.CommunicationFieldsSettingsModel = communicationFieldsSettingsModel;
            this.OtherFieldsSettingsModel = otherFieldsSettingsModel;
            this.OrganizationFieldsSettingsModel = organizationFieldsSettingsModel;
            this.PlaceFieldsSettingsModel = placeFieldsSettingsModel;
            this.StateFieldsSettingsModel = stateFieldsSettingsModel;
        }

        [IsId]
        public int? CustomerId { get; private set; }

        [IsId]
        public int? LanguageId { get; private set; }

        [NotNull]
        public GeneralFieldsSettingsModel GeneralFieldsSettingsModel { get; private set; }

        [NotNull]
        public InventoryFieldsSettingsModel InventoryFieldsSettingsModel { get; private set; }

        [NotNull]
        public CommunicationFieldsSettingsModel CommunicationFieldsSettingsModel { get; private set; }

        [NotNull]
        public OtherFieldsSettingsModel OtherFieldsSettingsModel { get; private set; }

        [NotNull]
        public OrganizationFieldsSettingsModel OrganizationFieldsSettingsModel { get; private set; }

        [NotNull]
        public PlaceFieldsSettingsModel PlaceFieldsSettingsModel { get; private set; }

        [NotNull]
        public StateFieldsSettingsModel StateFieldsSettingsModel { get; private set; }
    }
}
