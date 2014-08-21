namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Printer
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public abstract class Printer : INewBusinessModel
    {
        protected Printer(
            InventoryFields inventoryFields,
            GeneralFields generalFields,
            CommunicationFields communicationFields,
            OtherFields otherFields,
            OrganizationFields organizationFields,
            PlaceFields placeFields)
        {
            this.InventoryFields = inventoryFields;
            this.GeneralFields = generalFields;
            this.CommunicationFields = communicationFields;
            this.OtherFields = otherFields;
            this.OrganizationFields = organizationFields;
            this.PlaceFields = placeFields;
        }

        [IsId]
        public int Id { get; set; }

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