namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class PrinterForInsert : Printer
    {
        public PrinterForInsert(
            InventoryFields inventoryFields,
            GeneralFields generalFields,
            CommunicationFields communicationFields,
            OtherFields otherFields,
            OrganizationFields organizationFields,
            PlaceFields placeFields,
            int customerId,
            DateTime createdDate,
            int? changedByUserId)
            : base(inventoryFields, generalFields, communicationFields, otherFields, organizationFields, placeFields)
        {
            this.CustomerId = customerId;
            this.CreatedDate = createdDate;
            this.ChangedByUserId = changedByUserId;
        }

        [IsId]
        public int CustomerId { get; private set; }

        public DateTime CreatedDate { get; private set; }

        [IsId]
        public int? ChangedByUserId { get; private set; }
    }
}