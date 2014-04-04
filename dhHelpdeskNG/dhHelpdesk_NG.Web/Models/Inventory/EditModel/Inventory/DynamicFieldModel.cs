namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Inventory
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class DynamicFieldModel
    {
        public DynamicFieldModel(int inventoryId, int inventoryTypePropertyId, int groupId, string caption, int position, int propertyType, string value)
        {
            this.InventoryId = inventoryId;
            this.InventoryTypePropertyId = inventoryTypePropertyId;
            this.GroupId = groupId;
            this.Caption = caption;
            this.Position = position;
            this.PropertyType = propertyType;
            this.Value = value;
        }

        [IsId]
        public int InventoryId { get; set; }

        [IsId]
        public int InventoryTypePropertyId { get; set; }

        [IsId]
        public int GroupId { get; set; }

        [NotNullAndEmpty]
        public string Caption { get; set; }

        public int Position { get; set; }

        public int PropertyType { get; set; }

        public string Value { get; set; }
    }
}