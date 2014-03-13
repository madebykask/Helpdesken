namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class InventoryFieldSetting
    {
        public InventoryFieldSetting(int? inventoryTypeGroupId, string caption, int position, int propertySize, bool showInDetails, bool showInList)
        {
            this.InventoryTypeGroupId = inventoryTypeGroupId;
            this.Caption = caption;
            this.Position = position;
            this.PropertySize = propertySize;
            this.ShowInDetails = showInDetails;
            this.ShowInList = showInList;
        }

        [IsId]
        public int? InventoryTypeGroupId { get; private set; }

        [NotNullAndEmpty]
        public string Caption { get; private set; }

        public int Position { get; private set; }

        public int PropertySize { get; private set; }

        public bool ShowInDetails { get; private set; }

        public bool ShowInList { get; private set; }
    }
}