namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Printer
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Shared;

    public class PrinterViewModel
    {
        public PrinterViewModel()
        {
        }

        public PrinterViewModel(
            GeneralFieldsModel generalFields,
            InventoryFieldsModel inventoryFields,
            CommunicationFieldsModel communicationFields,
            OtherFieldsModel otherFields,
            OrganizationFieldsViewModel organizationFieldsViewModel,
            PlaceFieldsViewModel placeFieldsViewModel)
        {
            this.GeneralFieldsModel = generalFields;
            this.InventoryFieldsModel = inventoryFields;
            this.CommunicationFieldsModel = communicationFields;
            this.OtherFieldsModel = otherFields;
            this.OrganizationFieldsViewModel = organizationFieldsViewModel;
            this.PlaceFieldsViewModel = placeFieldsViewModel;
        }

        [IsId]
        public int Id { get; set; }

        [IsId]
        public int? CustomerId { get; set; }

        public ConfigurableFieldModel<DateTime> CreatedDate { get; set; }

        public ConfigurableFieldModel<DateTime> ChangedDate { get; set; }

        [NotNull]
        public GeneralFieldsModel GeneralFieldsModel { get; set; }

        [NotNull]
        public InventoryFieldsModel InventoryFieldsModel { get; set; }

        [NotNull]
        public CommunicationFieldsModel CommunicationFieldsModel { get; set; }

        [NotNull]
        public OtherFieldsModel OtherFieldsModel { get; set; }

        [NotNull]
        public OrganizationFieldsViewModel OrganizationFieldsViewModel { get; set; }

        [NotNull]
        public PlaceFieldsViewModel PlaceFieldsViewModel { get; set; }
    }
}