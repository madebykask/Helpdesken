namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class DynamicFieldModel
    {
        public const string True = "1";

        public DynamicFieldModel()
        {
        }

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

        public bool IsUseMaxSize
        {
            get
            {
                return this.FieldTypes == FieldTypes.Text;
            }
        }

        public FieldTypes FieldTypes { get; set; }

        [LocalizedMaxSizeFromAttribute("IsUseMaxSize", "MaxSize")]
        public string Value { get; set; }
    }
}