namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Printer
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Shared;

    public class Printer
    {
        public Printer()
        {
        }

        public Printer(
            int id,
            int? customerId,
            ConfigurableFieldModel<DateTime> createdDate,
            ConfigurableFieldModel<DateTime> changedDate,
            GeneralFields generalFields,
            InventoryFields inventoryFields,
            CommunicationFields communicationFields,
            OtherFields otherFields,
            OrganizationFields organizationFields,
            PlaceFields placeFields)
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
        public GeneralFields GeneralFields { get; private set; }

        [NotNull]
        public InventoryFields InventoryFields { get; private set; }

        [NotNull]
        public CommunicationFields CommunicationFields { get; private set; }

        [NotNull]
        public OtherFields OtherFields { get; private set; }

        [NotNull]
        public OrganizationFields OrganizationFields { get; private set; }

        [NotNull]
        public PlaceFields PlaceFields { get; private set; }
    }
}