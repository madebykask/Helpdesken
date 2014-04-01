namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Printer
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class PrinterOverview
    {
        public PrinterOverview(int id, int? customerId, DateTime createdDate, DateTime changedDate, GeneralFields generalFields, InventoryFields inventoryFields, CommunicationFields communicationFields, OtherFields otherFields, OrganizationFields organizationFields, PlaceFields placeFields, StateFields stateFields)
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
            this.StateFields = stateFields;
        }

        [IsId]
        public int Id { get; private set; }

        [IsId]
        public int? CustomerId { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime ChangedDate { get; private set; }

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

        [NotNull]
        public StateFields StateFields { get; private set; }
    }
}