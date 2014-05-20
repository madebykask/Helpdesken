namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class DynamicFieldModel
    {
        public const string True = "1";

        public DynamicFieldModel(
            int inventoryTypePropertyId,
            int groupId,
            string caption,
            int position,
            int maxSize,
            FieldTypes fieldTypes,
            string value)
        {
            this.InventoryTypePropertyId = inventoryTypePropertyId;
            this.GroupId = groupId;
            this.Caption = caption;
            this.Position = position;
            this.MaxSize = maxSize;
            this.FieldTypes = fieldTypes;
            this.Value = value;
        }

        [IsId]
        public int InventoryTypePropertyId { get; set; }

        public int GroupId { get; set; }

        [NotNullAndEmpty]
        public string Caption { get; set; }

        public int Position { get; set; }

        public int MaxSize { get; set; }

        public FieldTypes FieldTypes { get; set; }

        public string Value { get; set; }
    }

    public class DynamicFieldStringValue
    {
        public DynamicFieldStringValue(
            string value,
            int maxSize)
        {
            this.Value = value;
            this.MaxSize = maxSize;
        }

        //[MaxSizeFrom("MaxSize")]
        public string Value { get; set; }

        public int MaxSize { get; set; }
    }
}