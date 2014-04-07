namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Printer
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Server;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Shared;

    public class PrinterViewModel
    {
        public PrinterViewModel()
        {
        }

        public PrinterViewModel(
            int id,
            int? customerId,
            ConfigurableFieldModel<DateTime> createdDate,
            ConfigurableFieldModel<DateTime> changedDate,
            GeneralFieldsModel generalFields,
            InventoryFieldsModel inventoryFields,
            CommunicationFieldsModel communicationFields,
            OtherFieldsModel otherFields,
            OrganizationFieldsViewModel organizationFieldsViewModel,
            PlaceFieldsViewModel placeFieldsViewModel)
        {
            this.Id = id;
            this.CustomerId = customerId;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.GeneralFieldsModel = generalFields;
            this.InventoryFieldsModel = inventoryFields;
            this.CommunicationFieldsModel = communicationFields;
            this.OtherFieldsModel = otherFields;
            this.OrganizationFieldsViewModel = organizationFieldsViewModel;
            this.PlaceFieldsViewModel = placeFieldsViewModel;
        }

        [IsId]
        public int Id { get; private set; }

        [IsId]
        public int? CustomerId { get; private set; }

        public ConfigurableFieldModel<DateTime> CreatedDate { get; private set; }

        public ConfigurableFieldModel<DateTime> ChangedDate { get; private set; }

        [NotNull]
        public GeneralFieldsModel GeneralFieldsModel { get; private set; }

        [NotNull]
        public InventoryFieldsModel InventoryFieldsModel { get; private set; }

        [NotNull]
        public CommunicationFieldsModel CommunicationFieldsModel { get; private set; }

        [NotNull]
        public OtherFieldsModel OtherFieldsModel { get; private set; }

        [NotNull]
        public OrganizationFieldsViewModel OrganizationFieldsViewModel { get; private set; }

        [NotNull]
        public PlaceFieldsViewModel PlaceFieldsViewModel { get; private set; }
    }
}