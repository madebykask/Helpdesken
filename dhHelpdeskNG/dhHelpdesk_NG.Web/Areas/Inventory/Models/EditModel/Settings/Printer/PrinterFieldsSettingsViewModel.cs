namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Printer
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Shared;

    public class PrinterFieldsSettingsViewModel
    {
        public PrinterFieldsSettingsViewModel()
        {
        }

        public PrinterFieldsSettingsViewModel(
            int langaugeId,
            SelectList langauges,
            GeneralFieldsSettingsModel generalFieldsSettingsSettingsModel,
            InventoryFieldsSettingsModel inventoryFieldsSettingsSettingsModel,
            CommunicationFieldsSettingsModel communicationFieldsSettingsModel,
            OtherFieldsSettingsModel otherFieldsSettingsModel,
            OrganizationFieldsSettingsModel organizationFieldsSettingsModel,
            PlaceFieldsSettingsModel placeFieldsSettingsModel,
            StateFieldsSettingsModel stateFieldsSettingsModel)
        {
            this.LanguageId = langaugeId;
            this.Languages = langauges;
            this.GeneralFieldsSettingsModel = generalFieldsSettingsSettingsModel;
            this.InventoryFieldsSettingsModel = inventoryFieldsSettingsSettingsModel;
            this.CommunicationFieldsSettingsModel = communicationFieldsSettingsModel;
            this.OtherFieldsSettingsModel = otherFieldsSettingsModel;
            this.OrganizationFieldsSettingsModel = organizationFieldsSettingsModel;
            this.PlaceFieldsSettingsModel = placeFieldsSettingsModel;
            this.StateFieldsSettingsModel = stateFieldsSettingsModel;
        }

        [IsId]
        public int LanguageId { get; set; }

        [NotNull]
        public SelectList Languages { get; set; }

        [NotNull]
        public GeneralFieldsSettingsModel GeneralFieldsSettingsModel { get; set; }

        [NotNull]
        public InventoryFieldsSettingsModel InventoryFieldsSettingsModel { get; set; }

        [NotNull]
        public CommunicationFieldsSettingsModel CommunicationFieldsSettingsModel { get; set; }

        [NotNull]
        public OtherFieldsSettingsModel OtherFieldsSettingsModel { get; set; }

        [NotNull]
        public OrganizationFieldsSettingsModel OrganizationFieldsSettingsModel { get; set; }

        [NotNull]
        public PlaceFieldsSettingsModel PlaceFieldsSettingsModel { get; set; }

        [NotNull]
        public StateFieldsSettingsModel StateFieldsSettingsModel { get; set; }
    }
}
