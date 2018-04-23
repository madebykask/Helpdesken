namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;

    public class PrinterForRead : Printer
    {
        public PrinterForRead(
            int id,
            InventoryFields inventoryFields,
            GeneralFields generalFields,
            CommunicationFields communicationFields,
            OtherFields otherFields,
            OrganizationFields organizationFields,
            PlaceFields placeFields,
            DateTime createdDate,
            DateTime changedDate,
            DateTime? syncDate,
            StateFields stateFields)
            : base(inventoryFields, generalFields, communicationFields, otherFields, organizationFields, placeFields)
        {
            this.Id = id;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.SyncDate = syncDate;
            this.StateFields = stateFields;
        }

        public DateTime CreatedDate { get; private set; }

        public DateTime ChangedDate { get; private set; }

        public DateTime? SyncDate { get; private set; }

        public StateFields StateFields { get; private set; }
    }
}