namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Printer
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Shared;

    public class PrinterModel
    {
        public PrinterModel()
        {
        }

        public PrinterModel(
            int id,
            int? customerId,
            ConfigurableFieldModel<DateTime> createdDate,
            ConfigurableFieldModel<DateTime> changedDate,
            GeneralFieldsModel generalFields,
            InventoryFieldsModel inventoryFields,
            CommunicationFieldsModel communicationFields,
            OtherFieldsModel otherFields,
            OrganizationFieldsModel organizationFields,
            PlaceFieldsModel placeFields)
        {
            this.Id = id;
            this.CustomerId = customerId;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.GeneralFields = generalFields;
            this.InventoryFields = inventoryFields;
            this.CommunicationFields = communicationFields;
            this.OtherFields = otherFields;
            this.OrganizationFields = organizationFields;
            this.PlaceFields = placeFields;
        }

        [IsId]
        public int Id { get; private set; }

        [IsId]
        public int? CustomerId { get; private set; }

        public ConfigurableFieldModel<DateTime> CreatedDate { get; private set; }

        public ConfigurableFieldModel<DateTime> ChangedDate { get; private set; }

        [NotNull]
        public GeneralFieldsModel GeneralFields { get; private set; }

        [NotNull]
        public InventoryFieldsModel InventoryFields { get; private set; }

        [NotNull]
        public CommunicationFieldsModel CommunicationFields { get; private set; }

        [NotNull]
        public OtherFieldsModel OtherFields { get; private set; }

        [NotNull]
        public OrganizationFieldsModel OrganizationFields { get; private set; }

        [NotNull]
        public PlaceFieldsModel PlaceFields { get; private set; }
    }
}