namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer
{
    using System;

    using DH.Helpdesk.BusinessData.Attributes;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class Printer : BusinessModel
    {
        private Printer(ModelStates modelStates, InventoryFields inventoryFields, GeneralFields generalFields, CommunicationFields communicationFields, OtherFields otherFields, OrganizationFields organizationFields, PlaceFields placeFields)
            : base(modelStates)
        {
            this.InventoryFields = inventoryFields;
            this.GeneralFields = generalFields;
            this.CommunicationFields = communicationFields;
            this.OtherFields = otherFields;
            this.OrganizationFields = organizationFields;
            this.PlaceFields = placeFields;
        }

        [IsId]
        public int? CustomerId { get; private set; }

        [AllowRead(ModelStates.Updated | ModelStates.ForEdit)]
        public DateTime CreatedDate { get; private set; }

        [AllowRead(ModelStates.Updated | ModelStates.ForEdit)]
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

        public static Printer CreateNew(int? customerId, InventoryFields inventoryFields, GeneralFields generalFields, CommunicationFields communicationFields, OtherFields otherFields, OrganizationFields organizationFields, PlaceFields placeFields, DateTime createdDate)
        {
            var businessModel = new Printer(ModelStates.Created, inventoryFields, generalFields, communicationFields, otherFields, organizationFields, placeFields) { CustomerId = customerId, CreatedDate = createdDate };

            return businessModel;
        }

        public static Printer CreateUpdated(int id, InventoryFields inventoryFields, GeneralFields generalFields, CommunicationFields communicationFields, OtherFields otherFields, OrganizationFields organizationFields, PlaceFields placeFields, DateTime changedDate)
        {
            var businessModel = new Printer(ModelStates.Updated, inventoryFields, generalFields, communicationFields, otherFields, organizationFields, placeFields) { Id = id, ChangedDate = changedDate };

            return businessModel;
        }

        public static Printer CreateForEdit(int id, InventoryFields inventoryFields, GeneralFields generalFields, CommunicationFields communicationFields, OtherFields otherFields, OrganizationFields organizationFields, PlaceFields placeFields, DateTime createdDate, DateTime changedDate)
        {
            var businessModel = new Printer(ModelStates.ForEdit, inventoryFields, generalFields, communicationFields, otherFields, organizationFields, placeFields) { Id = id, CreatedDate = createdDate, ChangedDate = changedDate };

            return businessModel;
        }
    }
}