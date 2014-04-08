namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class DynamicFieldModel
    {
        public const string True = "1";

        public DynamicFieldModel(
            int inventoryId,
            int inventoryTypePropertyId,
            int groupId,
            string caption,
            int position,
            int? maxSize,
            FieldTypes fieldTypes,
            string value)
        {
            this.InventoryId = inventoryId;
            this.InventoryTypePropertyId = inventoryTypePropertyId;
            this.GroupId = groupId;
            this.Caption = caption;
            this.Position = position;
            this.MaxSize = maxSize;
            this.FieldTypes = fieldTypes;
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

        public int? MaxSize { get; set; }

        public FieldTypes FieldTypes { get; set; }

        public string Value { get; set; }
    }
}