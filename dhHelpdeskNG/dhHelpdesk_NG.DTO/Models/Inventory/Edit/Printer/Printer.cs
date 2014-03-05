namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class Printer
    {
        private Printer(InventoryFields inventoryFields, GeneralFields generalFields, CommunicationFields communicationFields, OtherFields otherFields, OrganizationFields organizationFields, PlaceFields placeFields, StateFields stateFields)
        {
            this.InventoryFields = inventoryFields;
            this.GeneralFields = generalFields;
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

        public static Printer CreateNew(int? customerId, InventoryFields inventoryFields, GeneralFields generalFields, CommunicationFields communicationFields, OtherFields otherFields, OrganizationFields organizationFields, PlaceFields placeFields, StateFields stateFields, DateTime createdDate)
        {
            var businessModel = new Printer(inventoryFields, generalFields, communicationFields, otherFields, organizationFields, placeFields, stateFields) { CustomerId = customerId, CreatedDate = createdDate };

            return businessModel;
        }

        public static Printer CreateUpdated(int id, InventoryFields inventoryFields, GeneralFields generalFields, CommunicationFields communicationFields, OtherFields otherFields, OrganizationFields organizationFields, PlaceFields placeFields, StateFields stateFields, DateTime changedDate)
        {
            var businessModel = new Printer(inventoryFields, generalFields, communicationFields, otherFields, organizationFields, placeFields, stateFields) { Id = id, ChangedDate = changedDate };

            return businessModel;
        }

        public static Printer CreateForEdit(int id, InventoryFields inventoryFields, GeneralFields generalFields, CommunicationFields communicationFields, OtherFields otherFields, OrganizationFields organizationFields, PlaceFields placeFields, StateFields stateFields, DateTime createdDate, DateTime changedDate)
        {
            var businessModel = new Printer(inventoryFields, generalFields, communicationFields, otherFields, organizationFields, placeFields, stateFields) { Id = id, CreatedDate = createdDate, ChangedDate = changedDate };

            return businessModel;
        }
    }
}