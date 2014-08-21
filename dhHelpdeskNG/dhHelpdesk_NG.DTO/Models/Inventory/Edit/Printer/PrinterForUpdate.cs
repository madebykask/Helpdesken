namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class PrinterForUpdate : Printer
    {
        public PrinterForUpdate(
            int id,
            InventoryFields inventoryFields,
            GeneralFields generalFields,
            CommunicationFields communicationFields,
            OtherFields otherFields,
            OrganizationFields organizationFields,
            PlaceFields placeFields,
            DateTime changedDate,
            int? changedByUserId)
            : base(inventoryFields, generalFields, communicationFields, otherFields, organizationFields, placeFields)
        {
            this.Id = id;
            this.ChangedDate = changedDate;
            this.ChangedByUserId = changedByUserId;
        }

        public DateTime ChangedDate { get; private set; }

        [IsId]
        public int? ChangedByUserId { get; private set; }
    }
}